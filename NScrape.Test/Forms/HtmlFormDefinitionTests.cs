using System.Linq;
using NScrape.Forms;
using Xunit;

namespace NScrape.Test.Forms {
	[System.Diagnostics.CodeAnalysis.SuppressMessage( "Assertions", "xUnit2013:Do not use equality check to check for collection size.", Justification = "<Pending>" )]
	public class HtmlFormDefinitionTests {
		[Fact]
		public void TestSingleFormTextsAndDropdown() {
			var parsedForms = HtmlFormDefinition.Parse( Properties.Resources.chicago_craigslist_org ).ToList();

			Assert.NotNull( parsedForms );
			Assert.Equal( 1, parsedForms.Count );

			var form0 = parsedForms.ElementAt( 0 );
			Assert.Equal( Properties.Resources.chicago_craigslist_org, form0.PageHtml );

			// Form attributes
			Assert.Equal( 3, form0.Attributes.Count );
			Assert.True( form0.Attributes.ContainsKey("id") );
			Assert.Equal( "search", form0.Attributes["id"] );
			Assert.True( form0.Attributes.ContainsKey( "action" ) );
			Assert.Equal( "/search/", form0.Attributes["action"] );
			Assert.True( form0.Attributes.ContainsKey( "method" ) );
			Assert.Equal( "GET", form0.Attributes["method"] );

			Assert.Equal( "search", form0.Id );

			// Form controls
			Assert.Equal( 3, form0.Controls.Count() );

			// Control 0 - input hidden
			var control0 = form0.Controls.ElementAt( 0 ) as InputHtmlFormControl;
			Assert.NotNull( control0 );
			Assert.Equal( InputHtmlFormControlType.Hidden, control0.ControlType );

			Assert.Equal( 3, control0.Attributes.Count );
			Assert.False( control0.Attributes.ContainsKey( "id" ) );
			Assert.True( control0.Attributes.ContainsKey( "type" ) );
			Assert.Equal( "hidden", control0.Attributes["type"] );
			Assert.True( control0.Attributes.ContainsKey( "name" ) );
			Assert.Equal( "sort", control0.Attributes["name"] );
			Assert.True( control0.Attributes.ContainsKey( "value" ) );
			Assert.Equal( "rel", control0.Attributes["value"] );

			Assert.Equal( "sort", control0.Name );
			Assert.Null( control0.Id );
			Assert.False( control0.Disabled );
			Assert.Equal( "rel", control0.Value );
			Assert.Equal( "sort=rel", control0.EncodedData );
			control0.Value = "foo";
			Assert.Equal( "sort=foo", control0.EncodedData );
			control0.Disabled = true;
			Assert.True( control0.Disabled );
			Assert.Equal( "sort=foo", control0.EncodedData );

			// Control 1 - input
			var control1 = form0.Controls.ElementAt( 1 ) as InputHtmlFormControl;
			Assert.NotNull( control1 );
			Assert.Equal( InputHtmlFormControlType.Text, control1.ControlType );

			Assert.Equal( 6, control1.Attributes.Count );
			Assert.True( control1.Attributes.ContainsKey( "id" ) );
			Assert.Equal( "query", control1.Attributes["id"] );
			Assert.True( control1.Attributes.ContainsKey( "name" ) );
			Assert.Equal( "query", control1.Attributes["name"] );
			Assert.True( control1.Attributes.ContainsKey( "data-suggest" ) );
			Assert.Equal( "search", control1.Attributes["data-suggest"] );
			Assert.True( control1.Attributes.ContainsKey( "autocorrect" ) );
			Assert.Equal( "off", control1.Attributes["autocorrect"] );
			Assert.True( control1.Attributes.ContainsKey( "autocapitalize" ) );
			Assert.Equal( "off", control1.Attributes["autocapitalize"] );
			Assert.True( control1.Attributes.ContainsKey( "placeholder" ) );
			Assert.Equal( "search", control1.Attributes["placeholder"] );

			Assert.Equal( "query", control1.Name );
			Assert.Equal( "query", control1.Id );
			Assert.False( control1.Disabled );
			Assert.Null( control1.Value );
			Assert.Equal( "query=", control1.EncodedData );
			control1.Value = "foo";
			Assert.Equal( "query=foo", control1.EncodedData );
			control1.Disabled = true;
			Assert.True( control1.Disabled );
			Assert.Equal( "query=foo", control1.EncodedData );

			// Control 2 - select
			var control2 = form0.Controls.ElementAt( 2 ) as SelectHtmlFormControl;
			Assert.NotNull( control2 );

			Assert.Equal( 2, control2.Attributes.Count );
			Assert.True( control2.Attributes.ContainsKey( "id" ) );
			Assert.Equal( "catAbb", control2.Attributes["id"] );
			Assert.True( control2.Attributes.ContainsKey( "name" ) );
			Assert.Equal( "catAbb", control2.Attributes["name"] );

			Assert.Equal( 9, control2.Options.Count );

			Assert.Equal( 1, control2.Options[0].Attributes.Count );
			Assert.True( control2.Options[0].Attributes.ContainsKey( "value" ) );
			Assert.Equal( "ccc", control2.Options[0].Attributes["value"] );
			Assert.Equal( "ccc", control2.Options[0].Value );
			Assert.Equal( "community", control2.Options[0].Option );
			Assert.False( control2.Options[0].Selected );

			Assert.Equal( 1, control2.Options[1].Attributes.Count );
			Assert.True( control2.Options[1].Attributes.ContainsKey( "value" ) );
			Assert.Equal( "eee", control2.Options[1].Attributes["value"] );
			Assert.Equal( "eee", control2.Options[1].Value );
			Assert.Equal( "events", control2.Options[1].Option );
			Assert.False( control2.Options[1].Selected );

			Assert.Equal( 1, control2.Options[2].Attributes.Count );
			Assert.True( control2.Options[2].Attributes.ContainsKey( "value" ) );
			Assert.Equal( "ggg", control2.Options[2].Attributes["value"] );
			Assert.Equal( "ggg", control2.Options[2].Value );
			Assert.Equal( "gigs", control2.Options[2].Option );
			Assert.False( control2.Options[2].Selected );

			Assert.Equal( 1, control2.Options[3].Attributes.Count );
			Assert.True( control2.Options[3].Attributes.ContainsKey( "value" ) );
			Assert.Equal( "hhh", control2.Options[3].Attributes["value"] );
			Assert.Equal( "hhh", control2.Options[3].Value );
			Assert.Equal( "housing", control2.Options[3].Option );
			Assert.False( control2.Options[3].Selected );

			Assert.Equal( 1, control2.Options[4].Attributes.Count );
			Assert.True( control2.Options[4].Attributes.ContainsKey( "value" ) );
			Assert.Equal( "jjj", control2.Options[4].Attributes["value"] );
			Assert.Equal( "jjj", control2.Options[4].Value );
			Assert.Equal( "jobs", control2.Options[4].Option );
			Assert.False( control2.Options[4].Selected );

			Assert.Equal( 1, control2.Options[5].Attributes.Count );
			Assert.True( control2.Options[5].Attributes.ContainsKey( "value" ) );
			Assert.Equal( "ppp", control2.Options[5].Attributes["value"] );
			Assert.Equal( "ppp", control2.Options[5].Value );
			Assert.Equal( "personals", control2.Options[5].Option );
			Assert.False( control2.Options[5].Selected );

			Assert.Equal( 1, control2.Options[6].Attributes.Count );
			Assert.True( control2.Options[6].Attributes.ContainsKey( "value" ) );
			Assert.Equal( "res", control2.Options[6].Attributes["value"] );
			Assert.Equal( "res", control2.Options[6].Value );
			Assert.Equal( "resumes", control2.Options[6].Option );
			Assert.False( control2.Options[6].Selected );

			Assert.Equal( 2, control2.Options[7].Attributes.Count );
			Assert.True( control2.Options[7].Attributes.ContainsKey( "value" ) );
			Assert.Equal( "sss", control2.Options[7].Attributes["value"] );
			Assert.True( control2.Options[7].Attributes.ContainsKey( "selected" ) );
			Assert.Equal( "selected", control2.Options[7].Attributes["selected"] );
			Assert.Equal( "sss", control2.Options[7].Value );
			Assert.Equal( "for sale", control2.Options[7].Option );
			Assert.True( control2.Options[7].Selected );

			Assert.Equal( 1, control2.Options[8].Attributes.Count );
			Assert.True( control2.Options[8].Attributes.ContainsKey( "value" ) );
			Assert.Equal( "bbb", control2.Options[8].Attributes["value"] );
			Assert.Equal( "bbb", control2.Options[8].Value );
			Assert.Equal( "services", control2.Options[8].Option );
			Assert.False( control2.Options[8].Selected );

			Assert.Equal( "catAbb", control2.Name );
			Assert.Equal( "catAbb", control2.Id );
			Assert.False( control2.Disabled );
			Assert.Equal( "catAbb=sss", control2.EncodedData );
			control2.UnselectAll();
			Assert.Equal( string.Empty, control2.EncodedData );
			control2.Options[0].Selected = true;
			control2.Options[8].Selected = true;
			Assert.Equal( "catAbb=ccc&catAbb=bbb", control2.EncodedData );
			control2.Disabled = true;
			Assert.True( control2.Disabled );
			Assert.Equal( "catAbb=ccc&catAbb=bbb", control2.EncodedData );
			control2.UnselectAll();
			Assert.Equal( string.Empty, control2.EncodedData );
		}

		[Fact]
		public void TestTwoFormsRadioAndCheckbox() {
			var parsedForms = HtmlFormDefinition.Parse( Properties.Resources.flickr_com_search_advanced ).ToList();

			Assert.NotNull( parsedForms );
			Assert.Equal( 2, parsedForms.Count );

			var form1 = parsedForms.ElementAt( 1 );
			Assert.Equal( Properties.Resources.flickr_com_search_advanced, form1.PageHtml );

			// Form attributes
			Assert.Equal( 2, form1.Attributes.Count );
			Assert.True( form1.Attributes.ContainsKey( "action" ) );
			Assert.Equal( "/search/advanced/", form1.Attributes["action"] );
			Assert.True( form1.Attributes.ContainsKey( "method" ) );
			Assert.Equal( "post", form1.Attributes["method"] );

			Assert.Null( form1.Id );

			// Form controls
			Assert.Equal( 18, form1.Controls.Count() );

			// Control 5 - checkbox (checked)
			var control5 = form1.Controls.ElementAt( 5 ) as InputCheckBoxHtmlFormControl;
			Assert.NotNull( control5 );
			Assert.Equal( InputHtmlFormControlType.CheckBox, control5.ControlType );

			Assert.Equal( 5, control5.Attributes.Count );
			Assert.True( control5.Attributes.ContainsKey( "id" ) );
			Assert.Equal( "prefs_photos", control5.Attributes["id"] );
			Assert.True( control5.Attributes.ContainsKey( "type" ) );
			Assert.Equal( "checkbox", control5.Attributes["type"] );
			Assert.True( control5.Attributes.ContainsKey( "name" ) );
			Assert.Equal( "prefs_photos", control5.Attributes["name"] );
			Assert.True( control5.Attributes.ContainsKey( "value" ) );
			Assert.Equal( "1", control5.Attributes["value"] );
			Assert.True( control5.Attributes.ContainsKey( "checked" ) );
			Assert.Equal( string.Empty, control5.Attributes["checked"] );

			Assert.Equal( "prefs_photos", control5.Name );
			Assert.Equal( "prefs_photos", control5.Id );
			Assert.True(control5.Checked );
			Assert.False( control5.Disabled );
			Assert.Equal( "1", control5.Value );
			Assert.Equal( "prefs_photos=1", control5.EncodedData );
			control5.Disabled = true;
			Assert.True( control5.Disabled );
			control5.Checked = false;
			Assert.False( control5.Checked );
			Assert.Equal( "prefs_photos=1", control5.EncodedData );

			// Control 6 - checkbox (unchecked)
			var control6 = form1.Controls.ElementAt( 6 ) as InputCheckBoxHtmlFormControl;
			Assert.NotNull( control6 );
			Assert.Equal( InputHtmlFormControlType.CheckBox, control6.ControlType );

			Assert.Equal( 4, control6.Attributes.Count );
			Assert.True( control6.Attributes.ContainsKey( "id" ) );
			Assert.Equal( "prefs_screenshots", control6.Attributes["id"] );
			Assert.True( control6.Attributes.ContainsKey( "type" ) );
			Assert.Equal( "checkbox", control6.Attributes["type"] );
			Assert.True( control6.Attributes.ContainsKey( "name" ) );
			Assert.Equal( "prefs_screenshots", control6.Attributes["name"] );
			Assert.True( control6.Attributes.ContainsKey( "value" ) );
			Assert.Equal( "1", control6.Attributes["value"] );
			Assert.False( control6.Attributes.ContainsKey( "checked" ) );

			Assert.Equal( "prefs_screenshots", control6.Name );
			Assert.Equal( "prefs_screenshots", control6.Id );
			Assert.False( control6.Checked );
			Assert.False( control6.Disabled );
			Assert.Equal( "1", control6.Value );
			Assert.Equal( "prefs_screenshots=1", control6.EncodedData );
			control6.Disabled = true;
			Assert.True( control6.Disabled );
			control6.Checked = true;
			Assert.True( control6.Checked );
			Assert.Equal( "prefs_screenshots=1", control6.EncodedData );

			// Control 8 - radio (checked)
			var control8 = form1.Controls.ElementAt( 8 ) as InputRadioHtmlFormControl;
			Assert.NotNull( control8 );
			Assert.Equal( InputHtmlFormControlType.Radio, control8.ControlType );

			Assert.Equal( 6, control8.Attributes.Count );
			Assert.True( control8.Attributes.ContainsKey( "id" ) );
			Assert.Equal( "media_all", control8.Attributes["id"] );
			Assert.True( control8.Attributes.ContainsKey( "type" ) );
			Assert.Equal( "radio", control8.Attributes["type"] );
			Assert.True( control8.Attributes.ContainsKey( "name" ) );
			Assert.Equal( "media", control8.Attributes["name"] );
			Assert.True( control8.Attributes.ContainsKey( "value" ) );
			Assert.Equal( "all", control8.Attributes["value"] );
			Assert.True( control8.Attributes.ContainsKey( "checked" ) );
			Assert.Equal( string.Empty, control8.Attributes["checked"] );
			Assert.True( control8.Attributes.ContainsKey( "onclick" ) );
			Assert.Equal( "update_media();", control8.Attributes["onclick"] );

			Assert.Equal( "media", control8.Name );
			Assert.Equal( "media_all", control8.Id );
			Assert.True( control8.Checked );
			Assert.False( control8.Disabled );
			Assert.Equal( "all", control8.Value );
			Assert.Equal( "media=all", control8.EncodedData );
			control8.Disabled = true;
			Assert.True( control8.Disabled );
			control8.Checked = false;
			Assert.False( control8.Checked );
			Assert.Equal( "media=all", control8.EncodedData );

			// Control 9 - radio (unchecked)
			var control9 = form1.Controls.ElementAt( 9 ) as InputRadioHtmlFormControl;
			Assert.NotNull( control9 );
			Assert.Equal( InputHtmlFormControlType.Radio, control9.ControlType );

			Assert.Equal( 5, control9.Attributes.Count );
			Assert.True( control9.Attributes.ContainsKey( "id" ) );
			Assert.Equal( "media_photos", control9.Attributes["id"] );
			Assert.True( control9.Attributes.ContainsKey( "type" ) );
			Assert.Equal( "radio", control9.Attributes["type"] );
			Assert.True( control9.Attributes.ContainsKey( "name" ) );
			Assert.Equal( "media", control9.Attributes["name"] );
			Assert.True( control9.Attributes.ContainsKey( "value" ) );
			Assert.Equal( "photos", control9.Attributes["value"] );
			Assert.True( control9.Attributes.ContainsKey( "onclick" ) );
			Assert.Equal( "update_media();", control9.Attributes["onclick"] );

			Assert.Equal( "media", control9.Name );
			Assert.Equal( "media_photos", control9.Id );
			Assert.False( control9.Checked );
			Assert.False( control9.Disabled );
			Assert.Equal( "photos", control9.Value );
			Assert.Equal( "media=photos", control9.EncodedData );
			control9.Disabled = true;
			Assert.True( control9.Disabled );
			control9.Checked = true;
			Assert.True( control9.Checked );
			Assert.Equal( "media=photos", control9.EncodedData );

			// Control 11 - checkbox (unchecked, disabled)
			var control11 = form1.Controls.ElementAt( 11 ) as InputCheckBoxHtmlFormControl;
			Assert.NotNull( control11 );
			Assert.Equal( InputHtmlFormControlType.CheckBox, control11.ControlType );

			Assert.Equal( 5, control11.Attributes.Count );
			Assert.True( control11.Attributes.ContainsKey( "id" ) );
			Assert.Equal( "hd_video", control11.Attributes["id"] );
			Assert.True( control11.Attributes.ContainsKey( "type" ) );
			Assert.Equal( "checkbox", control11.Attributes["type"] );
			Assert.True( control11.Attributes.ContainsKey( "name" ) );
			Assert.Equal( "hd_video", control11.Attributes["name"] );
			Assert.True( control11.Attributes.ContainsKey( "value" ) );
			Assert.Equal( "1", control11.Attributes["value"] );
			Assert.True( control11.Attributes.ContainsKey( "disabled" ) );

			Assert.Equal( "hd_video", control11.Name );
			Assert.Equal( "hd_video", control11.Id );
			Assert.False( control11.Checked );
			Assert.True( control11.Disabled );
			Assert.Equal( "1", control11.Value );
			Assert.Equal( "hd_video=1", control11.EncodedData );
			control11.Disabled = false;
			Assert.False( control11.Disabled );
			control11.Checked = true;
			Assert.True( control11.Checked );
			Assert.Equal( "hd_video=1", control11.EncodedData );

		}

		[Fact]
		public void TestMultipleFormsAndTextarea() {
			var parsedForms = HtmlFormDefinition.Parse( Properties.Resources.quackit_com_html_codes_comment_box_code ).ToList();

			Assert.NotNull( parsedForms );
			Assert.Equal( 12, parsedForms.Count );

			var form1 = parsedForms.ElementAt( 10 );
			Assert.Equal( Properties.Resources.quackit_com_html_codes_comment_box_code, form1.PageHtml );

			// Form attributes
			Assert.Equal( 2, form1.Attributes.Count );
			Assert.True( form1.Attributes.ContainsKey( "action" ) );
			Assert.Equal( "/html/tags/html_form_tag_action.cfm", form1.Attributes["action"] );
			Assert.True( form1.Attributes.ContainsKey( "method" ) );
			Assert.Equal( "post", form1.Attributes["method"] );

			Assert.Null( form1.Id );

			// Form controls
			Assert.Equal( 1, form1.Controls.Count() );

			// Control 0 - textarea
			var control0 = form1.Controls.ElementAt( 0 ) as TextAreaHtmlFormControl;
			Assert.NotNull( control0 );

			Assert.Equal( 3, control0.Attributes.Count );
			Assert.True( control0.Attributes.ContainsKey( "id" ) );
			Assert.Equal( "comments", control0.Attributes["id"] );
			Assert.True( control0.Attributes.ContainsKey( "name" ) );
			Assert.Equal( "comments", control0.Attributes["name"] );
			Assert.True( control0.Attributes.ContainsKey( "style" ) );
			Assert.Equal( "width:140px;height:100px;font-family:cursive;border:7px outset steelblue;", control0.Attributes["style"] );

			Assert.Equal( "comments", control0.Name );
			Assert.Equal( "comments", control0.Id );
			Assert.False( control0.Disabled );
			Assert.Equal( "\r\n\t\tEnter your comments here...\r\n\t\t\r\n\t\t...and watch your comment box grow scrollbars!\r\n\t\t", control0.Text );
			Assert.Equal( "comments=%0d%0a%09%09Enter+your+comments+here...%0d%0a%09%09%0d%0a%09%09...and+watch+your+comment+box+grow+scrollbars!%0d%0a%09%09", control0.EncodedData );
			control0.Disabled = true;
			Assert.True( control0.Disabled );
			control0.Text = "fubar";
			Assert.Equal( "comments=fubar", control0.EncodedData );
			control0.Text = "";
			Assert.Equal( "comments=", control0.EncodedData );
		}

		[Fact]
		public void TestPasswordText() {
			var parsedForms = HtmlFormDefinition.Parse( Properties.Resources.accounts_craigslist_org_login ).ToList();

			Assert.NotNull( parsedForms );
			Assert.Equal( 1, parsedForms.Count );

			var form0 = parsedForms.ElementAt( 0 );
			Assert.Equal( Properties.Resources.accounts_craigslist_org_login, form0.PageHtml );

			// Form attributes
			Assert.Equal( 3, form0.Attributes.Count );
			Assert.True( form0.Attributes.ContainsKey( "name" ) );
			Assert.Equal( "login", form0.Attributes["name"] );
			Assert.True( form0.Attributes.ContainsKey( "action" ) );
			Assert.Equal( "https://accounts.craigslist.org/login", form0.Attributes["action"] );
			Assert.True( form0.Attributes.ContainsKey( "method" ) );
			Assert.Equal( "post", form0.Attributes["method"] );

			Assert.Null( form0.Id );

			// Form controls
			Assert.Equal( 6, form0.Controls.Count() );

			// Control 5 - input password
			var control5 = form0.Controls.ElementAt( 5 ) as InputHtmlFormControl;
			Assert.NotNull( control5 );
			Assert.Equal( InputHtmlFormControlType.Password, control5.ControlType );

			Assert.Equal( 3, control5.Attributes.Count );
			Assert.True( control5.Attributes.ContainsKey( "id" ) );
			Assert.Equal( "inputPassword", control5.Attributes["id"] );
			Assert.True( control5.Attributes.ContainsKey( "type" ) );
			Assert.Equal( "password", control5.Attributes["type"] );
			Assert.True( control5.Attributes.ContainsKey( "name" ) );
			Assert.Equal( "inputPassword", control5.Attributes["name"] );

			Assert.Equal( "inputPassword", control5.Name );
			Assert.Equal( "inputPassword", control5.Id );
			Assert.False( control5.Disabled );
			Assert.Null( control5.Value );
			Assert.Equal( "inputPassword=", control5.EncodedData );
			control5.Value = "foo";
			Assert.Equal( "inputPassword=foo", control5.EncodedData );
			control5.Disabled = true;
			Assert.True( control5.Disabled );
			Assert.Equal( "inputPassword=foo", control5.EncodedData );
		}
	}
}
