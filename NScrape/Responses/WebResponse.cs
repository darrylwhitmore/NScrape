using System;
using NScrape.Interfaces;

namespace NScrape.Responses {
	/// <summary>
	/// Provides the base implementation for classes which represent web responses.
	/// </summary>
	public abstract class WebResponse : IWebResponse {
		private bool isDisposed;
		private readonly WebResponseType responseType;
		private readonly Uri responseUrl;
		private readonly bool success;

		/// <summary>
		/// Initializes a new instance of the <see cref="WebResponse"/> class.
		/// </summary>
		/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
		/// <param name="responseUrl">The URL of the response.</param>
		/// <param name="responseType">The type of response.</param>
		protected WebResponse( bool success, Uri responseUrl, WebResponseType responseType ) {
			this.responseUrl = responseUrl;
			this.responseType = responseType;
			this.success = success;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		public void Dispose() {
			if ( isDisposed ) {
				return;
			}

			DisposeResources();
			isDisposed = true;
		}

		/// <summary>
		/// Handles disposal of resources.
		/// </summary>
		/// <remarks>
		/// Inheriting classes owning disposable resources (such as an underlying <see cref="System.Net.HttpWebResponse"/>)
		/// should override this method and use it to dispose of them.
		/// </remarks>
		protected virtual void DisposeResources() {
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
