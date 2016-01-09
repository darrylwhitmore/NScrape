using System;
using System.IO;
using System.Linq;
using Xunit;

namespace NScrape.Test {
	public class WebResponseTests {

		[Fact]
		public void XmlWebResponseTest() {
			var webClient = new WebClient();

			var uri = new Uri( "http://www.xmlfiles.com/examples/cd_catalog.xml" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.Xml, response.ResponseType );
				Assert.Equal( uri, response.ResponseUrl );

				var xmlResponse = response as XmlWebResponse;
				Assert.NotNull( xmlResponse );

				Assert.NotNull( xmlResponse.XDocument );
				var catalog = xmlResponse.XDocument.Element( "CATALOG" );
				Assert.NotNull( catalog );
				var cds = catalog.Elements( "CD" );
				Assert.NotNull( cds );
				Assert.Equal( 26, cds.Count() );
			}
		}

		[Fact]
		public void PlainTextWebResponseTest() {
			var webClient = new WebClient();

			var uri = new Uri( "https://www.cl.cam.ac.uk/~mgk25/ucs/examples/UTF-8-demo.txt" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.PlainText, response.ResponseType );
				Assert.Equal( uri, response.ResponseUrl );

				var plainTextResponse = response as PlainTextWebResponse;
				Assert.NotNull( plainTextResponse );

				Assert.NotNull( plainTextResponse.PlainText );
				Assert.Contains( "∮ E⋅da = Q,  n → ∞, ∑ f(i) = ∏ g(i)", plainTextResponse.PlainText );
			}
		}

		[Fact]
		public void ImageWebResponseTest() {
			var webClient = new WebClient();

			var uri = new Uri( "http://sites.psu.edu/siowfa15/wp-content/uploads/sites/29639/2015/10/cat.jpg" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.Image, response.ResponseType );
				Assert.Equal( uri, response.ResponseUrl );

				var imageResponse = response as ImageWebResponse;
				Assert.NotNull( imageResponse );

				Assert.NotNull( imageResponse.Image );
			}
		}

		[Fact]
		public void BinaryWebResponseTest() {
			var webClient = new WebClient();

			var uri = new Uri( "https://download-cdn.getsync.com/stable/windows64/BitTorrent-Sync_x64.exe" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success ); 
				Assert.Equal( WebResponseType.Binary, response.ResponseType );
				Assert.Equal( uri, response.ResponseUrl );

				var binaryResponse = response as BinaryWebResponse;
				Assert.NotNull( binaryResponse );

				byte[] data;

				var s = binaryResponse.GetResponseStream();
				Assert.NotNull( s );

				using ( var ms = new MemoryStream() ) {
					s.CopyTo( ms );

					data = ms.ToArray();
				}

				Assert.NotNull( data );
			}
		}

		[Fact]
		public void BinaryWebResponseDataPropertyBackwardsCompatibilityTest() {
			var webClient = new WebClient();

			var uri = new Uri( "https://download-cdn.getsync.com/stable/windows64/BitTorrent-Sync_x64.exe" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.NotNull( response );
				Assert.True( response.Success );
				Assert.Equal( WebResponseType.Binary, response.ResponseType );
				Assert.Equal( uri, response.ResponseUrl );

				var binaryResponse = response as BinaryWebResponse;
				Assert.NotNull( binaryResponse );

				var data = binaryResponse.Data;
				Assert.NotNull( data );
			}
		}
	}
}


