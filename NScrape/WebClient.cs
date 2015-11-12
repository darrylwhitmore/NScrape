using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace NScrape {
	/// <summary>
	/// Represents a web client that handles cookies and redirection.
	/// </summary>
	public class WebClient : IWebClient {
        /// <include file='IWebClient.xml' path='/IWebClient/AddingCookie'/>
		public event EventHandler<AddingCookieEventArgs> AddingCookie;

        /// <include file='IWebClient.xml' path='/IWebClient/SendingRequest'/>
        public event EventHandler<SendingRequestEventArgs> SendingRequest;

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
			if ( String.Compare( headerName, CommonHeaders.Accept, StringComparison.OrdinalIgnoreCase ) == 0) {
				webRequest.Accept = headerValue;
			}
			else if ( String.Compare( headerName, CommonHeaders.Connection, StringComparison.OrdinalIgnoreCase ) == 0) {
				webRequest.Connection = headerValue;
			}
			else if ( String.Compare( headerName, CommonHeaders.ContentLength, StringComparison.OrdinalIgnoreCase ) == 0) {
				webRequest.ContentLength = Convert.ToInt64( headerValue );
			}
			else if ( String.Compare( headerName, CommonHeaders.ContentType, StringComparison.OrdinalIgnoreCase ) == 0) {
				webRequest.ContentType = headerValue;
			}
			else if ( String.Compare( headerName, CommonHeaders.Date, StringComparison.OrdinalIgnoreCase ) == 0) {
				DateTime parsedDateTime;

				if ( NScrapeUtility.TryParseHttpDate( headerValue, out parsedDateTime ) ) {
					webRequest.Date = parsedDateTime;
				}
			}
			else if ( String.Compare( headerName, CommonHeaders.Expect, StringComparison.OrdinalIgnoreCase ) == 0) {
				webRequest.Expect = headerValue;
			}
			else if ( String.Compare( headerName, CommonHeaders.Host, StringComparison.OrdinalIgnoreCase ) == 0) {
				webRequest.Host = headerValue;
			}
			else if ( String.Compare( headerName, CommonHeaders.IfModifiedSince, StringComparison.OrdinalIgnoreCase ) == 0) {
				DateTime parsedDateTime;

				if ( NScrapeUtility.TryParseHttpDate( headerValue, out parsedDateTime ) ) {
					webRequest.IfModifiedSince = parsedDateTime;
				}
			}
			else if ( String.Compare( headerName, CommonHeaders.Range, StringComparison.OrdinalIgnoreCase ) == 0) {
				// TODO: To support, we'd need to parse the provided range specification (which can include multiple ranges) and
				// make one or more calls to HttpWebRequest.AddRange(String, Int64, Int64). 
				// http://www.w3.org/Protocols/rfc2616/rfc2616-sec14.html
				throw new NotSupportedException( "The Range header is not currently supported." );
			}
			else if ( String.Compare( headerName, CommonHeaders.Referer, StringComparison.OrdinalIgnoreCase ) == 0) {
				webRequest.Referer = headerValue;
			}
			else if ( String.Compare( headerName, CommonHeaders.TransferEncoding, StringComparison.OrdinalIgnoreCase ) == 0) {
				throw new NotSupportedException( "The Transfer-Encoding header is not currently supported." );
			}
			else {
				// A header for which there is no property.
				webRequest.Headers.Add( headerName, headerValue );
			}
		}

        /// <include file='IWebClient.xml' path='/IWebClient/CookieJar'/>
        public CookieContainer CookieJar { get { return cookieJar; } }

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

		private static string ReadResponseText( HttpWebResponse response, Encoding encoding ) {
			var s = response.GetResponseStream();

			if ( s != null ) {
                StreamReader sr;

                if(response.ContentEncoding == "gzip") {
                    sr = new StreamReader( new GZipStream(s, CompressionMode.Decompress), encoding );
                }
                else { 
				    sr = new StreamReader( s, encoding );
                }

				var content = sr.ReadToEnd();

				sr.Close();

				return content;
			}

			return null;
		}

        /// <include file='IWebClient.xml' path='/IWebClient/SendRequest_Uri'/>
        public WebResponse SendRequest( Uri destination ) {
            return SendRequest( new GetWebRequest( destination ) );
        }

        /// <include file='IWebClient.xml' path='/IWebClient/SendRequest_Uri_bool'/>
        public WebResponse SendRequest( Uri destination, bool autoRedirect ) {
            return SendRequest( new GetWebRequest( destination, autoRedirect ) );
        }

        /// <include file='IWebClient.xml' path='/IWebClient/SendRequest_Uri_string'/>
        public WebResponse SendRequest( Uri destination, string requestData ) {
            return SendRequest( new PostWebRequest( destination, requestData ) );
        }

        /// <include file='IWebClient.xml' path='/IWebClient/SendRequest_Uri_string_bool'/>
        public WebResponse SendRequest( Uri destination, string requestData, bool autoRedirect ) {
            return SendRequest( new PostWebRequest( destination, requestData, autoRedirect ) );
        }

        /// <include file='IWebClient.xml' path='/IWebClient/SendRequest_WebRequest'/>
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

            try {
                var webResponse = ( HttpWebResponse )httpWebRequest.GetResponse();

                if ( httpWebRequest.HaveResponse ) {
                    // Handle cookies that are offered
                    foreach ( Cookie responseCookie in webResponse.Cookies ) {
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
						// We have a redirected response.

						// Get the new location.
                        Uri redirectUri;
                        var location = webResponse.Headers[CommonHeaders.Location];

						// Locations should always be absolute, per the RFC (http://tools.ietf.org/html/rfc2616#section-14.30), but
						// that won't always be the case.
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
                    }
                    else {
						// We have a regular response.

						var contentType = webResponse.Headers[CommonHeaders.ContentType];
                        response = WebResponseFactory.CreateResponse(webResponse, contentType);

                        if(response == null) {
							if ( contentType.StartsWith( "text/html", StringComparison.OrdinalIgnoreCase ) ) {
                                var encoding = WebResponseFactory.GetEncoding( webResponse );
								var html = WebResponseFactory.ReadResponseText( webResponse, encoding );

								var haveRefresh = false;

								// Check for an old school Meta refresh tag
								var match = RegexCache.Instance.Regex( RegexLibrary.ParseMetaRefresh, RegexLibrary.ParseMetaRefreshOptions ).Match( html );

								if ( match.Success ) {
									// If the Meta refresh is not within a NOSCRIPT block, we'll use it
									if ( match.Groups[RegexLibrary.ParseMetaRefreshStartNoScriptGroup].Value.Length == 0 && match.Groups[RegexLibrary.ParseMetaRefreshEndNoScriptGroup].Value.Length == 0 ) {
										haveRefresh = true;
									}
								}

								if ( haveRefresh ) {
									// The page has a Meta refresh tag, so build the redirect Url
									var redirectUri = new Uri( webResponse.ResponseUri, match.Groups[RegexLibrary.ParseMetaRefreshUrlGroup].Value );

									if ( webRequest.AutoRedirect ) {
										// We are auto redirecting, so make a recursive call to perform the redirect
										response = SendRequest( new GetWebRequest( redirectUri, httpWebRequest.AllowAutoRedirect ) );
									}
									else {
										// We are not auto redirecting, so send the caller a redirect response
										response = new RedirectedWebResponse( webResponse.ResponseUri, webRequest, redirectUri );
									}
								}
								else {
									// Just a regular Html page
									response = new HtmlWebResponse( true, webResponse.ResponseUri, html, encoding );
								}
							}
							else {
								response = new UnsupportedWebResponse( contentType, webResponse.ResponseUri );
							}
						}
                    }

                    webResponse.Close(); // also closes stream opened via GetResponseStream()
                }
                else {
                    response = new ExceptionWebResponse( new WebException( NScrapeResources.NoResponse ), webRequest.Destination );
                }
            }
            catch ( WebException ex ) {
                response = new ExceptionWebResponse( ex, webRequest.Destination );
            }

            return response;
        }

        /// <include file='IWebClient.xml' path='/IWebClient/UserAgent'/>
        public string UserAgent { get; set; }
	}
}
