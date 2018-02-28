using System;
using System.Runtime.CompilerServices;

namespace NScrape {
	/// <summary>
	/// Provides the base implementation for classes which represent web responses.
	/// </summary>
	public abstract class WebResponse : IDisposable {
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
		/// Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
		/// </summary>
		~WebResponse() {
			Dispose( false );
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
#if !NETSTANDARD1_5
		[MethodImpl( MethodImplOptions.Synchronized )]
#endif
		public void Dispose() {
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		private void Dispose( bool isDisposing ) {
			if ( isDisposed ) {
				return;
			}

			if ( isDisposing ) {
				DisposeManagedRessources();
			}

			DisposeUnmanagedRessources();
			isDisposed = true;
		}

		/// <summary>
		/// Handles disposal of managed resources.
		/// </summary>
		/// <remarks>
		/// Inheriting classes owning managed resources should override this method and use it to dispose of them.
		/// </remarks>
		protected virtual void DisposeManagedRessources() {
		}

		/// <summary>
		/// Handles disposal of unmanaged resources.
		/// </summary>
		/// <remarks>
		/// Inheriting classes owning unmanaged resources should override this method and use it to dispose of them.
		/// </remarks>
		protected virtual void DisposeUnmanagedRessources() {
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
