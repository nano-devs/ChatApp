# ChatApp.Api

ChatApp.Api uses Kestrel as the web server. To run the app you need CA or .pem.

Use [mkcert](https://github.com/FiloSottile/mkcert) to create local CA or .pem before running the server. Then put the `*.pem` and `*-key.pem` outside the project folder

```
ChatApp
	- ChatApp.Api
		- Program.cs
		- ...
	- localhost.pem
	- localhost-key.pem
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
