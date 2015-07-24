namespace NScrape {
	/// <summary>
	/// Indicates the type of web response.
	/// </summary>
	public enum WebResponseType {
		/// <summary>
		/// An exception reponse.
		/// </summary>
		Exception,

		/// <summary>
		/// An HTML reponse.
		/// </summary>
		Html,

		/// <summary>
		/// An image reponse.
		/// </summary>
		Image,

		/// <summary>
		/// A JavaScript reponse.
		/// </summary>
		JavaScript,

		/// <summary>
		/// A redirect reponse.
		/// </summary>
		Redirect,

		/// <summary>
		/// A plain text reponse.
		/// </summary>
		PlainText,

		/// <summary>
		/// An unsupported reponse.
		/// </summary>
		Unsupported,

		/// <summary>
		/// An XML reponse.
		/// </summary>
		Xml
	}
}
