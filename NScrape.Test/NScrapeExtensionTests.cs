using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NScrape.Test;

public class NScrapeExtensionTests {
	[Fact]
	public async Task GetEncodingTest() {
#pragma warning disable SYSLIB0014
		var request = System.Net.WebRequest.Create( "https://www.google.com/" );
#pragma warning restore SYSLIB0014
		var response = await request.GetResponseAsync();
		var httpWebResponse = response as HttpWebResponse;

		Assert.NotNull( httpWebResponse );
		Assert.Equal( Encoding.GetEncoding( "ISO-8859-1" ), httpWebResponse.GetEncoding() );
	}

	[Fact]
	public async Task GetResponseTextTest() {
		const string testText = "<title>Lorem Ipsum - All the facts - Lipsum generator</title>";

#pragma warning disable SYSLIB0014
		var request = System.Net.WebRequest.Create( "http://www.lipsum.com/" );
#pragma warning restore SYSLIB0014
		var response = await request.GetResponseAsync();
		var httpWebResponse = response as HttpWebResponse;
		Assert.NotNull( httpWebResponse );
		var html = httpWebResponse.GetResponseText();
		Assert.NotNull( html );
		Assert.Contains( testText, html );

#pragma warning disable SYSLIB0014
		request = System.Net.WebRequest.Create( "http://www.lipsum.com/" );
#pragma warning restore SYSLIB0014
		response = await request.GetResponseAsync();
		httpWebResponse = response as HttpWebResponse;
		Assert.NotNull( httpWebResponse );
		html = httpWebResponse.GetResponseText( Encoding.GetEncoding( "ISO-8859-1" ) );
		Assert.NotNull( html );
		Assert.Contains( testText, html );
	}
}