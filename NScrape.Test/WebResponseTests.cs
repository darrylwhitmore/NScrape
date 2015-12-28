using System;
using System.IO;
using Xunit;

namespace NScrape.Test {
	public class WebResponseTests {

		[Fact]
		public void ImageWebResponseTest() {
			var webClient = new WebClient();

			var uri = new Uri( "http://sites.psu.edu/siowfa15/wp-content/uploads/sites/29639/2015/10/cat.jpg" );

			using ( var response = webClient.SendRequest( uri ) ) {
				Assert.True( response.Success );
				Assert.NotNull( response );
				Assert.Equal( WebResponseType.Image, response.ResponseType );

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
				Assert.True( response.Success ); 
				Assert.NotNull( response );
				Assert.Equal( WebResponseType.Binary, response.ResponseType );

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
				Assert.True( response.Success );
				Assert.NotNull( response );
				Assert.Equal( WebResponseType.Binary, response.ResponseType );

				var binaryResponse = response as BinaryWebResponse;
				Assert.NotNull( binaryResponse );

				var data = binaryResponse.Data;
				Assert.NotNull( data );
			}
		}
	}
}


