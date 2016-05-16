using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;

namespace NScrape
{
    /// <summary>
    /// Provides methods for parsing the value of the <c>Set-Cookie</c> header. The standard .NET WebResponse class does not correctly parse all cookies;
    /// this class tries to correct that.
    /// </summary>
    /// <seealso href="http://stackoverflow.com/questions/15103513/httpwebresponse-cookies-empty-despite-set-cookie-header-no-redirect"/>
    public static class CookieCollectionExtensions
    {
        /// <summary>
        /// Adds cookies extracted from a <c>Set-Cookie</c> header to a <see cref="CookieCollection"/>.
        /// </summary>
        /// <param name="cookies">
        /// The <see cref="CookieCollection"/> to which to add the extracted cookies.
        /// </param>
        /// <param name="setCookieHeader">
        /// The value of the <c>Set-Cookie</c> header.
        /// </param>
        /// <param name="hostName">
        /// The host name of the server to which the request was sent. Cookies will be scoped to this host name, unless otherwise
        /// specified in the <c>Set-Cookie</c> declaration.
        /// </param>
        public static void Parse(this CookieCollection cookies, string setCookieHeader, string hostName)
        {
            var cookieDeclarations = ExtractCookieDeclarations(setCookieHeader);

            foreach (var cookieDeclaration in cookieDeclarations)
            {
                var cookieParts = cookieDeclaration.Split(';');

                Cookie cookie = new Cookie();
                cookie.Path = "/";
                cookie.Domain = hostName;

                bool hasMaxAge = false;

                // At index 0, we'll have the name of the cookie
                var cookieNameAndValue = cookieParts[0];

                if (cookieNameAndValue != string.Empty)
                {
                    int firstEqual = cookieNameAndValue.IndexOf("=");
                    string firstName = cookieNameAndValue.Substring(0, firstEqual);
                    string allValue = cookieNameAndValue.Substring(firstEqual + 1, cookieNameAndValue.Length - (firstEqual + 1));
                    cookie.Name = firstName;
                    cookie.Value = allValue;
                }

                foreach (var cookiePart in cookieParts.Skip(1))
                {
                    var pair = cookiePart.Split(new char[] { '=' }, 2);

                    if (pair.Length == 1)
                    {
                        if (string.Equals(pair[0], "httponly", StringComparison.InvariantCultureIgnoreCase))
                        {
                            cookie.HttpOnly = true;
                        }
                        else if (string.Equals(pair[0], "secure", StringComparison.InvariantCultureIgnoreCase))
                        {
                            cookie.Secure = true;
                        }
                    }
                    else
                    {
                        if (string.Equals(pair[0], "path", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (pair[1] != string.Empty)
                            {
                                cookie.Path = pair[1];
                            }
                            else
                            {
                                cookie.Path = "/";
                            }
                        }
                        else if (string.Equals(pair[0], "domain", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (pair[1] != string.Empty)
                            {
                                cookie.Domain = pair[1];
                            }
                            else
                            {
                                cookie.Domain = hostName;
                            }
                        }
                        else if (string.Equals(pair[0], "max-age", StringComparison.InvariantCultureIgnoreCase))
                        {
                            var maxAge = int.Parse(pair[1]);
                            cookie.Expires = DateTime.Now.AddSeconds(maxAge);

                            // Prevent the 'expires' value from overriding the value that was determined using max-age
                            hasMaxAge = true;
                        }
                        else if (!hasMaxAge && string.Equals(pair[0], "expires", StringComparison.InvariantCultureIgnoreCase))
                        {
                            // max-age takes precedence over "expires"
                            cookie.Expires = DateTime.ParseExact(pair[1], "r", null).ToLocalTime();
                        }
                    }
                }

                // Match cookies by name, and only add the cookie if it was not found previously
                if (!cookies.OfType<Cookie>().Any(c => c.Name == cookie.Name))
                {
                    cookies.Add(cookie);
                }
            }
        }

        /// <summary>
        /// Splits the <c>Set-Cookie</c> hierder into the individual strings that define a cookie. Corrects for lines that contain an expiry
        /// date which include a <c>,</c> in the expires value - such as <c>Expires=Tue, 03 May 2016 14:35:28 GMT</c>
        /// </summary>
        /// <param name="setCookieHeader"></param>
        /// <returns></returns>
        private static Collection<string> ExtractCookieDeclarations(string setCookieHeader)
        {
            // Remove any new line character
            setCookieHeader = setCookieHeader.Replace("\r", "");
            setCookieHeader = setCookieHeader.Replace("\n", "");

            // Split on the ',' sign
            string[] setCookieParts = setCookieHeader.Split(',');

            Collection<string> cookieLines = new Collection<string>();
            int i = 0;
            int n = setCookieParts.Length;

            while (i < n)
            {
                // Correct for the expires field, which can include a comma
                if (setCookieParts[i].IndexOf("expires=", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    cookieLines.Add(setCookieParts[i] + "," + setCookieParts[i + 1]);
                    i++;
                }
                else
                {
                    cookieLines.Add(setCookieParts[i]);
                }
                i++;
            }

            return cookieLines;
        }
    }
}