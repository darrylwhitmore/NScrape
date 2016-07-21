using NScrape.Cookies;
using Sprache;
using Xunit;

namespace NScrape.Test.Cookies {
	public class SetCookieHeaderGrammarTests {
		[Fact]
		public void Test() {
			const string cookie1 = "ADCDownloadAuth=fooooooooooooo;Version=1;Comment=;Domain=apple.com;Path=/;Max-Age=108000;HttpOnly;Expires=Tue, 03 May 2016 13:30:57 GMT";

			var result = SetCookieHeaderGrammar.SetCookieHeader.Parse( cookie1 );

			//Assert.Equal(1, result.)


		}
	}
}
