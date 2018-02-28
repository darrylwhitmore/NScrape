using System;

namespace NScrape {
	/// <summary>
	/// Provides data for the <see cref="WebClient.SendingRequest"/> event.
	/// </summary>
	public class SendingRequestEventArgs : EventArgs {

		internal SendingRequestEventArgs( WebRequest request ) {
			WebRequest = request;
		}

		/// <summary>
		/// Gets the web request.
		/// </summary>
		/// <remarks>
		/// Gets the web request that has been sent to the server.
		/// </remarks>
		public WebRequest WebRequest { get; private set; }
	}
}
