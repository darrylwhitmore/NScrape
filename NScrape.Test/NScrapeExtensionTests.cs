using System.Net;
using System.Text;
using Xunit;

namespace NScrape.Test {
	public class NScrapeExtensionTests {
		[Fact]
		public void GetEncodingTest() {
			var request = System.Net.WebRequest.Create( "https://www.wikipedia.org/" );
			var response = request.GetResponse();
			var httpWebResponse = response as HttpWebResponse;

			Assert.NotNull( httpWebResponse );
			Assert.Equal( Encoding.GetEncoding( "ISO-8859-1" ), httpWebResponse.GetEncoding() );
		}

		[Fact]
		public void GetEncodingBackwardsCompatibilityTest() {
			var request = System.Net.WebRequest.Create( "https://www.wikipedia.org/" );
			var response = request.GetResponse();
			var httpWebResponse = response as HttpWebResponse;

			Assert.NotNull( httpWebResponse );
			Assert.Equal( Encoding.GetEncoding( "ISO-8859-1" ), WebResponseFactory.GetEncoding( httpWebResponse ) );
		}

		[Fact]
		public void GetResponseTextTest() {
			const string testText = "<title>Lorem Ipsum - All the facts - Lipsum generator</title>";

			var request = System.Net.WebRequest.Create( "http://www.lipsum.com/" );
			var response = request.GetResponse();
			var httpWebResponse = response as HttpWebResponse;
			Assert.NotNull( httpWebResponse );
			var html = httpWebResponse.GetResponseText();
			Assert.NotNull( html );
			Assert.Contains( testText, html );

			request = System.Net.WebRequest.Create( "http://www.lipsum.com/" );
			response = request.GetResponse();
			httpWebResponse = response as HttpWebResponse;
			Assert.NotNull( httpWebResponse );
			html = httpWebResponse.GetResponseText( Encoding.GetEncoding( "ISO-8859-1" ) );
			Assert.NotNull( html );
			Assert.Contains( testText, html );
		}

		[Fact]
		public void ReadResponseTextBackwardsCompatibilityTest() {
			const string testText = "<title>Lorem Ipsum - All the facts - Lipsum generator</title>";

			var request = System.Net.WebRequest.Create( "http://www.lipsum.com/" );
			var response = request.GetResponse();
			var httpWebResponse = response as HttpWebResponse;
			Assert.NotNull( httpWebResponse );
			var html = WebResponseFactory.ReadResponseText( httpWebResponse );
			Assert.NotNull( html );
			Assert.Contains( testText, html );

			request = System.Net.WebRequest.Create( "http://www.lipsum.com/" );
			response = request.GetResponse();
			httpWebResponse = response as HttpWebResponse;
			Assert.NotNull( httpWebResponse );
			html = WebResponseFactory.ReadResponseText( httpWebResponse, Encoding.GetEncoding( "ISO-8859-1" ) );
			Assert.NotNull( html );
			Assert.Contains( testText, html );
		}

	}
}
