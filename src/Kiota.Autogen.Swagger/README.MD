[Kiota.Autogen.Swagger](https://github.com/ellizio/Kiota.Autogen) is a package for auto-generating API clients based on [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) using [Kiota](https://github.com/microsoft/kiota)

---

## Basic Usage

1. Add a `Class Library` project to the solution that contains WebApi project
2. Add WebApi project reference with `ExcludeAssets="All"`
```xml
<ItemGroup>
    <ProjectReference Include="..\ExampleService.Api\ExampleService.Api.csproj" ExcludeAssets="All" />
</ItemGroup>
```
3. Install `Kiota.Autogen.Swagger` with `PrivateAssets="All"`
```xml
<ItemGroup>
    <PackageReference Include="Kiota.Autogen.Swagger" Version="1.16.1" PrivateAssets="All" />
</ItemGroup>
```
4. Install following `Kiota` packages
```xml
<ItemGroup>
    <PackageReference Include="Microsoft.Kiota.Abstractions" Version="1.9.11" />
    <PackageReference Include="Microsoft.Kiota.Http.HttpClientLibrary" Version="1.9.11" />
    <PackageReference Include="Microsoft.Kiota.Serialization.Form" Version="1.9.11" />
    <PackageReference Include="Microsoft.Kiota.Serialization.Json" Version="1.9.11" />
    <PackageReference Include="Microsoft.Kiota.Serialization.Multipart" Version="1.9.11" />
    <PackageReference Include="Microsoft.Kiota.Serialization.Text" Version="1.9.11" />
</ItemGroup>
```
5. Create a `gensettings.json` file with the following structure
```jsonc
[
  {
    "name": "WeatherClient", // API client name to be generated
    "namespace": "Weather.Client", // API client namespace
    "version": "v1" // WebApi Swagger document version
  },
  {
    "name": "WeatherClientNew",
    "namespace": "Weather.Client.New",
    "version": "v2"
  }
]
```
6. Set up `Class library` project to build as a nuget package
```xml
<PropertyGroup>
    ... other properties

    <IsPackable>true</IsPackable>
    <PackageId>ExampleService.Client</PackageId>
</PropertyGroup>
```
7. Pack `Class library` project
8. Enjoy using API client
```csharp
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Weather.Client;

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://your_service_url");

var provider = new AnonymousAuthenticationProvider();
using var adapter = new HttpClientRequestAdapter(provider, httpClient: httpClient);
var client = new WeatherClient(adapter);

var forecasts = await client.Weatherforecast.GetAsync();
```

See [full example](https://github.com/ellizio/Kiota.Autogen-Examples/tree/master/1.%20Swagger)

## Advanced Usage

You can provide your own implementations of `Microsoft.Kiota.Abstractions` from the `Class library` project if you need\
Usage:
```csharp
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Weather.Client;

using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("http://your_service_url");

var provider = new AnonymousAuthenticationProvider();
using var adapter = new HttpClientRequestAdapter(provider, new WeatherParseNodeFactory(), new WeatherSerializationWriterFactory(), httpClient: httpClient);
var client = new WeatherClient(adapter);

var forecasts = await client.Weatherforecast.GetAsync();
```

See [full example](https://github.com/ellizio/Kiota.Autogen-Examples/tree/master/2.%20Advanced)

## More Examples

See more examples [here](https://github.com/ellizio/Kiota.Autogen-Examples)
