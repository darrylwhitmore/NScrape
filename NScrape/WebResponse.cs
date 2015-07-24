using System;

namespace NScrape {
	/// <summary>
	/// Provides the base implementation for classes which represent web responses.
	/// </summary>
	public abstract class WebResponse {
		private readonly WebResponseType responseType;
		private readonly Uri responseUrl;
		private readonly bool success;

		/// <summary>
		/// Initializes a new instance of the <see cref="WebResponse"/> class.
		/// </summary>
		/// <param name="responseUrl">The URL of the response.</param>
		/// <param name="responseType">The type of response.</param>
		/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
		protected WebResponse( Uri responseUrl, WebResponseType responseType, bool success ) {
			this.responseUrl = responseUrl;
			this.responseType = responseType;
			this.success = success;
		}

		/// <summary>
		/// Gets a value indicating whether the response is considered successful.
		/// </summary>
		public bool Success { get { return success; } }

		/// <summary>
		/// Gets the type of the response.
		/// </summary>
		public WebResponseType ResponseType { get { return responseType; } }

		/// <summary>
		/// Gets the URL of the response.
		/// </summary>
		public Uri ResponseUrl { get { return responseUrl; } }
	}
}
