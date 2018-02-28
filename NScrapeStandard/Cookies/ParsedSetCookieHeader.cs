using System.Collections.Generic;

namespace NScrape.Cookies {
	internal class ParsedSetCookieHeader {
		public ParsedSetCookieHeader( IEnumerable<ParsedCookie> parsedCookies ) {
			ParsedCookies = parsedCookies;
		}

		public IEnumerable<ParsedCookie> ParsedCookies { get; private set; }
	}
}
