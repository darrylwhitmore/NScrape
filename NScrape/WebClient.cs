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
	        UserAgent = string.Format( CultureInfo.InvariantCulture, NScrapeResources.DefaultUserAgent, Assembly.GetExecutingAssembly().GetName().Version );
        }

		private static void ConfigureHeader( HttpWebRequest webRequest, string headerName, string headerValue ) {
			// If the header in question corresponds to an HttpWebRequest property, set it.
			if ( string.Compare( headerName, CommonHeaders.Accept, StringComparison.OrdinalIgnoreCase ) == 0) {
				webRequest.Accept = headerValue;
			}
			else if ( string.Compare( headerName, CommonHeaders.Connection, StringComparison.OrdinalIgnoreCase ) == 0) {
				webRequest.Connection = headerValue;
			}
			else if ( string.Compare( headerName, CommonHeaders.ContentLength, StringComparison.OrdinalIgnoreCase ) == 0) {
				webRequest.ContentLength = Convert.ToInt64( headerValue );
			}
			else if ( string.Compare( headerName, CommonHeaders.ContentType, StringComparison.OrdinalIgnoreCase ) == 0) {
				webRequest.ContentType = headerValue;
			}
			else if ( string.Compare( headerName, CommonHeaders.Date, StringComparison.OrdinalIgnoreCase ) == 0) {
				DateTime parsedDateTime;

				if ( NScrapeUtility.TryParseHttpDate( headerValue, out parsedDateTime ) ) {
					webRequest.Date = parsedDateTime;
				}
			}
			else if ( string.Compare( headerName, CommonHeaders.Expect, StringComparison.OrdinalIgnoreCase ) == 0) {
				webRequest.Expect = headerValue;
			}
			else if ( string.Compare( headerName, CommonHeaders.Host, StringComparison.OrdinalIgnoreCase ) == 0) {
				webRequest.Host = headerValue;
			}
			else if ( string.Compare( headerName, CommonHeaders.IfModifiedSince, StringComparison.OrdinalIgnoreCase ) == 0) {
				DateTime parsedDateTime;

				if ( NScrapeUtility.TryParseHttpDate( headerValue, out parsedDateTime ) ) {
					webRequest.IfModifiedSince = parsedDateTime;
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
				webRequest.Headers.Add( headerName, headerValue );
			}
		}

		/// <include file='IWebClient.xml' path='/IWebClient/CookieJar/*'/>
        public CookieContainer CookieJar { get { return cookieJar; } }

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
			if ( AddingCookie != null ) {
				AddingCookie( this, args );
			}
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
			if ( SendingRequest != null ) {
				SendingRequest( this, args );
			}
		}

        /// <summary>
        /// Raises the <see cref="ProcessingResponse"/> event.
        /// </summary>
        /// <remarks>
        /// Called when a response has been received.
        /// </remarks>
        /// <param name="args">A <see cref="ProcessingResponseEventArgs"/> that contains the event data.</param>
        /// <seealso cref="SendingRequest"/>
        protected virtual void OnProcessingResponse(ProcessingResponseEventArgs args)
        {
            if (ProcessingResponse != null)
            {
                ProcessingResponse(this, args);
            }
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
            httpWebRequest.CookieContainer.Add( cookieJar.GetCookies( webRequest.Destination ) );
            
			if ( webRequest.Type == WebRequestType.Post ) {
                var postRequest = (PostWebRequest)webRequest;

                var requestDataBytes = Encoding.UTF8.GetBytes( postRequest.RequestData );
                Stream requestStream = null;

                httpWebRequest.ContentLength = requestDataBytes.Length;
                httpWebRequest.ContentType = postRequest.ContentType;
                httpWebRequest.ServicePoint.Expect100Continue = false;

                try {
                    requestStream = httpWebRequest.GetRequestStream();
                    requestStream.Write( requestDataBytes, 0, requestDataBytes.Length );
                }
                finally {
                    if ( requestStream != null ) {
                        requestStream.Close();
                    }
                }
            }

			OnSendingRequest( new SendingRequestEventArgs( webRequest ) );

            WebResponse response;
			HttpWebResponse webResponse = null;

            try {
                webResponse = ( HttpWebResponse )httpWebRequest.GetResponse();

	            OnProcessingResponse( new ProcessingResponseEventArgs( webResponse ) );

                if ( httpWebRequest.HaveResponse ) {
                    // Process cookies that the .NET client 'forgot' to include,
                    // see http://stackoverflow.com/questions/15103513/httpwebresponse-cookies-empty-despite-set-cookie-header-no-redirect
                    // for more details;
                    // an example cookie which is not parsed is this one:
                    //
                    // Set-Cookie:ADCDownloadAuth=[long token];Version=1;Comment=;Domain=apple.com;Path=/;Max-Age=108000;HttpOnly;Expires=Tue, 03 May 2016 13:30:57 GMT

                    // Handle cookies that are offered
                    CookieCollection cookies = new CookieCollection();
                    cookies.Add(webResponse.Cookies);

                    if (webResponse.Headers.AllKeys.Contains("Set-Cookie"))
                    {
                        var alternateCookies = CookieParser.GetAllCookiesFromHeader(webResponse.Headers["Set-Cookie"], httpWebRequest.Host);

                        foreach (Cookie alternateCookie in alternateCookies)
                        {
                            // Match cookies by name, and only add the cookie if it was not found previously
                            if (!cookies.OfType<Cookie>().Any(c => c.Name == alternateCookie.Name))
                            {
                                cookies.Add(alternateCookie);
                            }
                        }
                    }

                    foreach ( Cookie responseCookie in cookies ) {
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
                                cookieJar.Add( responseCookie );
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
							// We are auto redirecting, so make a recursive call to perform the redirect by hand.
							response = SendRequest( new GetWebRequest( redirectUri ) );
						}
						else {
							// We are not auto redirecting, so send the caller a redirect response.
							response = new RedirectedWebResponse( webResponse.ResponseUri, webRequest, redirectUri );
						}

						webResponse.Dispose();
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
					response = new ExceptionWebResponse( webRequest.Destination, new WebException( NScrapeResources.NoResponse ) );

					webResponse.Dispose();
				}
            }
            catch ( WebException ex ) {
                response = new ExceptionWebResponse( webRequest.Destination, ex );

				if ( webResponse != null ) {
					webResponse.Dispose();
				}
            }

            return response;
        }

		/// <include file='IWebClient.xml' path='/IWebClient/UserAgent/*'/>
        public string UserAgent { get; set; }
	}
}
