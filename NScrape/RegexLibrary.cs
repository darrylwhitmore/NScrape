using System.Text.RegularExpressions;

namespace NScrape {
    internal static class RegexLibrary {
        // Parse attributes
		public const string ParseAttributes = "(?<name>[^\\s=]+)  (?: (?# empty) (?:\\s+|$) | (?# unquoted) \\s*=\\s* (?<value>\\w+)(?:\\s+|$)  | (?# single/double quoted) \\s*=\\s* (?<delimiter>'|\\\")?  (?<value>.*?) \\k<delimiter> | (?# non-standard empty) \\s*=(?:\\s+|$) )  #http://www.w3.org/TR/html-markup/syntax.html#syntax-attributes";
		public const string ParseAttributesNameGroup = "name";
		public const string ParseAttributesValueGroup = "value";
		public const RegexOptions ParseAttributesOptions = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;

		// Parse FORMs
		public const string ParseForms = "<form (?<attributes>.*?) > (?<body>.*?) </form>";
		public const string ParseFormsAttributesGroup = "attributes";
		public const string ParseFormsBodyGroup = "body";
		public const RegexOptions ParseFormsOptions = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;

		public const string ParseFormControls = "<input (?<input>.*?) /?> | <select (?<select>.*?) </select> | <textarea (?<textarea>.*?) </textarea>";
		public const string ParseFormControlsInputGroup = "input";
		public const string ParseFormControlsSelectGroup = "select";
		public const string ParseFormControlsTextAreaGroup = "textarea";
		public const RegexOptions ParseFormControlsOptions = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;

		// Parse an INPUT
		public const string ParseInput = "<input (?<attributes>.*?) /?>";
		public const string ParseInputAttributesGroup = "attributes";
		public const RegexOptions ParseInputOptions = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;

		// Parse META REFRESH
		public const string ParseMetaRefresh = "(?<startNoScript> <noscript>.*? )?  <meta \\s+ http-equiv \\s*=\\s* \"refresh\" .*? url \\s*=\\s* (?<url> .*? ) \"\\s*?/?> (?<endNoScript> .*?</noscript> )?";
		public const string ParseMetaRefreshStartNoScriptGroup = "startNoScript";
		public const string ParseMetaRefreshUrlGroup = "url";
		public const string ParseMetaRefreshEndNoScriptGroup = "endNoScript";
		public const RegexOptions ParseMetaRefreshOptions = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;

		// Parse a SELECT
		public const string ParseSelect = "<select(?<attributes>.*?)>(?<options>.*?)</select>";
		public const string ParseSelectAttributesGroup = "attributes";
		public const string ParseSelectOptionsGroup = "options";
		public const RegexOptions ParseSelectOptions = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;

		// Parse a list of SELECT OPTIONS
		public const string ParseOptionList = "<option\\s.*?</option>";
		public const RegexOptions ParseOptionListOptions = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;

		// Parse a single SELECT OPTION
		public const string ParseOption = "<option\\s+ (?<attributes>.*?) > (?<option>.*?) </option>";
		public const string ParseOptionAttributesGroup = "attributes";
		public const string ParseOptionOptionGroup = "option";
		public const RegexOptions ParseOptionOptions = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;

		// Parse a TEXTAREA
	    public const string ParseTextArea = "<textarea(?<attributes>.*?)>(?<text>.*?)</textarea>";
		public const string ParseTextAreaAttributesGroup = "attributes";
		public const string ParseTextAreaTextGroup = "text";
		public const RegexOptions ParseTextAreaOptions = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;

		// Match code in __doPostBack()
		public const string MatchDoPostBack = "__EVENTTARGET\\.value\\s*=\\s*eventTarget\\.split";
		public const RegexOptions MatchDoPostBackOptions = RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace;
    }
}
