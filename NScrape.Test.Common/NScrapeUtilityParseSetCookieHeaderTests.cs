using System;
using System.Linq;
using System.Net;
using Xunit;
// ReSharper disable UnusedParameter.Local

namespace NScrape.Test {
	public class NScrapeUtilityParseSetCookieHeaderTests {
		private const string DefaultDomain = "DefaultDomainWhenNotSuppliedInCookie.com";
		private const string CookieGithub1 = "_octo=GH1.1.1471152791.1465438116; domain=.github.com; path=/; expires=Sat, 09 Jun 2018 02:08:36 -0000";
		private const string CookieGithub2 = "logged_in=no; domain=.github.com; path=/; expires=Mon, 09 Jun 2036 02:08:36 -0000; secure; HttpOnly";
		private const string CookieGithub3 = "_gh_sess=eyJzZXNzaW9uX2lkIjoiMDRjMTdhZmMyNDliODRlMWVmNmIwNzQ2Mzg3MmY4OTUiLCJzcHlfcmVwbyI6ImRhcnJ5bHdoaXRtb3JlL05TY3JhcGUiLCJzcHlfcmVwb19hdCI6MTQ2NTQzODExNiwiX2NzcmZfdG9rZW4iOiJVSnJXRlJ5M2VmRHdXMlNLVHdrOXBTdnlXSWYxOG5UUDFSWUNUQ1hjSFQ4PSIsImZsYXNoIjp7ImRpc2NhcmQiOlsiYW5hbHl0aWNzX2xvY2F0aW9uIl0sImZsYXNoZXMiOnsiYW5hbHl0aWNzX2xvY2F0aW9uIjoiLzx1c2VyLW5hbWU%2BLzxyZXBvLW5hbWU%2BIn19fQ%3D%3D--f9dbdf5ad33c4585c4856015ae97fc680c087a24; path=/; secure; HttpOnly";
		private const string CookieStackOverflow = "s=541E2101-B768-45C8-B814-34A00525E50F; Domain=example.com; Path=/; Version=1";
		private const string CookieFrederik = "ADCDownloadAuth=[long token];Version=1;Comment=;Domain=apple.com;Path=/;Max-Age=108000;HttpOnly;Expires=Tue, 03 May 2016 13:30:57 GMT";
		private const string CookieOddExpiresDate1 = "__cfduid=d9de9c2173f6f46cc6a9457aada153ace1467497253; expires=Sun 02-Jul-17 22:07:33 GMT; path=/; domain=.typicode.com; HttpOnly";
		private const string CookieOddExpiresDate2 = "__cfduid=d9de9c2173f6f46cc6a9457aada153ace1467497253; expires=Sun, 02-Jul-17 22:07:33 GMT; path=/; domain=.typicode.com; HttpOnly";
		private const string CookieOddExpiresDateAnsiCasctime = "__cfduid=d9de9c2173f6f46cc6a9457aada153ace1467497253; expires=Sun Jul  2 22:07:33 2017; path=/; domain=.typicode.com; HttpOnly";
		private const string CookieOddExpiresDateRfc850 = "__cfduid=d9de9c2173f6f46cc6a9457aada153ace1467497253; expires=Sunday, 02-Jul-17 22:07:33 GMT; path=/; domain=.typicode.com; HttpOnly";


		[Fact]
		public void NullAndEmptyArgumentsCornerCaseTests() {
			// Null arguments
			var cookies = NScrapeUtility.ParseSetCookieHeader( null, DefaultDomain );
			Assert.Equal( 0, cookies.Count() );

			var ex = Assert.Throws<ArgumentException>( () => NScrapeUtility.ParseSetCookieHeader( null, null ) );
			Assert.Equal( ex.ParamName, "hostName" );

			// Empty arguments
			cookies = NScrapeUtility.ParseSetCookieHeader( string.Empty, DefaultDomain );
			Assert.Equal( 0, cookies.Count() );

			ex = Assert.Throws<ArgumentException>( () => NScrapeUtility.ParseSetCookieHeader( string.Empty, string.Empty ) );
			Assert.Equal( ex.ParamName, "hostName" );
		}

		[Fact]
		public void BadSetCookieHeaderTests() {
			// Bad header
			var cookies = NScrapeUtility.ParseSetCookieHeader( "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx", DefaultDomain ).ToList();
			Assert.Equal( 0, cookies.Count );

			// Bad header with commas
			cookies = NScrapeUtility.ParseSetCookieHeader( "xxxxxxxxxxxxxxxxxx,xxxxxxxxxxxxxxxxxxx,xxxxxxxxxxxxxxx", DefaultDomain ).ToList();
			Assert.Equal( 0, cookies.Count );

			// Bad header with commas and semicolons
			cookies = NScrapeUtility.ParseSetCookieHeader( "xxxxxxxx; xxxxxxxxxx,xxxxxxx; xxxxxxxxxxxx,xxxxx; xxxxxxxxxx", DefaultDomain ).ToList();
			Assert.Equal( 0, cookies.Count );

			// Valid cookie but invalid attributes
			cookies = NScrapeUtility.ParseSetCookieHeader( "foo=bar; xxxxxxxxxx; xxxxxxx; xxxxx; xxxxxxxxxx", DefaultDomain ).ToList();
			Assert.Equal( 1, cookies.Count );
			var cookie = cookies[0];
			Assert.Equal( string.Empty, cookie.Comment );
			Assert.Null( cookie.CommentUri );
			Assert.False( cookie.Discard );
			Assert.Equal( DefaultDomain, cookie.Domain );
			Assert.False( cookie.Expired );
			Assert.Equal( new DateTime( 1, 1, 1, 0, 0, 0 ), cookie.Expires );
			Assert.False( cookie.HttpOnly );
			Assert.Equal( "foo", cookie.Name );
			Assert.Equal( "/", cookie.Path );
			Assert.Equal( string.Empty, cookie.Port );
			Assert.False( cookie.Secure );
			var timeStamp = DateTime.Now;
			Assert.True( timeStamp.Subtract( cookie.TimeStamp ).TotalSeconds < 1.0 ); // just a few milliseconds between parse and test
			Assert.Equal( "bar", cookie.Value );
			Assert.Equal( 0, cookie.Version );
		}

		[Fact]
		public void BadExpiresDateTest() {
			var cookies = NScrapeUtility.ParseSetCookieHeader( "foo=bar; expires=Xxx, 00-Xxx-00 00:00:00 GMT; path=/", DefaultDomain ).ToList();
			Assert.Equal( 1, cookies.Count );
			var cookie = cookies[0];
			Assert.Equal( string.Empty, cookie.Comment );
			Assert.Null( cookie.CommentUri );
			Assert.False( cookie.Discard );
			Assert.Equal( DefaultDomain, cookie.Domain );
			Assert.False( cookie.Expired );
			Assert.Equal( new DateTime( 1, 1, 1, 0, 0, 0 ), cookie.Expires );
			Assert.False( cookie.HttpOnly );
			Assert.Equal( "foo", cookie.Name );
			Assert.Equal( "/", cookie.Path );
			Assert.Equal( string.Empty, cookie.Port );
			Assert.False( cookie.Secure );
			var timeStamp = DateTime.Now;
			Assert.True( timeStamp.Subtract( cookie.TimeStamp ).TotalSeconds < 1.0 ); // just a few milliseconds between parse and test
			Assert.Equal( "bar", cookie.Value );
			Assert.Equal( 0, cookie.Version );
		}

		[Fact]
		public void SingleCookieTests() {
			var cookies = NScrapeUtility.ParseSetCookieHeader( CookieGithub1, DefaultDomain ).ToList();
			Assert.Equal( 1, cookies.Count );
			CookieGithub1Test( cookies[0] );

			cookies = NScrapeUtility.ParseSetCookieHeader( CookieGithub2, DefaultDomain ).ToList();
			Assert.Equal( 1, cookies.Count );
			CookieGithub2Test( cookies[0] );

			cookies = NScrapeUtility.ParseSetCookieHeader( CookieGithub3, DefaultDomain ).ToList();
			Assert.Equal( 1, cookies.Count );
			CookieGithub3Test( cookies[0] );

			cookies = NScrapeUtility.ParseSetCookieHeader( CookieStackOverflow, DefaultDomain ).ToList();
			Assert.Equal( 1, cookies.Count );
			CookieStackOverflowTest( cookies[0] );

			cookies = NScrapeUtility.ParseSetCookieHeader( CookieFrederik, DefaultDomain ).ToList();
			Assert.Equal( 1, cookies.Count );
			CookieFrederikTest( cookies[0] );
		}

		[Fact]
		public void OddExpiresDateTests() {
			var cookies = NScrapeUtility.ParseSetCookieHeader( CookieOddExpiresDate1, DefaultDomain ).ToList();
			Assert.Equal( 1, cookies.Count );
			CookieOddExpiresDateTest( cookies[0] );

			cookies = NScrapeUtility.ParseSetCookieHeader( CookieOddExpiresDate2, DefaultDomain ).ToList();
			Assert.Equal( 1, cookies.Count );
			CookieOddExpiresDateTest( cookies[0] );

			cookies = NScrapeUtility.ParseSetCookieHeader( CookieOddExpiresDateAnsiCasctime, DefaultDomain ).ToList();
			Assert.Equal( 1, cookies.Count );
			CookieOddExpiresDateTest( cookies[0] );

			cookies = NScrapeUtility.ParseSetCookieHeader( CookieOddExpiresDateRfc850, DefaultDomain ).ToList();
			Assert.Equal( 1, cookies.Count );
			CookieOddExpiresDateTest( cookies[0] );
		}

		[Fact]
		public void OddExpiresDateFailingTests() {
			var cookies = NScrapeUtility.ParseSetCookieHeader( string.Join( ",", CookieOddExpiresDate1, CookieOddExpiresDate2 ), DefaultDomain ).ToList();
			Assert.Equal( 2, cookies.Count );
			CookieOddExpiresDateTest( cookies[0] );
			CookieOddExpiresDateTest( cookies[1] );

			cookies = NScrapeUtility.ParseSetCookieHeader( string.Join( ",", CookieOddExpiresDate2, CookieOddExpiresDate1 ), DefaultDomain ).ToList();
			Assert.Equal( 2, cookies.Count );
			CookieOddExpiresDateTest( cookies[0] );
			CookieOddExpiresDateTest( cookies[1] );
		}

		[Fact]
		public void MultipleCookieTest() {
			var header = string.Join( ",", CookieGithub1, CookieGithub2, CookieGithub3 );

			var cookies = NScrapeUtility.ParseSetCookieHeader( header, DefaultDomain ).ToList();
			Assert.Equal( 3, cookies.Count );
			CookieGithub1Test( cookies[0] );
			CookieGithub2Test( cookies[1] );
			CookieGithub3Test( cookies[2] );
		}

		[Fact]
		public void NewlinesTest() {
			var cookies = NScrapeUtility.ParseSetCookieHeader( "foo=\rline1\nline2\rline3\r\nline4\r\rline5\n", DefaultDomain ).ToList();
			Assert.Equal( 1, cookies.Count );
			var cookie = cookies[0];
			Assert.Equal( string.Empty, cookie.Comment );
			Assert.Null( cookie.CommentUri );
			Assert.False( cookie.Discard );
			Assert.Equal( DefaultDomain, cookie.Domain );
			Assert.False( cookie.Expired );
			Assert.Equal( new DateTime( 1, 1, 1, 0, 0, 0 ), cookie.Expires );
			Assert.False( cookie.HttpOnly );
			Assert.Equal( "foo", cookie.Name );
			Assert.Equal( "/", cookie.Path );
			Assert.Equal( string.Empty, cookie.Port );
			Assert.False( cookie.Secure );
			var timeStamp = DateTime.Now;
			Assert.True( timeStamp.Subtract( cookie.TimeStamp ).TotalSeconds < 1.0 ); // just a few milliseconds between parse and test
			Assert.Equal( "line1line2line3line4line5", cookie.Value );
			Assert.Equal( 0, cookie.Version );
		}

		private static void CookieGithub1Test( Cookie cookie) {
			Assert.Equal( string.Empty, cookie.Comment );
			Assert.Null( cookie.CommentUri );
			Assert.False( cookie.Discard );
			Assert.Equal( ".github.com", cookie.Domain );
			Assert.False( cookie.Expired );
			Assert.Equal( new DateTime( 2018, 6, 8, 21, 8, 36 ), cookie.Expires );
			Assert.False( cookie.HttpOnly );
			Assert.Equal( "_octo", cookie.Name );
			Assert.Equal( "/", cookie.Path );
			Assert.Equal( string.Empty, cookie.Port );
			Assert.False( cookie.Secure );
			var timeStamp = DateTime.Now;
			Assert.True( timeStamp.Subtract( cookie.TimeStamp ).TotalSeconds < 1.0 ); // just a few milliseconds between parse and test
			Assert.Equal( "GH1.1.1471152791.1465438116", cookie.Value );
			Assert.Equal( 0, cookie.Version );
		}

		private void CookieGithub2Test( Cookie cookie ) {
			Assert.Equal( string.Empty, cookie.Comment );
			Assert.Null( cookie.CommentUri );
			Assert.False( cookie.Discard );
			Assert.Equal( ".github.com", cookie.Domain );
			Assert.False( cookie.Expired );
			Assert.Equal( new DateTime( 2036, 6, 8, 21, 8, 36 ), cookie.Expires );
			Assert.True( cookie.HttpOnly );
			Assert.Equal( "logged_in", cookie.Name );
			Assert.Equal( "/", cookie.Path );
			Assert.Equal( string.Empty, cookie.Port );
			Assert.True( cookie.Secure );
			var timeStamp = DateTime.Now;
			Assert.True( timeStamp.Subtract( cookie.TimeStamp ).TotalSeconds < 1.0 ); // just a few milliseconds between parse and test
			Assert.Equal( "no", cookie.Value );
			Assert.Equal( 0, cookie.Version );
		}

		private static void CookieGithub3Test( Cookie cookie ) {
			Assert.Equal( string.Empty, cookie.Comment );
			Assert.Null( cookie.CommentUri );
			Assert.False( cookie.Discard );
			Assert.Equal( DefaultDomain, cookie.Domain );
			Assert.False( cookie.Expired );
			Assert.Equal( new DateTime( 1, 1, 1, 0, 0, 0 ), cookie.Expires );
			Assert.True( cookie.HttpOnly );
			Assert.Equal( "_gh_sess", cookie.Name );
			Assert.Equal( "/", cookie.Path );
			Assert.Equal( string.Empty, cookie.Port );
			Assert.True( cookie.Secure );
			var timeStamp = DateTime.Now;
			Assert.True( timeStamp.Subtract( cookie.TimeStamp ).TotalSeconds < 1.0 ); // just a few milliseconds between parse and test
			Assert.Equal( "eyJzZXNzaW9uX2lkIjoiMDRjMTdhZmMyNDliODRlMWVmNmIwNzQ2Mzg3MmY4OTUiLCJzcHlfcmVwbyI6ImRhcnJ5bHdoaXRtb3JlL05TY3JhcGUiLCJzcHlfcmVwb19hdCI6MTQ2NTQzODExNiwiX2NzcmZfdG9rZW4iOiJVSnJXRlJ5M2VmRHdXMlNLVHdrOXBTdnlXSWYxOG5UUDFSWUNUQ1hjSFQ4PSIsImZsYXNoIjp7ImRpc2NhcmQiOlsiYW5hbHl0aWNzX2xvY2F0aW9uIl0sImZsYXNoZXMiOnsiYW5hbHl0aWNzX2xvY2F0aW9uIjoiLzx1c2VyLW5hbWU%2BLzxyZXBvLW5hbWU%2BIn19fQ%3D%3D--f9dbdf5ad33c4585c4856015ae97fc680c087a24", cookie.Value );
			Assert.Equal( 0, cookie.Version );
		}

		private static void CookieStackOverflowTest( Cookie cookie ) {
			Assert.Equal( string.Empty, cookie.Comment );
			Assert.Null( cookie.CommentUri );
			Assert.False( cookie.Discard );
			Assert.Equal( "example.com", cookie.Domain );
			Assert.False( cookie.Expired );
			Assert.Equal( new DateTime( 1, 1, 1, 0, 0, 0 ), cookie.Expires );
			Assert.False( cookie.HttpOnly );
			Assert.Equal( "s", cookie.Name );
			Assert.Equal( "/", cookie.Path );
			Assert.Equal( string.Empty, cookie.Port );
			Assert.False( cookie.Secure );
			var timeStamp = DateTime.Now;
			Assert.True( timeStamp.Subtract( cookie.TimeStamp ).TotalSeconds < 1.0 ); // just a few milliseconds between parse and test
			Assert.Equal( "541E2101-B768-45C8-B814-34A00525E50F", cookie.Value );
			Assert.Equal( 1, cookie.Version );
		}

		private static void CookieFrederikTest( Cookie cookie ) {
			Assert.Equal( string.Empty, cookie.Comment );
			Assert.Null( cookie.CommentUri );
			Assert.False( cookie.Discard );
			Assert.Equal( "apple.com", cookie.Domain );
			Assert.False( cookie.Expired );
			var expires = DateTime.Now.AddSeconds( 108000 );
			Assert.True( expires.Subtract( cookie.Expires ).TotalSeconds < 1.0 ); // just a few milliseconds between parse and test
			Assert.True( cookie.HttpOnly );
			Assert.Equal( "ADCDownloadAuth", cookie.Name );
			Assert.Equal( "/", cookie.Path );
			Assert.Equal( string.Empty, cookie.Port );
			Assert.False( cookie.Secure );
			var timeStamp = DateTime.Now;
			Assert.True( timeStamp.Subtract( cookie.TimeStamp ).TotalSeconds < 1.0 ); // just a few milliseconds between parse and test
			Assert.Equal( "[long token]", cookie.Value );
			Assert.Equal( 1, cookie.Version );
		}

		private static void CookieOddExpiresDateTest( Cookie cookie ) {
			Assert.Equal( string.Empty, cookie.Comment );
			Assert.Null( cookie.CommentUri );
			Assert.False( cookie.Discard );
			Assert.Equal( ".typicode.com", cookie.Domain );
			Assert.True( cookie.Expired );
			Assert.Equal( new DateTime( 2017, 7, 2, 17, 7, 33 ), cookie.Expires );
			Assert.True( cookie.HttpOnly );
			Assert.Equal( "__cfduid", cookie.Name );
			Assert.Equal( "/", cookie.Path );
			Assert.Equal( string.Empty, cookie.Port );
			Assert.False( cookie.Secure );
			var timeStamp = DateTime.Now;
			Assert.True( timeStamp.Subtract( cookie.TimeStamp ).TotalSeconds < 1.0 ); // just a few milliseconds between parse and test
			Assert.Equal( "d9de9c2173f6f46cc6a9457aada153ace1467497253", cookie.Value );
			Assert.Equal( 0, cookie.Version );
		}
	}
}

