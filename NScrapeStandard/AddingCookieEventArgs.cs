using System.ComponentModel;
using System.Net;

namespace NScrape {
	/// <summary>
	/// Provides data for the <see cref="WebClient.AddingCookie"/> event.
	/// </summary>
	public class AddingCookieEventArgs : CancelEventArgs {
		private readonly Cookie cookie;

		internal AddingCookieEventArgs( Cookie cookie ) {
			this.cookie = cookie;
		}

		/// <summary>
		/// Gets the cookie.
		/// </summary>
		/// <remarks>
		/// Gets the cookie that has been added to the <see cref="WebClient.CookieJar"/>.
		/// </remarks>
		public Cookie Cookie { get { return cookie; } }
	}
}
