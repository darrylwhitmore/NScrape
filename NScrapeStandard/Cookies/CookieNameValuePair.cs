namespace NScrape.Cookies {
	internal class CookieNameValuePair {

		public CookieNameValuePair( string name, string value = "" ) {
			Name = name;
			Value = value;
		}

		public string Name { get; }
		public string Value { get; }

		public override string ToString() {
			return $"{Name}={Value}";
		}
	}
}
