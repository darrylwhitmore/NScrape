﻿using System.Linq;
using NScrape.Cookies;
using Sprache;
using Xunit;

namespace NScrape.Test.Cookies {
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Assertions", "xUnit2013:Do not use equality check to check for collection size.", Justification = "<Pending>" )]
	public class SetCookieHeaderGrammarTests {
		private const string ExpiresName = "expires";
		private const string ExpiresNameUpperCase = "EXPIRES";
		private const string ExpiresNameMixedCase = "Expires";
		private const string PreferredFormatDateValue = "Sun, 06 Nov 1994 08:49:37 GMT";
		private const string Rfc850FormatDateValue = "Sunday, 06-Nov-94 08:49:37 GMT";
		private const string AnsiCasctimeFormatDateValue = "Sun Nov  6 08:49:37 1994";

		[Fact]
		public void NameValueTests() {
			// Nothing to parse
			Assert.Throws<ParseException>( () => SetCookieHeaderGrammar.NameValue.Parse( string.Empty ) );

			// Basic
			var test = "foo=bar";
			var nameValue = SetCookieHeaderGrammar.NameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( "foo", nameValue.Name );
			Assert.Equal( "bar", nameValue.Value );
			// Comma cookie separator, no second cookie
			test += ",";
			nameValue = SetCookieHeaderGrammar.NameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( "foo", nameValue.Name );
			Assert.Equal( "bar", nameValue.Value );

			// No value
			test = "foo=;";
			nameValue = SetCookieHeaderGrammar.NameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( "foo", nameValue.Name );
			Assert.Equal( string.Empty, nameValue.Value );
			// Comma cookie separator, no second cookie
			test += ",";
			nameValue = SetCookieHeaderGrammar.NameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( "foo", nameValue.Name );
			Assert.Equal( string.Empty, nameValue.Value );

			// Semi-colon terminator
			test = "foo=bar;";
			nameValue = SetCookieHeaderGrammar.NameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( "foo", nameValue.Name );
			Assert.Equal( "bar", nameValue.Value );
			// Comma cookie separator, no second cookie
			test += ",";
			nameValue = SetCookieHeaderGrammar.NameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( "foo", nameValue.Name );
			Assert.Equal( "bar", nameValue.Value );

			// Uppercase
			test = "FOO=BAR";
			nameValue = SetCookieHeaderGrammar.NameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( "FOO", nameValue.Name );
			Assert.Equal( "BAR", nameValue.Value );
			// Comma cookie separator, no second cookie
			test += ",";
			nameValue = SetCookieHeaderGrammar.NameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( "FOO", nameValue.Name );
			Assert.Equal( "BAR", nameValue.Value );

			// No value
			test = "FOO=;";
			nameValue = SetCookieHeaderGrammar.NameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( "FOO", nameValue.Name );
			Assert.Equal( string.Empty, nameValue.Value );
			// Comma cookie separator, no second cookie
			test += ",";
			nameValue = SetCookieHeaderGrammar.NameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( "FOO", nameValue.Name );
			Assert.Equal( string.Empty, nameValue.Value );
		}

		[Fact]
		public void NameOnlyTests() {
			// Nothing to parse
			Assert.Throws<ParseException>( () => SetCookieHeaderGrammar.NameOnly.Parse( string.Empty ) );

			// Basic
			var test = "foo";
			var nameValue = SetCookieHeaderGrammar.NameOnly.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( "foo", nameValue.Name );
			Assert.Equal( string.Empty, nameValue.Value );
			// Comma cookie separator, no second cookie
			test += ",";
			nameValue = SetCookieHeaderGrammar.NameOnly.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( "foo", nameValue.Name );
			Assert.Equal( string.Empty, nameValue.Value );

			// No value
			test = "foo=;";
			nameValue = SetCookieHeaderGrammar.NameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( "foo", nameValue.Name );
			Assert.Equal( string.Empty, nameValue.Value );
			// Comma cookie separator, no second cookie
			test += ",";
			nameValue = SetCookieHeaderGrammar.NameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( "foo", nameValue.Name );
			Assert.Equal( string.Empty, nameValue.Value );

			// Semi-colon terminator
			test = "foo;";
			nameValue = SetCookieHeaderGrammar.NameOnly.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( "foo", nameValue.Name );
			Assert.Equal( string.Empty, nameValue.Value );
			// Comma cookie separator, no second cookie
			test += ",";
			nameValue = SetCookieHeaderGrammar.NameOnly.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( "foo", nameValue.Name );
			Assert.Equal( string.Empty, nameValue.Value );

			// Uppercase
			test = "FOO";
			nameValue = SetCookieHeaderGrammar.NameOnly.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( "FOO", nameValue.Name );
			Assert.Equal( string.Empty, nameValue.Value );
			// Comma cookie separator, no second cookie
			test += ",";
			nameValue = SetCookieHeaderGrammar.NameOnly.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( "FOO", nameValue.Name );
			Assert.Equal( string.Empty, nameValue.Value );

			// No value
			test = "FOO=;";
			nameValue = SetCookieHeaderGrammar.NameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( "FOO", nameValue.Name );
			Assert.Equal( string.Empty, nameValue.Value );
			// Comma cookie separator, no second cookie
			test += ",";
			nameValue = SetCookieHeaderGrammar.NameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( "FOO", nameValue.Name );
			Assert.Equal( string.Empty, nameValue.Value );
		}

		[Fact]
		public void ExpiresNameValueTests() {
			// Nothing to parse
			Assert.Throws<ParseException>( () => SetCookieHeaderGrammar.ExpiresNameValue.Parse( string.Empty ) );

			// Basic preferred
			var test = $"{ExpiresName}={PreferredFormatDateValue}";
			var nameValue = SetCookieHeaderGrammar.ExpiresNameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( ExpiresName, nameValue.Name );
			Assert.Equal( PreferredFormatDateValue, nameValue.Value );
			// Semi-colon terminator
			test += ";";
			nameValue = SetCookieHeaderGrammar.ExpiresNameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( ExpiresName, nameValue.Name );
			Assert.Equal( PreferredFormatDateValue, nameValue.Value );
			// Comma cookie separator, no second cookie
			test = test.Replace( ";", "," );
			nameValue = SetCookieHeaderGrammar.ExpiresNameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( ExpiresName, nameValue.Name );
			Assert.Equal( PreferredFormatDateValue, nameValue.Value );
			// Uppercase
			test = $"{ExpiresNameUpperCase}={PreferredFormatDateValue}";
			nameValue = SetCookieHeaderGrammar.ExpiresNameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( ExpiresNameUpperCase, nameValue.Name );
			Assert.Equal( PreferredFormatDateValue, nameValue.Value );
			// Mixed case
			test = $"{ExpiresNameMixedCase}={PreferredFormatDateValue}";
			nameValue = SetCookieHeaderGrammar.ExpiresNameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( ExpiresNameMixedCase, nameValue.Name );
			Assert.Equal( PreferredFormatDateValue, nameValue.Value );

			// Basic obsolete RFC 850
			test = $"{ExpiresName}={Rfc850FormatDateValue}";
			nameValue = SetCookieHeaderGrammar.ExpiresNameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( ExpiresName, nameValue.Name );
			Assert.Equal( Rfc850FormatDateValue, nameValue.Value );
			// Semi-colon terminator
			test += ";";
			nameValue = SetCookieHeaderGrammar.ExpiresNameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( ExpiresName, nameValue.Name );
			Assert.Equal( Rfc850FormatDateValue, nameValue.Value );
			// Comma cookie separator, no second cookie
			test = test.Replace( ";", "," );
			nameValue = SetCookieHeaderGrammar.ExpiresNameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( ExpiresName, nameValue.Name );
			Assert.Equal( Rfc850FormatDateValue, nameValue.Value );
			// Uppercase
			test = $"{ExpiresNameUpperCase}={Rfc850FormatDateValue}";
			nameValue = SetCookieHeaderGrammar.ExpiresNameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( ExpiresNameUpperCase, nameValue.Name );
			Assert.Equal( Rfc850FormatDateValue, nameValue.Value );
			// Mixed case
			test = $"{ExpiresNameMixedCase}={Rfc850FormatDateValue}";
			nameValue = SetCookieHeaderGrammar.ExpiresNameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( ExpiresNameMixedCase, nameValue.Name );
			Assert.Equal( Rfc850FormatDateValue, nameValue.Value );

			// Basic obsolete ANSI C asctime()
			test = $"{ExpiresName}={AnsiCasctimeFormatDateValue}";
			nameValue = SetCookieHeaderGrammar.ExpiresNameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( ExpiresName, nameValue.Name );
			Assert.Equal( AnsiCasctimeFormatDateValue, nameValue.Value );
			// Semi-colon terminator
			test += ";";
			nameValue = SetCookieHeaderGrammar.ExpiresNameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( ExpiresName, nameValue.Name );
			Assert.Equal( AnsiCasctimeFormatDateValue, nameValue.Value );
			// Comma cookie separator, no second cookie
			test = test.Replace( ";", "," );
			nameValue = SetCookieHeaderGrammar.ExpiresNameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( ExpiresName, nameValue.Name );
			Assert.Equal( AnsiCasctimeFormatDateValue, nameValue.Value );
			// Uppercase
			test = $"{ExpiresNameUpperCase}={AnsiCasctimeFormatDateValue}";
			nameValue = SetCookieHeaderGrammar.ExpiresNameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( ExpiresNameUpperCase, nameValue.Name );
			Assert.Equal( AnsiCasctimeFormatDateValue, nameValue.Value );
			// Mixed case
			test = $"{ExpiresNameMixedCase}={AnsiCasctimeFormatDateValue}";
			nameValue = SetCookieHeaderGrammar.ExpiresNameValue.Parse( test );
			Assert.NotNull( nameValue );
			Assert.Equal( ExpiresNameMixedCase, nameValue.Name );
			Assert.Equal( AnsiCasctimeFormatDateValue, nameValue.Value );
		}

		[Fact]
		public void CookieTests() {
			// Nothing to parse
			var test = string.Empty;
			Assert.Throws<ParseException>( () => SetCookieHeaderGrammar.Cookie.Parse( test ) );

			// Basic
			test = "aaa=bbb";
			var parsedCookie = SetCookieHeaderGrammar.Cookie.Parse( test );
			Assert.NotNull( parsedCookie );
			Assert.Equal( 0, parsedCookie.AttributePairs.Count() );
			Assert.Equal( "aaa", parsedCookie.CookiePair.Name );
			Assert.Equal( "bbb", parsedCookie.CookiePair.Value );
			// Semi-colon terminator with space
			test += "; ";
			parsedCookie = SetCookieHeaderGrammar.Cookie.Parse( test );
			Assert.NotNull( parsedCookie );
			Assert.Equal( 0, parsedCookie.AttributePairs.Count() );
			Assert.Equal( "aaa", parsedCookie.CookiePair.Name );
			Assert.Equal( "bbb", parsedCookie.CookiePair.Value );

			// No value
			test += "foo=;";
			parsedCookie = SetCookieHeaderGrammar.Cookie.Parse( test );
			Assert.NotNull( parsedCookie );
			Assert.Equal( 1, parsedCookie.AttributePairs.Count() );
			Assert.Equal( "aaa", parsedCookie.CookiePair.Name );
			Assert.Equal( "bbb", parsedCookie.CookiePair.Value );
			Assert.Equal( "foo", parsedCookie.AttributePairs.ElementAt( 0 ).Name );
			Assert.Equal( string.Empty, parsedCookie.AttributePairs.ElementAt( 0 ).Value );

			// Uppercase
			test += "FOO=BAR";
			parsedCookie = SetCookieHeaderGrammar.Cookie.Parse( test );
			Assert.NotNull( parsedCookie );
			Assert.Equal( 2, parsedCookie.AttributePairs.Count() );
			Assert.Equal( "aaa", parsedCookie.CookiePair.Name );
			Assert.Equal( "bbb", parsedCookie.CookiePair.Value );
			Assert.Equal( "foo", parsedCookie.AttributePairs.ElementAt( 0 ).Name );
			Assert.Equal( string.Empty, parsedCookie.AttributePairs.ElementAt( 0 ).Value );
			Assert.Equal( "FOO", parsedCookie.AttributePairs.ElementAt( 1 ).Name );
			Assert.Equal( "BAR", parsedCookie.AttributePairs.ElementAt( 1 ).Value );
			// Semi-colon terminator with space
			test += "; ";
			parsedCookie = SetCookieHeaderGrammar.Cookie.Parse( test );
			Assert.NotNull( parsedCookie );
			Assert.Equal( 2, parsedCookie.AttributePairs.Count() );
			Assert.Equal( "aaa", parsedCookie.CookiePair.Name );
			Assert.Equal( "bbb", parsedCookie.CookiePair.Value );
			Assert.Equal( "foo", parsedCookie.AttributePairs.ElementAt( 0 ).Name );
			Assert.Equal( string.Empty, parsedCookie.AttributePairs.ElementAt( 0 ).Value );
			Assert.Equal( "FOO", parsedCookie.AttributePairs.ElementAt( 1 ).Name );
			Assert.Equal( "BAR", parsedCookie.AttributePairs.ElementAt( 1 ).Value );

			// No value
			test += "FOO";
			parsedCookie = SetCookieHeaderGrammar.Cookie.Parse( test );
			Assert.NotNull( parsedCookie );
			Assert.Equal( 3, parsedCookie.AttributePairs.Count() );
			Assert.Equal( "aaa", parsedCookie.CookiePair.Name );
			Assert.Equal( "bbb", parsedCookie.CookiePair.Value );
			Assert.Equal( "foo", parsedCookie.AttributePairs.ElementAt( 0 ).Name );
			Assert.Equal( string.Empty, parsedCookie.AttributePairs.ElementAt( 0 ).Value );
			Assert.Equal( "FOO", parsedCookie.AttributePairs.ElementAt( 1 ).Name );
			Assert.Equal( "BAR", parsedCookie.AttributePairs.ElementAt( 1 ).Value );
			Assert.Equal( "FOO", parsedCookie.AttributePairs.ElementAt( 2 ).Name );
			Assert.Equal( string.Empty, parsedCookie.AttributePairs.ElementAt( 2 ).Value );
			// Semi-colon terminator with space
			test += ";";
			parsedCookie = SetCookieHeaderGrammar.Cookie.Parse( test );
			Assert.NotNull( parsedCookie );
			Assert.Equal( 3, parsedCookie.AttributePairs.Count() );
			Assert.Equal( "aaa", parsedCookie.CookiePair.Name );
			Assert.Equal( "bbb", parsedCookie.CookiePair.Value );
			Assert.Equal( "foo", parsedCookie.AttributePairs.ElementAt( 0 ).Name );
			Assert.Equal( string.Empty, parsedCookie.AttributePairs.ElementAt( 0 ).Value );
			Assert.Equal( "FOO", parsedCookie.AttributePairs.ElementAt( 1 ).Name );
			Assert.Equal( "BAR", parsedCookie.AttributePairs.ElementAt( 1 ).Value );
			Assert.Equal( "FOO", parsedCookie.AttributePairs.ElementAt( 2 ).Name );
			Assert.Equal( string.Empty, parsedCookie.AttributePairs.ElementAt( 2 ).Value );
		}

		[Fact]
		public void SetCookieHeaderTests() {
			// Nothing to parse
			var test = string.Empty;
			var setCookieHeader = SetCookieHeaderGrammar.SetCookieHeader.Parse( test );
			Assert.NotNull( setCookieHeader );
			Assert.Equal( 0, setCookieHeader.ParsedCookies.Count() );

			// One cookie, one pair
			test = "aaa=bbb";
			setCookieHeader = SetCookieHeaderGrammar.SetCookieHeader.Parse( test );
			Assert.NotNull( setCookieHeader );
			Assert.Equal( 1, setCookieHeader.ParsedCookies.Count() );
			Assert.Equal( 0, setCookieHeader.ParsedCookies.ElementAt(0).AttributePairs.Count() );
			Assert.Equal( "aaa", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Name );
			Assert.Equal( "bbb", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Value );
			// Comma cookie separator, no second cookie
			test += ",";
			setCookieHeader = SetCookieHeaderGrammar.SetCookieHeader.Parse( test );
			Assert.NotNull( setCookieHeader );
			Assert.Equal( 1, setCookieHeader.ParsedCookies.Count() );
			Assert.Equal( 0, setCookieHeader.ParsedCookies.ElementAt( 0 ).AttributePairs.Count() );
			Assert.Equal( "aaa", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Name );
			Assert.Equal( "bbb", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Value );

			// One cookie, two pairs
			test = "aaa=bbb;ccc=ddd";
			setCookieHeader = SetCookieHeaderGrammar.SetCookieHeader.Parse( test );
			Assert.NotNull( setCookieHeader );
			Assert.Equal( 1, setCookieHeader.ParsedCookies.Count() );
			Assert.Equal( 1, setCookieHeader.ParsedCookies.ElementAt( 0 ).AttributePairs.Count() );
			Assert.Equal( "aaa", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Name );
			Assert.Equal( "bbb", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Value );
			Assert.Equal( "ccc", setCookieHeader.ParsedCookies.ElementAt( 0 ).AttributePairs.ElementAt( 0 ).Name );
			Assert.Equal( "ddd", setCookieHeader.ParsedCookies.ElementAt( 0 ).AttributePairs.ElementAt( 0 ).Value );
			// Comma cookie separator, no second cookie
			test += ",";
			setCookieHeader = SetCookieHeaderGrammar.SetCookieHeader.Parse( test );
			Assert.NotNull( setCookieHeader );
			Assert.Equal( 1, setCookieHeader.ParsedCookies.Count() );
			Assert.Equal( 1, setCookieHeader.ParsedCookies.ElementAt( 0 ).AttributePairs.Count() );
			Assert.Equal( "aaa", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Name );
			Assert.Equal( "bbb", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Value );
			Assert.Equal( "ccc", setCookieHeader.ParsedCookies.ElementAt( 0 ).AttributePairs.ElementAt( 0 ).Name );
			Assert.Equal( "ddd", setCookieHeader.ParsedCookies.ElementAt( 0 ).AttributePairs.ElementAt( 0 ).Value );

			// Two cookies: one pair each
			test = "aaa=bbb,ccc=ddd";
			setCookieHeader = SetCookieHeaderGrammar.SetCookieHeader.Parse( test );
			Assert.NotNull( setCookieHeader );
			Assert.Equal( 2, setCookieHeader.ParsedCookies.Count() );
			Assert.Equal( 0, setCookieHeader.ParsedCookies.ElementAt( 0 ).AttributePairs.Count() );
			Assert.Equal( "aaa", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Name );
			Assert.Equal( "bbb", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Value );
			Assert.Equal( 0, setCookieHeader.ParsedCookies.ElementAt( 1 ).AttributePairs.Count() );
			Assert.Equal( "ccc", setCookieHeader.ParsedCookies.ElementAt( 1 ).CookiePair.Name );
			Assert.Equal( "ddd", setCookieHeader.ParsedCookies.ElementAt( 1 ).CookiePair.Value );
			// Comma cookie separator, no third cookie
			test += ",";
			setCookieHeader = SetCookieHeaderGrammar.SetCookieHeader.Parse( test );
			Assert.NotNull( setCookieHeader );
			Assert.Equal( 2, setCookieHeader.ParsedCookies.Count() );
			Assert.Equal( 0, setCookieHeader.ParsedCookies.ElementAt( 0 ).AttributePairs.Count() );
			Assert.Equal( "aaa", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Name );
			Assert.Equal( "bbb", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Value );
			Assert.Equal( 0, setCookieHeader.ParsedCookies.ElementAt( 1 ).AttributePairs.Count() );
			Assert.Equal( "ccc", setCookieHeader.ParsedCookies.ElementAt( 1 ).CookiePair.Name );
			Assert.Equal( "ddd", setCookieHeader.ParsedCookies.ElementAt( 1 ).CookiePair.Value );

			// Two cookies: one pair, two pair
			test = "aaa=bbb;xxx=yyy,ccc=ddd";
			setCookieHeader = SetCookieHeaderGrammar.SetCookieHeader.Parse( test );
			Assert.NotNull( setCookieHeader );
			Assert.Equal( 2, setCookieHeader.ParsedCookies.Count() );
			Assert.Equal( 1, setCookieHeader.ParsedCookies.ElementAt( 0 ).AttributePairs.Count() );
			Assert.Equal( "aaa", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Name );
			Assert.Equal( "bbb", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Value );
			Assert.Equal( "xxx", setCookieHeader.ParsedCookies.ElementAt( 0 ).AttributePairs.ElementAt( 0 ).Name );
			Assert.Equal( "yyy", setCookieHeader.ParsedCookies.ElementAt( 0 ).AttributePairs.ElementAt( 0 ).Value );
			Assert.Equal( 0, setCookieHeader.ParsedCookies.ElementAt( 1 ).AttributePairs.Count() );
			Assert.Equal( "ccc", setCookieHeader.ParsedCookies.ElementAt( 1 ).CookiePair.Name );
			Assert.Equal( "ddd", setCookieHeader.ParsedCookies.ElementAt( 1 ).CookiePair.Value );
			// Comma cookie separator, no third cookie
			test += ",";
			setCookieHeader = SetCookieHeaderGrammar.SetCookieHeader.Parse( test );
			Assert.NotNull( setCookieHeader );
			Assert.Equal( 2, setCookieHeader.ParsedCookies.Count() );
			Assert.Equal( 1, setCookieHeader.ParsedCookies.ElementAt( 0 ).AttributePairs.Count() );
			Assert.Equal( "aaa", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Name );
			Assert.Equal( "bbb", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Value );
			Assert.Equal( "xxx", setCookieHeader.ParsedCookies.ElementAt( 0 ).AttributePairs.ElementAt( 0 ).Name );
			Assert.Equal( "yyy", setCookieHeader.ParsedCookies.ElementAt( 0 ).AttributePairs.ElementAt( 0 ).Value );
			Assert.Equal( 0, setCookieHeader.ParsedCookies.ElementAt( 1 ).AttributePairs.Count() );
			Assert.Equal( "ccc", setCookieHeader.ParsedCookies.ElementAt( 1 ).CookiePair.Name );
			Assert.Equal( "ddd", setCookieHeader.ParsedCookies.ElementAt( 1 ).CookiePair.Value );

			// Two cookies: two pair, one pair
			test = "aaa=bbb,ccc=ddd;xxx=yyy";
			setCookieHeader = SetCookieHeaderGrammar.SetCookieHeader.Parse( test );
			Assert.NotNull( setCookieHeader );
			Assert.Equal( 2, setCookieHeader.ParsedCookies.Count() );
			Assert.Equal( 0, setCookieHeader.ParsedCookies.ElementAt( 0 ).AttributePairs.Count() );
			Assert.Equal( "aaa", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Name );
			Assert.Equal( "bbb", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Value );
			Assert.Equal( 1, setCookieHeader.ParsedCookies.ElementAt( 1 ).AttributePairs.Count() );
			Assert.Equal( "ccc", setCookieHeader.ParsedCookies.ElementAt( 1 ).CookiePair.Name );
			Assert.Equal( "ddd", setCookieHeader.ParsedCookies.ElementAt( 1 ).CookiePair.Value );
			Assert.Equal( "xxx", setCookieHeader.ParsedCookies.ElementAt( 1 ).AttributePairs.ElementAt( 0 ).Name );
			Assert.Equal( "yyy", setCookieHeader.ParsedCookies.ElementAt( 1 ).AttributePairs.ElementAt( 0 ).Value );
			// Comma cookie separator, no third cookie
			test += ",";
			setCookieHeader = SetCookieHeaderGrammar.SetCookieHeader.Parse( test );
			Assert.NotNull( setCookieHeader );
			Assert.Equal( 2, setCookieHeader.ParsedCookies.Count() );
			Assert.Equal( 0, setCookieHeader.ParsedCookies.ElementAt( 0 ).AttributePairs.Count() );
			Assert.Equal( "aaa", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Name );
			Assert.Equal( "bbb", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Value );
			Assert.Equal( 1, setCookieHeader.ParsedCookies.ElementAt( 1 ).AttributePairs.Count() );
			Assert.Equal( "ccc", setCookieHeader.ParsedCookies.ElementAt( 1 ).CookiePair.Name );
			Assert.Equal( "ddd", setCookieHeader.ParsedCookies.ElementAt( 1 ).CookiePair.Value );
			Assert.Equal( "xxx", setCookieHeader.ParsedCookies.ElementAt( 1 ).AttributePairs.ElementAt( 0 ).Name );
			Assert.Equal( "yyy", setCookieHeader.ParsedCookies.ElementAt( 1 ).AttributePairs.ElementAt( 0 ).Value );

			// Assorted cookies
			test = "aaa=bbb;,ccc=ddd;xxx;yyy=,zzz=000;" + $"{ExpiresName}={PreferredFormatDateValue}" + ",abc=def;" + $"{ExpiresName}={Rfc850FormatDateValue}" + ",ghi=jkl;" + $"{ExpiresName}={AnsiCasctimeFormatDateValue}" + ",mno=pqr;";
			setCookieHeader = SetCookieHeaderGrammar.SetCookieHeader.Parse( test );
			Assert.NotNull( setCookieHeader );
			Assert.Equal( 6, setCookieHeader.ParsedCookies.Count() );
			Assert.Equal( 0, setCookieHeader.ParsedCookies.ElementAt( 0 ).AttributePairs.Count() );
			Assert.Equal( "aaa", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Name );
			Assert.Equal( "bbb", setCookieHeader.ParsedCookies.ElementAt( 0 ).CookiePair.Value );
			Assert.Equal( 2, setCookieHeader.ParsedCookies.ElementAt( 1 ).AttributePairs.Count() );
			Assert.Equal( "ccc", setCookieHeader.ParsedCookies.ElementAt( 1 ).CookiePair.Name );
			Assert.Equal( "ddd", setCookieHeader.ParsedCookies.ElementAt( 1 ).CookiePair.Value );
			Assert.Equal( "xxx", setCookieHeader.ParsedCookies.ElementAt( 1 ).AttributePairs.ElementAt( 0 ).Name );
			Assert.Equal( string.Empty, setCookieHeader.ParsedCookies.ElementAt( 1 ).AttributePairs.ElementAt( 0 ).Value );
			Assert.Equal( "yyy", setCookieHeader.ParsedCookies.ElementAt( 1 ).AttributePairs.ElementAt( 1 ).Name );
			Assert.Equal( string.Empty, setCookieHeader.ParsedCookies.ElementAt( 1 ).AttributePairs.ElementAt( 1 ).Value );
			Assert.Equal( 1, setCookieHeader.ParsedCookies.ElementAt( 2 ).AttributePairs.Count() );
			Assert.Equal( "zzz", setCookieHeader.ParsedCookies.ElementAt( 2 ).CookiePair.Name );
			Assert.Equal( "000", setCookieHeader.ParsedCookies.ElementAt( 2 ).CookiePair.Value );
			Assert.Equal( ExpiresName, setCookieHeader.ParsedCookies.ElementAt( 2 ).AttributePairs.ElementAt( 0 ).Name );
			Assert.Equal( PreferredFormatDateValue, setCookieHeader.ParsedCookies.ElementAt( 2 ).AttributePairs.ElementAt( 0 ).Value );
			Assert.Equal( 1, setCookieHeader.ParsedCookies.ElementAt( 3 ).AttributePairs.Count() );
			Assert.Equal( "abc", setCookieHeader.ParsedCookies.ElementAt( 3 ).CookiePair.Name );
			Assert.Equal( "def", setCookieHeader.ParsedCookies.ElementAt( 3 ).CookiePair.Value );
			Assert.Equal( ExpiresName, setCookieHeader.ParsedCookies.ElementAt( 3 ).AttributePairs.ElementAt( 0 ).Name );
			Assert.Equal( Rfc850FormatDateValue, setCookieHeader.ParsedCookies.ElementAt( 3 ).AttributePairs.ElementAt( 0 ).Value );
			Assert.Equal( 1, setCookieHeader.ParsedCookies.ElementAt( 4 ).AttributePairs.Count() );
			Assert.Equal( "ghi", setCookieHeader.ParsedCookies.ElementAt( 4 ).CookiePair.Name );
			Assert.Equal( "jkl", setCookieHeader.ParsedCookies.ElementAt( 4 ).CookiePair.Value );
			Assert.Equal( ExpiresName, setCookieHeader.ParsedCookies.ElementAt( 4 ).AttributePairs.ElementAt( 0 ).Name );
			Assert.Equal( AnsiCasctimeFormatDateValue, setCookieHeader.ParsedCookies.ElementAt( 4 ).AttributePairs.ElementAt( 0 ).Value );
			Assert.Equal( 0, setCookieHeader.ParsedCookies.ElementAt( 5 ).AttributePairs.Count() );
			Assert.Equal( "mno", setCookieHeader.ParsedCookies.ElementAt( 5 ).CookiePair.Name );
			Assert.Equal( "pqr", setCookieHeader.ParsedCookies.ElementAt( 5 ).CookiePair.Value );
		}
	}
}
