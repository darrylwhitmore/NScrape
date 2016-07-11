using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;

namespace NScrape {
	/// <summary>
	/// Provides miscellaneous utility functions.
	/// </summary>
	public static class NScrapeUtility {
		/// <summary>
		/// Parse cookies from from a <c>Set-Cookie</c> header.
		/// </summary>
		/// <param name="setCookieHeader">The <c>Set-Cookie</c> header.</param>
		/// <param name="hostName">
		/// The host name of the server to which the request was sent. Cookies will be scoped to this host name, unless otherwise
		/// specified in the <c>Set-Cookie</c> declaration.
		/// </param>
		/// <returns>An enumeration of <see cref="Cookie"/>.</returns>
		public static IEnumerable<Cookie> ParseSetCookieHeader( string setCookieHeader, string hostName ) {
			if ( string.IsNullOrWhiteSpace( hostName ) ) {
				throw new ArgumentException( "A host name is required.", nameof( hostName ) );
			}

			var cookies = new List<Cookie>();

			// Remove any new line character
			var cookieHeader = ( setCookieHeader ?? string.Empty ).Replace( "\r", string.Empty );
			cookieHeader = cookieHeader.Replace( "\n", string.Empty );

			// Split on the ',' sign
			var parts = cookieHeader.Split( new[] { ',' }, StringSplitOptions.RemoveEmptyEntries );
			var cookieLines = new List<string>();
			var i = 0;

			while ( i < parts.Length ) {
				// Correct for the expires field, which can include a comma
				if ( parts[i].IndexOf( "expires=", StringComparison.OrdinalIgnoreCase ) > 0 ) {
					if ( i + 1 < parts.Length ) {
						cookieLines.Add( parts[i] + "," + parts[i + 1] );
						i++;
					}
					else {
						cookieLines.Add( parts[i] );
					}
				}
				else {
					cookieLines.Add( parts[i] );
				}

				i++;
			}

			foreach ( var cookieLine in cookieLines ) {
				parts = cookieLine.Split( ';' );

				var cookie = new Cookie {
					Path = "/",
					Domain = hostName
				};

				var hasMaxAge = false;

				// At index 0, we'll have the name of the cookie
				var cookieNameAndValue = parts[0];

				if ( cookieNameAndValue != string.Empty ) {
					var firstEqual = cookieNameAndValue.IndexOf( "=", StringComparison.InvariantCulture );

					if ( firstEqual != -1 ) { 
						var firstName = cookieNameAndValue.Substring( 0, firstEqual );
						var allValue = cookieNameAndValue.Substring( firstEqual + 1, cookieNameAndValue.Length - ( firstEqual + 1 ) );
						cookie.Name = firstName;
						cookie.Value = allValue;

						foreach ( var cookiePart in parts.Skip( 1 ) ) {
							var pair = cookiePart.Trim().Split( new[] { '=' }, 2 );

							if ( pair.Length == 1 ) {
								if ( string.Equals( pair[0], "httponly", StringComparison.OrdinalIgnoreCase ) ) {
									cookie.HttpOnly = true;
								}
								else if ( string.Equals( pair[0], "secure", StringComparison.OrdinalIgnoreCase ) ) {
									cookie.Secure = true;
								}
							}
							else {
								if ( string.Equals( pair[0], "path", StringComparison.OrdinalIgnoreCase ) ) {
									if ( pair[1] != string.Empty ) {
										cookie.Path = pair[1];
									}
									else {
										cookie.Path = "/";
									}
								}
								else if ( string.Equals( pair[0], "domain", StringComparison.OrdinalIgnoreCase ) ) {
									if ( pair[1] != string.Empty ) {
										cookie.Domain = pair[1];
									}
									else {
										cookie.Domain = hostName;
									}
								}
								else if ( string.Equals( pair[0], "max-age", StringComparison.OrdinalIgnoreCase ) ) {
									var maxAge = int.Parse( pair[1] );
									cookie.Expires = DateTime.Now.AddSeconds( maxAge );

									// Prevent the 'expires' value from overriding the value that was determined using max-age
									hasMaxAge = true;
								}
								else if ( !hasMaxAge && string.Equals( pair[0], "expires", StringComparison.OrdinalIgnoreCase ) ) {
									// max-age takes precedence over "expires"
									DateTime parsedDate;

									if ( TryParseHttpDate( pair[1], out parsedDate ) ) {
										cookie.Expires = parsedDate.ToLocalTime();
									}
								}
								else if ( string.Equals( pair[0], "version", StringComparison.OrdinalIgnoreCase ) ) {
									if ( pair[1] != string.Empty ) {
										int version;

										if ( int.TryParse( pair[1], out version ) ) {
											cookie.Version = version;
										}
									}
								}
							}
						}

						cookies.Add( cookie );
					}
				}
			}

			return cookies;
		}

		/// <summary>
		/// Converts the specified HTTP string representation of a date and time to its <see cref="DateTime"/> equivalent
		/// </summary>
		/// <param name="httpDate">Contains the HTTP date string.</param>
		/// <param name="parsedDate">When this method returns, contains the <see cref="DateTime"/> value equivalent to the date and time
		/// contained in <i>httpDate</i>, if the conversion succeeded, or <see cref="DateTime.MinValue">MinValue</see> if the conversion failed.
		/// </param>
		/// <remarks>
		/// Handles conversion of the HTTP date formats specified in section <b>7.1.1.1. Date/Time Formats</b> of
		/// <see href="http://tools.ietf.org/html/rfc7231">RFC 7231</see>.
		/// </remarks>
		/// <returns><b>true</b> if the date was successfully parsed; <b>false</b> otherwise.</returns>
		public static bool TryParseHttpDate( string httpDate, out DateTime parsedDate ) {
			// http://tools.ietf.org/html/rfc7231#section-7.1.1.1
			var formats = new[] {
				"r",							// preferred
				"dddd, dd-MMM-yy HH:mm:ss GMT",	// obsolete RFC 850 format 
				"ddd MMM  d HH:mm:ss yyyy"		// ANSI C's asctime() format
			};

			// Try with strictly defined formats first.
			var result = DateTime.TryParseExact( httpDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out parsedDate );

			if ( !result ) {
				// Let DateTime try to make sense of the date string.
				result = DateTime.TryParse( httpDate, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out parsedDate );
			}

			return result;
		}
	}
}
