﻿﻿﻿﻿NScrape is a web scraping framework for .NET that helps with much of the grunt work involved in web scraping, leaving you to concentrate on the scraping itself. NScrape recommends and supports scraping via the [HTML Agility Pack](http://html-agility-pack.net/), but if you'd like to use string functions or regular expressions, feel free! 

## Installation
Install the NScrape [nuget package](https://www.nuget.org/packages/NScrape/): `Install-Package NScrape`

## Reference
The NScrape API reference is available [here](https://darrylwhitmore.github.io/NScrape/reference).

## Tutorial
We'll use the US National Weather Service page at [http://www.weather.gov/](http://www.weather.gov/) for our example. This page has a simple search form that will allow us to look up the weather for a given location. Try it out: enter a location and click the button to submit the form.

![Search box on weather.gov](Assets/Search%20box%20on%20weather%20page.jpg)

In the resulting page, identify the *condition* and *temperature* values; this is what we're going to scrape. View the source using your browser's developer tools. These values can be uniquely identified by the *class* attributes *myforecast-current* and *myforecast-current-lrg*:

![Using browser development tools](Assets/Fairbanks%20Alaska%20weather%20developer%20tools%20result.jpg)

Alternately, you can find these classes by viewing the page HTML:

![Using HTML source](Assets/Fairbanks%20Alaska%20weather%20HTML%20result.jpg)

### Implement a Scraper
Let's implement a scraper to scrape the *condition* and *temperature* values from the page. We create a class that inherits from the [Scraper](https://nscrape.com/reference/html/T_NScrape_Scraper.htm) base class. Once instantiated with the page HTML, our class will have the [HTML Agility Pack](http://html-agility-pack.net/) [HtmlDocument](https://nscrape.com/reference/html/P_NScrape_Scraper_HtmlDocument.htm) property ready for us to use. All we have to do is implement a couple methods to scrape the values from the HTML.  If you'd rather scrape manually, you can reference the [Html](https://nscrape.com/reference/html/P_NScrape_Scraper_Html.htm) property instead.

Scrapers are page-centric; create one for every page you need to scrape. You can implement multiple methods to scrape each bit of data that you're after, or alternately, implement one method and have it return an object containing all of your data. Your choice.

```c#
using NScrape;
using NScrape.Forms;

private class TestScraper : Scraper {
	public TestScraper( string html ) : base( html ) {
	}

	public string GetConditions() {
		var node = HtmlDocument.DocumentNode.Descendants().SingleOrDefault( n => n.Attributes.Contains( "class" ) && n.Attributes["class"].Value == "myforecast-current" );

		if ( node != null ) {
			return node.InnerText;
		}

		throw new ScrapeException( "Could not scrape conditions.", Html );
	}

	public string GetTemperature() {
		var node = HtmlDocument.DocumentNode.Descendants().SingleOrDefault( n => n.Attributes.Contains( "class" ) && n.Attributes["class"].Value == "myforecast-current-lrg" );

		if ( node != null ) {
			return node.InnerText.Replace( "&deg;", "°" );
		}

		throw new ScrapeException( "Could not scrape temperature.", Html );
	}
}
```
### Get the HTML and Scrape It
Now that we have a scraper, we need to feed it some HTML. NScrape provides the [WebClient](https://nscrape.com/reference/html/T_NScrape_WebClient.htm) class, which can be used to download HTML at a given URL. If the page you want to scrape is accessible in that way, [WebClient](https://nscrape.com/reference/html/T_NScrape_WebClient.htm) is your ticket.

In this example, though, we need to populate and submit an HTML form to get the HTML that we want to scrape. For this, NScrape provides the [BasicHtmlForm](https://nscrape.com/reference/html/T_NScrape_Forms_BasicHtmlForm.htm) class, which makes this job easy.

First, instantiate a [WebClient](https://nscrape.com/reference/html/T_NScrape_WebClient.htm) and use it to instantiate a [BasicHtmlForm](https://nscrape.com/reference/html/T_NScrape_Forms_BasicHtmlForm.htm). Tell our form object to load the US National Weather Service page. This page contains multiple forms, so we specify that we want the HTML form identified by the attributes *name=getForecast*.

Our form object will now load the specified HTML form and parse out the HTML controls it contains. We can access these controls via collections exposed by [BasicHtmlForm](https://nscrape.com/reference/html/T_NScrape_Forms_BasicHtmlForm.htm)  and populate them as needed. 

In this case, we need to provide the location where we want the weather; let's get the weather for [Fairbanks, Alaska](https://www.google.com/maps/place/Fairbanks,+AK/@64.8283644,-147.6690026,12z/data=!3m1!4b1!4m2!3m1!1s0x5132454f67fd65a9:0xb3d805e009fef73a). We use Linq to get the control we need (*name=inputstring*) and set it accordingly.

Next, we submit the form and get the response, which we expect to be a chunk of HTML.

Finally, we instantiate our scraper with the HTML from the response, and call its methods to scrape the values we need!

```c#
var webClient = new WebClient();

var form = new BasicHtmlForm( webClient );
form.Load( new Uri( "http://www.weather.gov/" ), new KeyValuePair<string, string>( "name", "getForecast" ) );
form.InputControls.Single( c => c.Name == "inputstring" ).Value = "fairbanks, ak";

using ( var response = form.Submit() ) {
	if ( response.ResponseType == WebResponseType.Html ) {
		var scraper = new TestScraper( ( ( HtmlWebResponse )response ).Html );

		var conditions = scraper.GetConditions();

		var temperature = scraper.GetTemperature();
	}
}
```



