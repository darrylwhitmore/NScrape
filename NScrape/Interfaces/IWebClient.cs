using System;
using System.Net;
using WebRequest = NScrape.Requests.WebRequest;

namespace NScrape.Interfaces {
	/// <summary>
	/// Defines the contract for a web client that manages cookies, handles redirection, 
	/// and facilitates sending HTTP requests and processing responses.
	/// </summary>
	public interface IWebClient {
		/// <include file='IWebClient.xml' path='/IWebClient/AddingCookie/*'/>
		event EventHandler<AddingCookieEventArgs> AddingCookie;

		/// <include file='IWebClient.xml' path='/IWebClient/SendingRequest/*'/>
		event EventHandler<SendingRequestEventArgs> SendingRequest;

		/// <include file='IWebClient.xml' path='/IWebClient/ProcessingResponse/*'/>
		event EventHandler<ProcessingResponseEventArgs> ProcessingResponse;

		/// <include file='IWebClient.xml' path='/IWebClient/CookieJar/*'/>
		CookieContainer CookieJar { get; }

		/// <include file='IWebClient.xml' path='/IWebClient/SendRequest_Uri/*'/>
		IWebResponse SendRequest( Uri destination );
		/// <include file='IWebClient.xml' path='/IWebClient/SendRequest_Uri_bool/*'/>
		IWebResponse SendRequest( Uri destination, bool autoRedirect );

		/// <include file='IWebClient.xml' path='/IWebClient/SendRequest_Uri_string/*'/>
		IWebResponse SendRequest( Uri destination, string requestData );

		/// <include file='IWebClient.xml' path='/IWebClient/SendRequest_Uri_string_bool/*'/>
		IWebResponse SendRequest( Uri destination, string requestData, bool autoRedirect );

		/// <include file='IWebClient.xml' path='/IWebClient/SendRequest_WebRequest/*'/>
		IWebResponse SendRequest( WebRequest webRequest );

		/// <include file='IWebClient.xml' path='/IWebClient/UserAgent/*'/>
		string UserAgent { get; set; }
	}
}

