using System;
using Xunit;

namespace NScrape.Test {
	public class NScrapeUtilityTryParseHttpDateTests {
		[Fact]
		// http://tools.ietf.org/html/rfc7231#section-7.1.1.1
		public void TestPreferredAndObsoleteHttpDateFormats() {
			const string preferredFormatHttpDate = "Sun, 06 Nov 1994 08:49:37 GMT";

			DateTime parsedDate;

		    // Preferred format
			var httpDate = preferredFormatHttpDate;
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

		    string httpDate = null;
			// ReSharper disable once ExpressionIsAlwaysNull
			Assert.False( NScrapeUtility.TryParseHttpDate( httpDate, out parsedDate ) );
			Assert.Equal( new DateTime(), parsedDate );

			httpDate = string.Empty;
			Assert.False( NScrapeUtility.TryParseHttpDate( httpDate, out parsedDate ) );
			Assert.Equal( new DateTime(), parsedDate );

			httpDate = "Xxx, 00-Xxx-00 00:00:00 GMT";
			Assert.False( NScrapeUtility.TryParseHttpDate( httpDate, out parsedDate ) );
			Assert.Equal( new DateTime(), parsedDate );

			httpDate = "ks alkdjf alkd falk flka flakf";
			Assert.False( NScrapeUtility.TryParseHttpDate( httpDate, out parsedDate ) );
			Assert.Equal( new DateTime(), parsedDate );
		}

		[Fact]
		public void TestInvalidButParseableHttpDateFormats() {
			const string preferredFormatHttpDate = "Sun, 02 Jul 2017 22:07:33 GMT";

			DateTime parsedDate;

			var httpDate = "Sun, 02-Jul-17 22:07:33 GMT";
			Assert.True( NScrapeUtility.TryParseHttpDate( httpDate, out parsedDate ) );
			Assert.Equal( DateTimeKind.Utc, parsedDate.Kind );
			Assert.Equal( preferredFormatHttpDate, parsedDate.ToString( "r" ) );

			httpDate = "Sunday, July 2, 2017 10:07:33pm";
			Assert.True( NScrapeUtility.TryParseHttpDate( httpDate, out parsedDate ) );
			Assert.Equal( DateTimeKind.Utc, parsedDate.Kind );
			Assert.Equal( preferredFormatHttpDate, parsedDate.ToString( "r" ) );

			httpDate = "Sun, 02 Jul 2017 22:07:33 -0000";
			Assert.True( NScrapeUtility.TryParseHttpDate( httpDate, out parsedDate ) );
			Assert.Equal( DateTimeKind.Utc, parsedDate.Kind );
			Assert.Equal( preferredFormatHttpDate, parsedDate.ToString( "r" ) );

			httpDate = "Sun, 02 Jul 2017 22:07:33 +0000";
			Assert.True( NScrapeUtility.TryParseHttpDate( httpDate, out parsedDate ) );
			Assert.Equal( DateTimeKind.Utc, parsedDate.Kind );
			Assert.Equal( preferredFormatHttpDate, parsedDate.ToString( "r" ) );
		}

		[Fact]
		public void TestValidHttpDateFormats() {
			DateTime parsedDate;

		    var httpDate = "Fri, 23 Jan 2015 23:55:58 GMT";
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
