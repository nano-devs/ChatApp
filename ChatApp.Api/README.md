# ChatApp.Api

Chat App Api using Kestrel as the web server. To run the app you need CA or .pem.

Use [mkcert](https://github.com/FiloSottile/mkcert) to create local CA or .pem before running the server. Then put the `*.pem` and `*-key.pem` outside the project folder

```
ChatApp
	- ChatApp.Api
		- Program.cs
		- ...
	- localhost.pem
	- localhost-key.pem
```

Since The .NET Core SDK includes an HTTPS development certificate and installed as part of the first-run experience. Alternatively (if you already had cert configured, signed, trusted, & installed from the templates in Visual Studio or from the dotnet new command), you can update the following settings in `appsettings.json`:

```json
  "Kestrel": {
    "Endpoints": {
      "Http": {
        "Url": "http://localhost:5000"
      },
      "Https": {
        "Url": "https://localhost:8000"
      }
    }
  }
```

## Apply Migration
```sh
PM> Update-Database -context ChatAppDbContext
```

in case of `Could not load assembly 'ChatApp.Auth'. Ensure it is referenced by the startup project 'ChatApp.Api'.`. Add project reference:
```xml
<ItemGroup>
  <ProjectReference Include="..\ChatApp.Auth\ChatApp.Auth.csproj" />
</ItemGroup>
```
