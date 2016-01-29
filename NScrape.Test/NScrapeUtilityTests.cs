using System;
using System.Net;
using System.Text;
using Xunit;

namespace NScrape.Test {
	public class NScrapeUtilityTests {
		[Fact]
		// http://tools.ietf.org/html/rfc7231#section-7.1.1.1
		public void TestPreferredAndObsoleteHttpDateFormats() {
			const string preferredFormatHttpDate = "Sun, 06 Nov 1994 08:49:37 GMT";

			DateTime parsedDate;
			string httpDate;

			// Preferred format
			httpDate = preferredFormatHttpDate;
			Assert.True( NScrapeUtility.TryParseHttpDate( httpDate, out parsedDate ) );
			Assert.Equal( DateTimeKind.Utc, parsedDate.Kind );
			Assert.Equal( preferredFormatHttpDate, parsedDate.ToString( "r" ) );

			// Obsolete RFC 850 format
			httpDate = "Sunday, 06-Nov-94 08:49:37 GMT";
			Assert.True( NScrapeUtility.TryParseHttpDate( httpDate, out parsedDate ) );
			Assert.Equal( DateTimeKind.Utc, parsedDate.Kind );
			Assert.Equal( preferredFormatHttpDate, parsedDate.ToString( "r" ) );

			// Obsolete ANSI C's asctime() format
			httpDate = "Sun Nov  6 08:49:37 1994";
			Assert.True( NScrapeUtility.TryParseHttpDate( httpDate, out parsedDate ) );
			Assert.Equal( DateTimeKind.Utc, parsedDate.Kind );
			Assert.Equal( preferredFormatHttpDate, parsedDate.ToString( "r" ) );
		}

		[Fact]
		public void TestInvalidHttpDateFormats() {
			DateTime parsedDate;
			string httpDate;

			httpDate = null;
			Assert.False( NScrapeUtility.TryParseHttpDate( httpDate, out parsedDate ) );

			httpDate = string.Empty;
			Assert.False( NScrapeUtility.TryParseHttpDate( httpDate, out parsedDate ) );

			httpDate = "ks alkdjf alkd falk flka flakf";
			Assert.False( NScrapeUtility.TryParseHttpDate( httpDate, out parsedDate ) );

			httpDate = "Sunday, November 6, 1994 8:49:37am";
			Assert.False( NScrapeUtility.TryParseHttpDate( httpDate, out parsedDate ) );
		}

		[Fact]
		public void TestValidHttpDateFormats() {
			DateTime parsedDate;
			string httpDate;

			httpDate = "Fri, 23 Jan 2015 23:55:58 GMT";
			Assert.True( NScrapeUtility.TryParseHttpDate( httpDate, out parsedDate ) );
			Assert.Equal( DateTimeKind.Utc, parsedDate.Kind );
			Assert.Equal( httpDate, parsedDate.ToString( "r" ) );

			httpDate = "Tue, 15 Nov 1994 08:12:31 GMT";
			Assert.True( NScrapeUtility.TryParseHttpDate( httpDate, out parsedDate ) );
			Assert.Equal( DateTimeKind.Utc, parsedDate.Kind );
			Assert.Equal( httpDate, parsedDate.ToString( "r" ) );

			httpDate = "Thu, 01 Jan 1970 00:00:00 GMT";
			Assert.True( NScrapeUtility.TryParseHttpDate( httpDate, out parsedDate ) );
			Assert.Equal( DateTimeKind.Utc, parsedDate.Kind );
			Assert.Equal( httpDate, parsedDate.ToString( "r" ) );

			httpDate = "Mon, 15 Jun 2009 20:45:30 GMT";
			Assert.True( NScrapeUtility.TryParseHttpDate( httpDate, out parsedDate ) );
			Assert.Equal( DateTimeKind.Utc, parsedDate.Kind );
			Assert.Equal( httpDate, parsedDate.ToString( "r" ) );
		}
	}
}
