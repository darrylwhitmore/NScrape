using System;
#if !NETSTANDARD1_5
using System.Runtime.Serialization;
#endif

namespace NScrape {
	/// <summary>
	/// The exception that is thrown when a web scraping operation fails.
	/// </summary>
#if !NETSTANDARD1_5
	[Serializable]
#endif
	public class ScrapeException : Exception {
		/// <summary>
		/// Initializes a new instance of the <see cref="ScrapeException"/> class.
		/// </summary>
		public ScrapeException() {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ScrapeException"/> class with a specified error message.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		public ScrapeException( string message )
			: base( message ) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ScrapeException"/> class with a specified error message and the HTML in question.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="html">Contains the HTML text that could not be scraped.</param>
		public ScrapeException( string message, string html )
			: base( message ) {
			Html = html;
		}

#if !NETSTANDARD1_5
		/// <summary>
		/// Initializes a new instance of the <see cref="ScrapeException"/> class with serialized data.
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected ScrapeException( SerializationInfo info, StreamingContext context )
			: base( info, context ) {
		}
#endif

		/// <summary>
		/// Initializes a new instance of the <see cref="ScrapeException"/> class with a specified error message, the HTML in question and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="html">Contains the HTML text that could not be scraped.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
		public ScrapeException( string message, string html, Exception innerException )
			: base( message, innerException ) {
			Html = html;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ScrapeException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
		public ScrapeException( string message, Exception innerException )
			: base( message, innerException ) {
		}

#if !NETSTANDARD1_5
		/// <summary>
		/// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown. </param><param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination. </param><exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is a null reference (Nothing in Visual Basic). </exception><filterpriority>2</filterpriority><PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*"/><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter"/></PermissionSet>
		public override void GetObjectData( SerializationInfo info, StreamingContext context ) {
			base.GetObjectData( info, context );

			info.AddValue( "Html", Html );
		}
#endif

		/// <summary>
		/// Gets the HTML text that could not be scraped.
		/// </summary>
		public string Html { get; private set; }
	}
}
