using System.Collections.Generic;

namespace NScrape.Cookies {
	internal class ParsedCookie {
		public ParsedCookie( IEnumerable<CookieNameValuePair> cookieNameValuePairs ) {
			CookieNameValuePairs = cookieNameValuePairs;
		}

		public IEnumerable<CookieNameValuePair> CookieNameValuePairs { get; private set; }
	}
}
