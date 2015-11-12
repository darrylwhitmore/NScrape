using System;
using System.Net;

namespace NScrape
{
    /// <summary>
    /// Represents a web client that handles cookies and redirection.
    /// </summary>
    public interface IWebClient
    {
        /// <include file='IWebClient.xml' path='/IWebClient/AddingCookie'/>
		event EventHandler<AddingCookieEventArgs> AddingCookie;

        /// <include file='IWebClient.xml' path='/IWebClient/SendingRequest'/>
        event EventHandler<SendingRequestEventArgs> SendingRequest;

        /// <include file='IWebClient.xml' path='/IWebClient/CookieJar'/>
        CookieContainer CookieJar { get; }

        /// <include file='IWebClient.xml' path='/IWebClient/SendRequest_Uri'/>
        WebResponse SendRequest(Uri destination);
        /// <include file='IWebClient.xml' path='/IWebClient/SendRequest_Uri_bool'/>
        WebResponse SendRequest(Uri destination, bool autoRedirect);

        /// <include file='IWebClient.xml' path='/IWebClient/SendRequest_Uri_string'/>
        WebResponse SendRequest(Uri destination, string requestData);

        /// <include file='IWebClient.xml' path='/IWebClient/SendRequest_Uri_string_bool'/>
        WebResponse SendRequest(Uri destination, string requestData, bool autoRedirect);

        /// <include file='IWebClient.xml' path='/IWebClient/SendRequest_WebRequest'/>
        WebResponse SendRequest(WebRequest webRequest);

        /// <include file='IWebClient.xml' path='/IWebClient/UserAgent'/>
        string UserAgent { get; set; }
    }
}

