namespace NScrape {
	/// <summary>
	/// Indicates the type of web response.
	/// </summary>
	public enum WebResponseType {
		/// <summary>
		/// An exception response.
		/// </summary>
		Exception,

		/// <summary>
		/// An HTML response.
		/// </summary>
		Html,

		/// <summary>
		/// An image response.
		/// </summary>
		Image,

		/// <summary>
		/// A JavaScript response.
		/// </summary>
		JavaScript,

		/// <summary>
		/// A JSON response.
		/// </summary>
		Json,

		/// <summary>
		/// A redirect response.
		/// </summary>
		Redirect,

		/// <summary>
		/// A plain text response.
		/// </summary>
		PlainText,

		/// <summary>
		/// An unsupported response.
		/// </summary>
		Unsupported,

		/// <summary>
		/// An XML response.
		/// </summary>
		Xml,

		/// <summary>
		/// A binary response.
		/// </summary>
		Binary
	}
}
