using System.Collections.Generic;

namespace NScrape.Cookies {
	internal class ParsedCookie {
		public ParsedCookie( CookieNameValuePair cookiePair, IEnumerable<CookieNameValuePair> attributePairs ) {
			CookiePair = cookiePair;
			AttributePairs = attributePairs;
		}

		public CookieNameValuePair CookiePair { get; set; }

		public IEnumerable<CookieNameValuePair> AttributePairs { get; private set; }
	}
}
