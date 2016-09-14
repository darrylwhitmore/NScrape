using Sprache;

namespace NScrape.Cookies {
	// Building an External DSL in C#
	// http://nblumhardt.com/2010/01/building-an-external-dsl-in-c/
	//
	// Sprache
	// https://github.com/sprache/Sprache
	internal static class SetCookieHeaderGrammar {
		public static readonly Parser<char> EqualsSeparator = Parse.Char( '=' );

		public static readonly Parser<char> SemiColonSeparator = Parse.Char( ';' );

		public static readonly Parser<char> CommaSeparator = Parse.Char( ',' );

		public static readonly Parser<char> EqualsOrSemiColonOrCommaSeparator = EqualsSeparator.Or( SemiColonSeparator ).Or( CommaSeparator );

		public static readonly Parser<char> SemiColonOrCommaSeparator = SemiColonSeparator.Or( CommaSeparator );

		public static readonly Parser<string> Name = Parse.AnyChar.Except( EqualsOrSemiColonOrCommaSeparator ).AtLeastOnce().Text().Token();

		public static readonly Parser<string> Value = Parse.AnyChar.Except( SemiColonOrCommaSeparator ).Many().Text().Token();

		public static readonly Parser<string> DateValue =
			from beforeComma in Parse.Letter.Many()
			from comma in Parse.Char( ',' ).Optional()
			from whitespace in Parse.WhiteSpace
			from afterComma in Parse.AnyChar.Except( SemiColonOrCommaSeparator ).Many()
			select string.Concat( beforeComma ) + ( comma.IsEmpty ? "" : string.Concat( comma.Get() ) ) + string.Concat( whitespace ) + string.Concat( afterComma );

		public static readonly Parser<CookieNameValuePair> ExpiresNameValue =
			from name in Parse.IgnoreCase( "expires" ).Text()
			from equalSign in EqualsSeparator.Token()
			from value in DateValue
			from delimiter in SemiColonSeparator.Optional().Token()
			select new CookieNameValuePair( name, value );

		public static readonly Parser<CookieNameValuePair> NameValue =
			from name in Name
			from equalSign in EqualsSeparator.Token()
			from value in Value
			from delimiter in SemiColonSeparator.Optional().Token()
			select new CookieNameValuePair( name, value );

		public static readonly Parser<CookieNameValuePair> NameOnly =
			from name in Name
			from delimiter in SemiColonSeparator.Optional().Token()
			select new CookieNameValuePair( name );

		public static readonly Parser<ParsedCookie> Cookie =
			from cookiePair in NameValue
			from cookieAttributes in ExpiresNameValue.Or( NameValue.Or( NameOnly ) ).Many()
			from delimiter in CommaSeparator.Optional()
			select new ParsedCookie( cookiePair, cookieAttributes );

		public static readonly Parser<ParsedSetCookieHeader> SetCookieHeader =
			from cookies in Cookie.Many()
			select new ParsedSetCookieHeader( cookies );
	}
}
