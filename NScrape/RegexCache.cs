using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NScrape {
	internal class RegexCache {
		private readonly Dictionary<string, Regex> cache = new Dictionary<string, Regex>();

        private RegexCache() {
        }
        
        public static readonly RegexCache Instance = new RegexCache();

        public void Clear() {
            cache.Clear();
        }

        public Regex Regex( string pattern ) {
            return Regex( pattern, RegexOptions.None );
        }

		public Regex Regex( string pattern, RegexOptions options ) {
			var key = string.Format( CultureInfo.InvariantCulture, "{0}:{1}", pattern.GetHashCode(), options.GetHashCode() );

			if ( cache.ContainsKey( key ) ) {
				return cache[key];
			}

			var regex = new Regex( pattern, options );

			cache.Add( key, regex );

			return regex;
		}
    }
}
