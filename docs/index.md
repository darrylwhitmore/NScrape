# NScrape

A web scraping framework for .NET that handles the plumbing — HTTP requests, cookies, redirects, form submission — so you can focus on extracting data.

## Installation

```
dotnet add package NScrape
```

## Quick Start

This example scrapes the book category list from [books.toscrape.com](https://books.toscrape.com).

### 1. Create a scraper

Inherit from `Scraper` and use the [HTML Agility Pack](https://html-agility-pack.net/) `HtmlDocument` to extract data:

```csharp
using NScrape;

public class BookstoreScraper : Scraper {
    public BookstoreScraper( string html ) : base( html ) { }

    public List<string> GetTitles() {
        return HtmlDocument.DocumentNode
            .Descendants( "article" )
            .Where( n => n.Attributes["class"]?.Value == "product_pod" )
            .Select( n => n.SelectSingleNode( ".//h3/a" ).Attributes["title"].Value )
            .ToList();
    }
}
```

### 2. Fetch the page and scrape it

```csharp
using NScrape;
using NScrape.Interfaces;

var webClient = new WebClient();

using var response = webClient.SendRequest( new Uri( "https://books.toscrape.com" ) );

if ( response is IHtmlWebResponse htmlResponse ) {
    var scraper = new BookstoreScraper( htmlResponse.Html );
    var titles = scraper.GetTitles();
}
```

## Features

- GET and POST requests
- Cookie management
- Automatic redirect handling
- HTML form submission (`BasicHtmlForm`, `BasicAspxForm`)
- Proxy support
- Extensible response handling via custom content type handlers

## Proxy Support

NScrape supports routing requests through a proxy at either the client or per-request level:

```csharp
// Client-level proxy — applies to all requests
webClient.Proxy = new WebProxy( "http://your-proxy:port" );

// Per-request proxy — overrides client proxy for this request
var request = new GetWebRequest( uri ) {
    Proxy = new WebProxy( "http://your-proxy:port" )
};
```

For reliable scraping at scale, a dedicated proxy service such as [ScrapingAnt](https://scrapingant.com/?ref=ztrmzdv) *(affiliate link)* can help manage proxy rotation and avoid blocks.

## Requirements

.NET 8.0 or later
