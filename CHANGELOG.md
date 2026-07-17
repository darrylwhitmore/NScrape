# Changelog

## [1.0.0] - July 2026

### Breaking Changes

- **`ImageWebResponse` removed** — image responses are now handled by `BinaryWebResponse`. Use `GetStream()` to access the image data.
- **`WebResponseFactory` redesigned** — was a static class with static methods and a `SupportedContentTypes` dictionary; now an instantiable class implementing `IWebResponseFactory`, injectable into `WebClient`. Custom content types are now provided via `IContentTypeHandler` implementations passed to the `WebResponseFactory` constructor.
- **`HtmlForm.Load()` signature changed** — the `KeyValuePair<string, string>` identifying attribute parameter replaced with two explicit `string attributeName, string attributeValue` parameters.
- **`WebRequest.IsXmlHttpRequest` removed** — add the `X-Requested-With` header manually via the `Headers` collection if needed.
- **`PlaintextWebResponse` renamed to `PlainTextWebResponse`** (capital T).
- **Namespaces reorganized:**
  - Request classes (`WebRequest`, `GetWebRequest`, `PostWebRequest`, `WebRequestType`) moved to `NScrape.Requests`
  - Response classes moved to `NScrape.Responses`
  - Interfaces moved to `NScrape.Interfaces`
- **Target framework updated** — now targets .NET 8.0 (previously .NET Standard 2.0).

### New Features

- **Proxy support** — `IWebProxy Proxy` property added to both `WebClient` and `WebRequest`. Request-level proxy takes precedence over client-level; if neither is set, system default proxy settings are preserved.
- **Dependency injection support** — `WebClient` now accepts `IWebResponseFactory` via constructor.
- **Extensible content type handling** — implement `IContentTypeHandler` to handle additional or override existing content types.
- **Expanded default content type support** — added `audio/`, `video/`, `font/`, and numerous `application/*` binary types; added `application/ld+json`, `application/atom+xml`, `application/rss+xml`, `application/xhtml+xml`.

### Improvements

- Simplified disposal pattern across response hierarchy — single `DisposeResources()` virtual method replaces the previous managed/unmanaged split with finalizer.
- Removed `System.Drawing.Common` dependency for cross-platform compatibility.
- Modernized codebase — file-scoped namespaces, expression-bodied members, `ArgumentNullException.ThrowIfNull()`, target-typed `new()`.

---

## [0.4.1] - March 2019

Initial public release.
