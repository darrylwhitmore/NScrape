using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace NScrape {
	/// <summary>
	/// Represents a web client that handles cookies and redirection.
	/// </summary>
	public class WebClient : IWebClient {
        /// <include file='IWebClient.xml' path='/IWebClient/AddingCookie/*'/>
		public event EventHandler<AddingCookieEventArgs> AddingCookie;

		/// <include file='IWebClient.xml' path='/IWebClient/SendingRequest/*'/>
        public event EventHandler<SendingRequestEventArgs> SendingRequest;

		/// <include file='IWebClient.xml' path='/IWebClient/ProcessingResponse/*'/>
        public event EventHandler<ProcessingResponseEventArgs> ProcessingResponse;

	    private readonly HttpStatusCode[] redirectionStatusCodes = {
		    HttpStatusCode.Moved,				// 301
		    HttpStatusCode.MovedPermanently,	// 301
		    HttpStatusCode.Found,				// 302
		    HttpStatusCode.Redirect,			// 302
		    HttpStatusCode.SeeOther,			// 303
		    HttpStatusCode.RedirectMethod		// 303
	    };
		private readonly CookieContainer cookieJar = new CookieContainer();

        /// <summary>
        /// Initializes a new instance of the <see cref="WebClient"/> class.
        /// </summary>
		public WebClient() {
	        // The request was aborted: Could not create SSL/TLS secure channel.
	        // https://github.com/google/google-api-dotnet-client/issues/911
			//
			// The request was aborted: Could not create SSL/TLS secure channel
			// https://stackoverflow.com/a/2904963/83861
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

			UserAgent = string.Format( CultureInfo.InvariantCulture, Properties.Resources.DefaultUserAgent, typeof(WebClient).GetTypeInfo().Assembly.GetName().Version );
        }

		private static void ConfigureHeader( HttpWebRequest webRequest, string headerName, string headerValue ) {
			// If the header in question corresponds to an HttpWebRequest property, set it.
			if ( string.Compare( headerName, CommonHeaders.Accept, StringComparison.OrdinalIgnoreCase ) == 0) {
				webRequest.Accept = headerValue;
			}
			else if ( string.Compare( headerName, CommonHeaders.Connection, StringComparison.OrdinalIgnoreCase ) == 0) {
				webRequest.Headers[HttpRequestHeader.Connection] = headerValue;
			}
			else if ( string.Compare( headerName, CommonHeaders.ContentLength, StringComparison.OrdinalIgnoreCase ) == 0 ) {
				if ( long.TryParse( headerValue, out var contentLength ) ) {
					webRequest.ContentLength = contentLength;
				}
			}
			else if ( string.Compare( headerName, CommonHeaders.ContentType, StringComparison.OrdinalIgnoreCase ) == 0) {
				webRequest.ContentType = headerValue;
			}
			else if ( string.Compare( headerName, CommonHeaders.Date, StringComparison.OrdinalIgnoreCase ) == 0) {
				if ( NScrapeUtility.TryParseHttpDate( headerValue, out DateTime _ ) ) {
					webRequest.Headers[HttpRequestHeader.Date] = headerValue;
				}
			}
			else if ( string.Compare( headerName, CommonHeaders.Expect, StringComparison.OrdinalIgnoreCase ) == 0) {
				webRequest.Headers[HttpRequestHeader.Expect] = headerValue;
			}
			else if ( string.Compare( headerName, CommonHeaders.Host, StringComparison.OrdinalIgnoreCase ) == 0) {
				webRequest.Headers[HttpRequestHeader.Host] = headerValue;
			}
			else if ( string.Compare( headerName, CommonHeaders.IfModifiedSince, StringComparison.OrdinalIgnoreCase ) == 0) {
				if ( NScrapeUtility.TryParseHttpDate( headerValue, out DateTime _ ) ) {
					webRequest.Headers[HttpRequestHeader.IfModifiedSince] = headerValue;
				}
			}
			else if ( string.Compare( headerName, CommonHeaders.Range, StringComparison.OrdinalIgnoreCase ) == 0) {
				// TODO: To support, we'd need to parse the provided range specification (which can include multiple ranges) and
				// make one or more calls to HttpWebRequest.AddRange(String, Int64, Int64). 
				// http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html
				throw new NotSupportedException( "The Range header is not currently supported." );
			}
			else if ( string.Compare( headerName, CommonHeaders.Referer, StringComparison.OrdinalIgnoreCase ) == 0) {
				webRequest.Referer = headerValue;
			}
			else if ( string.Compare( headerName, CommonHeaders.TransferEncoding, StringComparison.OrdinalIgnoreCase ) == 0) {
				throw new NotSupportedException( "The Transfer-Encoding header is not currently supported." );
			}
			else {
				// A header for which there is no property.
				webRequest.Headers[headerName] = headerValue;
			}
		}

		/// <include file='IWebClient.xml' path='/IWebClient/CookieJar/*'/>
        public CookieContainer CookieJar => cookieJar;

		private string GetMetaRefreshUrl( string html ) {
			// Look for a Meta refresh.
			var match = RegexCache.Instance.Regex( RegexLibrary.ParseMetaRefresh, RegexLibrary.ParseMetaRefreshOptions ).Match( html );

			if ( !match.Success ) {
				return null;
			}

			// If the Meta refresh is not within a NOSCRIPT block, we'll use it
			if ( match.Groups[RegexLibrary.ParseMetaRefreshStartNoScriptGroup].Value.Length == 0 && match.Groups[RegexLibrary.ParseMetaRefreshEndNoScriptGroup].Value.Length == 0 ) {
				return match.Groups[RegexLibrary.ParseMetaRefreshUrlGroup].Value;
			}

			return null;
		}

		/// <summary>
		/// Raises the <see cref="AddingCookie"/> event.
		/// </summary>
		/// <remarks>
		/// Called when a cookie is added to the <see cref="CookieJar"/>.
		/// </remarks>
		/// <param name="args">An <see cref="AddingCookieEventArgs"/> that contains the event data.</param>
		/// <seealso cref="AddingCookie"/>
		/// <seealso cref="AddingCookieEventArgs"/>
		protected virtual void OnAddingCookie( AddingCookieEventArgs args ) {
			AddingCookie?.Invoke( this, args );
		}

		/// <summary>
		/// Raises the <see cref="SendingRequest"/> event.
		/// </summary>
		/// <remarks>
		/// Called when a request is being sent.
		/// </remarks>
		/// <param name="args">A <see cref="SendingRequestEventArgs"/> that contains the event data.</param>
		/// <seealso cref="SendingRequest"/>
		protected virtual void OnSendingRequest( SendingRequestEventArgs args ) {
			SendingRequest?.Invoke( this, args );
		}

		/// <summary>
        /// Raises the <see cref="ProcessingResponse"/> event.
        /// </summary>
        /// <remarks>
        /// Called when a response has been received.
        /// </remarks>
        /// <param name="args">A <see cref="ProcessingResponseEventArgs"/> that contains the event data.</param>
        /// <seealso cref="SendingRequest"/>
        protected virtual void OnProcessingResponse(ProcessingResponseEventArgs args) {
			ProcessingResponse?.Invoke(this, args);
		}

		/// <include file='IWebClient.xml' path='/IWebClient/SendRequest_Uri/*'/>
        public WebResponse SendRequest( Uri destination ) {
            return SendRequest( new GetWebRequest( destination ) );
        }

		/// <include file='IWebClient.xml' path='/IWebClient/SendRequest_Uri_bool/*'/>
        public WebResponse SendRequest( Uri destination, bool autoRedirect ) {
            return SendRequest( new GetWebRequest( destination, autoRedirect ) );
        }

		/// <include file='IWebClient.xml' path='/IWebClient/SendRequest_Uri_string/*'/>
        public WebResponse SendRequest( Uri destination, string requestData ) {
            return SendRequest( new PostWebRequest( destination, requestData ) );
        }

		/// <include file='IWebClient.xml' path='/IWebClient/SendRequest_Uri_string_bool/*'/>
        public WebResponse SendRequest( Uri destination, string requestData, bool autoRedirect ) {
            return SendRequest( new PostWebRequest( destination, requestData, autoRedirect ) );
        }

		/// <include file='IWebClient.xml' path='/IWebClient/SendRequest_WebRequest/*'/>
        public WebResponse SendRequest( WebRequest webRequest ) {
            var httpWebRequest = (HttpWebRequest)System.Net.WebRequest.Create( webRequest.Destination );

			httpWebRequest.Method = webRequest.Type.ToString().ToUpperInvariant();

			// We shall handle redirects by hand so that we may capture cookies and properly
			// handle login forms.
			//
			// Automating Web Login With HttpWebRequest
			// https://www.stevefenton.co.uk/Content/Blog/Date/201210/Blog/Automating-Web-Login-With-HttpWebRequest/
			httpWebRequest.AllowAutoRedirect = false;

			// Default headers.
			httpWebRequest.Accept = "*/*";
			httpWebRequest.UserAgent = UserAgent;

			// Set and/or override any provided headers.
			foreach ( var headerName in webRequest.Headers.AllKeys ) {
				ConfigureHeader( httpWebRequest, headerName, webRequest.Headers[headerName] );
	        }

            httpWebRequest.CookieContainer = new CookieContainer();
            httpWebRequest.CookieContainer.Add( webRequest.Destination, cookieJar.GetCookies( webRequest.Destination ) );
            
			if ( webRequest.Type == WebRequestType.Post ) {
                var postRequest = (PostWebRequest)webRequest;

                var requestDataBytes = Encoding.UTF8.GetBytes( postRequest.RequestData );
                Stream requestStream = null;

				httpWebRequest.ContentLength = requestDataBytes.Length;
				httpWebRequest.ContentType = postRequest.ContentType;

				try {
                    requestStream = httpWebRequest.GetRequestStreamAsync().Result;
                    requestStream.Write( requestDataBytes, 0, requestDataBytes.Length );
                }
                finally {
	                requestStream?.Dispose();
                }
			}

			OnSendingRequest( new SendingRequestEventArgs( webRequest ) );

            WebResponse response;
			HttpWebResponse webResponse = null;

            try {
				try {
					webResponse = ( HttpWebResponse )httpWebRequest.GetResponseAsync().Result;
				}
				catch ( AggregateException ex ) {
					// While the line above executes without exception under the .Net Framework, under
					// .Net Core, it will throw an exception for non-successful (non-200) status codes.
					// However, the response we need is buried within the exception, so pull it out and
					// continue.
					//
					// See thread on the following page, notably the comment from davidsh on Sep 6, 2017:
					//
					// HttpWebRequest in .NET Core 2.0 throwing 301 Moved Permanently #23422
					// https://github.com/dotnet/corefx/issues/23422
					if ( ex.InnerExceptions.Count == 1 ) {
						if ( ex.InnerExceptions[0] is WebException webException ) {
							if ( webException.Response is HttpWebResponse httpWebResponse ) {
								webResponse = httpWebResponse;
							}
						}
					}

					if ( webResponse == null ) {
						// The exception was not as expected so we can't process it.
						throw;
					}
				}

	            OnProcessingResponse( new ProcessingResponseEventArgs( webResponse ) );

                if ( httpWebRequest.HaveResponse ) {
					var responseCookies = new CookieCollection { webResponse.Cookies };

					// Some cookies in the Set-Cookie header can be omitted from the response's CookieCollection. For example:
					//	Set-Cookie:ADCDownloadAuth=[long token];Version=1;Comment=;Domain=apple.com;Path=/;Max-Age=108000;HttpOnly;Expires=Tue, 03 May 2016 13:30:57 GMT
					// 
					// See also:
					// http://stackoverflow.com/questions/15103513/httpwebresponse-cookies-empty-despite-set-cookie-header-no-redirect
					//
					// To catch these, we parse the header manually and add any cookie that is missing.
					if ( webResponse.Headers.AllKeys.Contains( CommonHeaders.SetCookie ) ) {
						var responseCookieList = responseCookies.OfType<Cookie>().ToList();

						var host = httpWebRequest.Host;
						var cookies = NScrapeUtility.ParseSetCookieHeader( webResponse.Headers[CommonHeaders.SetCookie], host );

						foreach ( var cookie in cookies ) {
							if ( responseCookieList.All( c => c.Name != cookie.Name ) ) {
								responseCookies.Add( cookie );
							}
						}
					}

					// Handle cookies that are offered
					foreach ( Cookie responseCookie in responseCookies ) {
                        var cookieFound = false;

                        foreach ( Cookie existingCookie in cookieJar.GetCookies( webRequest.Destination ) ) {
                            if ( responseCookie.Name.Equals( existingCookie.Name ) ) {
                                existingCookie.Value = responseCookie.Value;
                                cookieFound = true;
                            }
                        }

                        if ( !cookieFound ) {
                            var args = new AddingCookieEventArgs( responseCookie );

                            OnAddingCookie( args );

                            if ( !args.Cancel ) {
                                // .NET Core seems to enfore the fact that the cookie domain _must_ start with a dot,
                                // so let's make sure that's the case.
                                if(!string.IsNullOrEmpty(responseCookie.Domain) && !responseCookie.Domain.StartsWith("."))
                                {
                                    responseCookie.Domain = "." + responseCookie.Domain;
                                }

                                string url = responseCookie.Secure ? "https://" : "http://";
                                url += responseCookie.Domain.Substring(1);

                                var uri = new Uri(url);
                                cookieJar.Add (uri, responseCookie );
                            }
                        }
                    }

					if ( redirectionStatusCodes.Contains( webResponse.StatusCode ) ) {
						// We have a redirected response, so get the new location.
                        var location = webResponse.Headers[CommonHeaders.Location];

						// Locations should always be absolute, per the RFC (http://tools.ietf.org/html/rfc2616#section-14.30), but
						// that won't always be the case.
						Uri redirectUri;
						if ( Uri.IsWellFormedUriString( location, UriKind.Absolute ) ) {
							redirectUri = new Uri( location );
						}
						else {
							redirectUri = new Uri( webRequest.Destination, new Uri( location, UriKind.Relative ) );
						}

						if ( webRequest.AutoRedirect ) {
							// Dispose webResponse before auto redirect, otherwise the connection will not be closed until all the auto redirect finished.
							// http://www.wadewegner.com/2007/08/systemnetwebexception-when-issuing-more-than-two-concurrent-webrequests/
							webResponse.Dispose();

							// We are auto redirecting, so make a recursive call to perform the redirect by hand.
							response = SendRequest( new GetWebRequest( redirectUri ) );
						}
						else {
							var responseUri = webResponse.ResponseUri;
							webResponse.Dispose();

							// We are not auto redirecting, so send the caller a redirect response.
							response = new RedirectedWebResponse( responseUri, webRequest, redirectUri );
						}
                    }
					else {
						// We have a non-redirected response.
						response = WebResponseFactory.CreateResponse( webResponse );

						if ( response.ResponseType == WebResponseType.Html ) {
							// We have an HTML response, so check for an old school Meta refresh tag
							var metaRefreshUrl = GetMetaRefreshUrl( ( ( HtmlWebResponse )response ).Html );

							if ( !string.IsNullOrWhiteSpace( metaRefreshUrl ) ) {
								// The page has a Meta refresh tag, so build the redirect Url
								var redirectUri = new Uri( response.ResponseUrl, metaRefreshUrl );

								if ( webRequest.AutoRedirect ) {
									// Dispose webResponse before auto redirect, otherwise the connection will not be closed until all the auto redirect finished.
									// http://www.wadewegner.com/2007/08/systemnetwebexception-when-issuing-more-than-two-concurrent-webrequests/
									response.Dispose();

									// We are auto redirecting, so make a recursive call to perform the redirect
									response = SendRequest( new GetWebRequest( redirectUri, httpWebRequest.AllowAutoRedirect ) );
								}
								else {
									var responseUrl = response.ResponseUrl;

									response.Dispose();

									// We are not auto redirecting, so send the caller a redirect response
									response = new RedirectedWebResponse( responseUrl, webRequest, redirectUri );
								}
							}
						}
					}
				}
				else {
					response = new ExceptionWebResponse( webRequest.Destination, new WebException( Properties.Resources.NoResponse ) );

					webResponse.Dispose();
				}
            }
            catch ( WebException ex ) {
                response = new ExceptionWebResponse( webRequest.Destination, ex );

	            webResponse?.Dispose();
            }

            return response;
        }

		/// <include file='IWebClient.xml' path='/IWebClient/UserAgent/*'/>
        public string UserAgent { get; set; }
	}
}
