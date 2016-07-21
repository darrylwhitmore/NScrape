Here we store 3rd party packages that are unsigned as-is. Since NScrape is signed, all our references
must also be signed. We use .NET Assembly Strong-Name Signer (https://www.nuget.org/packages/Brutal.Dev.StrongNameSigner/)
to sign these assemblies, and store them here.

See also: https://brutaldev.com/post/net-assembly-strong-name-signer

Example command executed in Visual Studio Package Manager Console:

	strongnamesigner.console.exe -a ".\packagesSigned\Sprache.2.0.0.52\lib\portable-net4+netcore45+win8+wp8+sl5+MonoAndroid+Xamarin.iOS10+MonoTouch\Sprache.dll" -k .\NScrape\nscrape.snk
