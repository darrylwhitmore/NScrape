using System;
using System.Globalization;

namespace NScrape {
	/// <summary>
	/// Provides miscellaneous utility functions.
	/// </summary>
	public static class NScrapeUtility {
		/// <summary>
		/// Converts the specified HTTP string representation of a date and time to its <see cref="DateTime"/> equivalent
		/// </summary>
		/// <param name="httpDate">Contains the HTTP date string.</param>
		/// <param name="parsedDate">When this method returns, contains the <see cref="DateTime"/> value equivalent to the date and time
		/// contained in <i>httpDate</i>, if the conversion succeeded, or <see cref="DateTime.MinValue">MinValue</see> if the conversion failed.
		/// </param>
		/// <remarks>
		/// Handles conversion of the HTTP date formats specified in section <b>7.1.1.1. Date/Time Formats</b> of
		/// <see href="http://tools.ietf.org/html/rfc7231">RFC 7231</see>.
		/// </remarks>
		/// <returns><b>true</b> if the date was successfully parsed; <b>false</b> otherwise.</returns>
		public static bool TryParseHttpDate( string httpDate, out DateTime parsedDate ) {
			// http://tools.ietf.org/html/rfc7231#section-7.1.1.1
			var formats = new[] {
				"r",							// preferred
				"dddd, dd-MMM-yy HH:mm:ss GMT",	// obsolete RFC 850 format 
				"ddd MMM  d HH:mm:ss yyyy"		// ANSI C's asctime() format
			};

			return DateTime.TryParseExact( httpDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out parsedDate );
		}
	}
}
