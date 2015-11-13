using System;
using System.Net;

namespace NScrape {
	/// <summary>
	/// Provides data for the <see cref="WebClient.ResponseReceived"/> event.
	/// </summary>
	public class ResponseReceivedEventArgs : EventArgs {

		internal ResponseReceivedEventArgs( HttpWebResponse response ) {
            WebResponse = response;
		}

		/// <summary>
		/// Gets the web response.
		/// </summary>
		/// <remarks>
		/// Gets the web response that has been received from the server.
		/// </remarks>
		public HttpWebResponse WebResponse { get; private set; }
	}
}
