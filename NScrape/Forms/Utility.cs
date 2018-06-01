using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NScrape.Forms {

	internal static class Utility {

		internal static Dictionary<string, string> ParseAttributes( string html ) {
			var attributes = new Dictionary<string, string>( StringComparer.OrdinalIgnoreCase );

			var matches = RegexCache.Instance.Regex( RegexLibrary.ParseAttributes, RegexLibrary.ParseAttributesOptions ).Matches( html );

			foreach ( Match match in matches ) {
				// Normalize the attribute name to lowercase
				var attributeName = match.Groups[RegexLibrary.ParseAttributesNameGroup].Value.ToLowerInvariant();

				// In the case of duplicate attributes, take the first one.
				if ( !attributes.ContainsKey( attributeName ) ) {
					attributes.Add( attributeName, match.Groups[RegexLibrary.ParseAttributesValueGroup].Value );
				}
			}

			return attributes;
		}
	}
}
