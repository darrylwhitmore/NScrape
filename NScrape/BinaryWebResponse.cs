using System;
using System.IO;
using System.Net;

namespace NScrape {
    /// <summary>
	/// Represents a web response for a request that returned binary data.
	/// </summary>
    public class BinaryWebResponse : WebResponse {
		private byte[] data;
		private readonly HttpWebResponse webResponse;

	    /// <summary>
	    /// Initializes a new instance of the <see cref="BinaryWebResponse"/> class.
	    /// </summary>
	    /// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
	    /// <param name="responseUrl">The URL of the response.</param>
	    /// <param name="data">The data that was returned by the web server.</param>
	    /// <remarks>
		/// Deprecated; please use <see cref="BinaryWebResponse( bool, HttpWebResponse )"/> instead.
	    /// </remarks>
		[Obsolete( "Please use BinaryWebResponse( bool, HttpWebResponse ) instead." )]
		public BinaryWebResponse( bool success, Uri responseUrl, byte[] data )
            : base( success, responseUrl, WebResponseType.Binary ) {
            this.data = data;
        }

	    /// <summary>
	    /// Initializes a new instance of the <see cref="BinaryWebResponse"/> class.
	    /// </summary>
		/// <param name="success"><b>true</b> if the response is considered successful, <b>false</b> otherwise.</param>
		/// <param name="webResponse">The web response object.</param>
	    public BinaryWebResponse( bool success, HttpWebResponse webResponse )
			: base( success, webResponse.ResponseUri, WebResponseType.Binary ) {
			this.webResponse = webResponse;
		}

		/// <summary>
		/// Closes the binary response stream.
		/// </summary>
		/// <remarks>
		/// The Close method closes the binary response stream and releases the connection to the resource for reuse by other requests.
		/// <br/><br/>
		/// You must call either the <see cref="Stream.Close">Stream.Close</see> or the BinaryWebResponse.Close method to close the stream and release the
		/// connection for reuse. It is not necessary to call both <see cref="Stream.Close">Stream.Close</see> and BinaryWebResponse.Close, but doing so does not cause an
		/// error. Failure to close the stream can cause your application to run out of connections.
		/// </remarks>
		/// <seealso cref="GetResponseStream"/>
		public void Close() {
			if ( webResponse != null ) {
				webResponse.Close();
			}
		}

	    /// <summary>
		/// Gets the binary data.
		/// </summary>
		/// <remarks>
		/// Deprecated; please use <see cref="BinaryWebResponse.GetResponseStream()"/> instead.
		/// </remarks>
		[Obsolete( "Please use BinaryWebResponse.GetResponseStream() instead." )]
		public byte[] Data {
			get {
				if ( data == null ) {
					// Backwards compatibility. 
					using ( var s = GetResponseStream() ) {
						// Skeet says a null is unlikely, but check to make Resharper happy.
						// http://stackoverflow.com/questions/16911056/can-webresponse-getresponsestream-return-a-null
						if ( s == null ) {
							throw new IOException( "HttpWebResponse.GetResponseStream() has returned null" );
						}

						using ( var ms = new MemoryStream() ) {
							s.CopyTo( ms );

							data = ms.ToArray();
						}
					}
				}

				return data;
			}
		}

	    /// <summary>
	    /// Handles disposal of managed resources.
	    /// </summary>
	    /// <remarks>
	    /// Inheriting classes owning managed resources should override this method and use it to dispose of them.
	    /// </remarks>
	    protected override void DisposeManagedRessources() {
		    base.DisposeManagedRessources();

			if ( webResponse != null ) {
				webResponse.Dispose();
			}
		}

	    /// <summary>
		/// Gets the stream that is used to read the binary response.
	    /// </summary>
		/// <returns>A <see cref="Stream"/> containing the binary response.</returns>
		/// <remarks>
		/// The GetResponseStream method returns the binary data stream from the response.
		/// <br/><br/>
		/// <note>
		/// You must call either the <see cref="Stream.Close">Stream.Close</see> or the <see cref="BinaryWebResponse.Close"/> method to close the stream and release the
		/// connection for reuse. It is not necessary to call both <see cref="Stream.Close">Stream.Close</see> and <see cref="BinaryWebResponse.Close"/>, but doing so does not cause an
		/// error. Failure to close the stream can cause your application to run out of connections.
		/// </note>
		/// </remarks>
		/// <seealso cref="Close"/>
	    public Stream GetResponseStream() {
		    if ( webResponse != null ) {
			    return webResponse.GetResponseStream();
		    }

			throw new InvalidOperationException( "This object was not instantiated with a valid HttpWebResponse object." );
	    }
    }
}
