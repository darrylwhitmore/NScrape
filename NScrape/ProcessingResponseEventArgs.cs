using System;
using System.Net;

namespace NScrape {
	/// <summary>
	/// Provides data for the <see cref="WebClient.ProcessingResponse"/> event.
	/// </summary>
	public class ProcessingResponseEventArgs : EventArgs {

		internal ProcessingResponseEventArgs( HttpWebResponse response ) {
            Response = response;
		}

		/// <summary>
		/// Gets the web response.
		/// </summary>
		/// <remarks>
		/// Gets the web response that has been received from the server.
		/// </remarks>
		public HttpWebResponse Response { get; private set; }
	}
}
