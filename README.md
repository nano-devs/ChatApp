# ChatApp
Minimum chat app project made with C# using ASP.NET Core

## Using the JWT Authentication Authorization from Other Backend App
- Add "Authentication" settings to `appsettings.json`:
```json
{
  ...
  "Authentication": {
    "AccessTokenSecret": "9GHdZCAJ2XaXFuhOhIt21zxJCWk7obnzcHqDB4t7X0WcvrB8bzvkyEFlIMRXO4o-y3eQs8e4uDiFJcAhnFOiE6I45aJQi22DEy5epVLyQIVFYI-dbumj8ieK1sKMPySfN9S4eliQznJYL82XhtI_8U1EvEL2_C7PX4rTR0Xjf8k",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenSecret": "8GHdZCAJ2XaXFuhOhIt21zxJCWk7obnzcHqDB4t7X0WcvrB8bzvkyEFlIMRXO4o-y3eQs8e4uDiFJcAhnFOiE6I45aJQi22DEy5epVLyQIVFYI-dbumj8ieK1sKMPySfN9S4eliQznJYL82XhtI_8U1EvEL2_C7PX4rTR0Xjf8k",
    "RefreshTokenExpirationMinutes": 6000,
    "Audience": "https://localhost:5001",
    "Issuer": "https://localhost:5001"
  },
  ...
}
```
Make sure `AccessTokenSecret` & `RefreshTokenSecret` matches with the one from the JWT Authentication app backend, get the value [here](https://github.com/nano-devs/ChatApp/blob/main/NET5AuthServerAPI/appsettings.json). This is required because the JWT backend uses the `SymmetricSecurityKey`, which means the security key used to sign jwt is gonna be the same as the key used to verify jwt.

- Create a class to load and bind json values from the appsettings, for example:
```cs
public class AuthenticationConfiguration
{
    public string AccessTokenSecret { get; set; }
    public int AccessTokenExpirationMinutes { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string RefreshTokenSecret { get; set; }
    public int RefreshTokenExpirationMinutes { get; set; }
}
```

### ASP.NET CORE 5
in **Startup.cs** class
```cs
public void ConfigureServices(IServiceCollection services)
{
  ...
  ...
  
  // Load Authentication values from appsettings
  AuthenticationConfiguration authConfig = new AuthenticationConfiguration();
  configuration.Bind("Authentication", authConfig);
  services.AddSingleton(authConfig);
  
  // Add & Configure Authentication
  services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
  {
    option.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.AccessTokenSecret)),
        ValidIssuer = authConfig.Issuer,
        ValidAudience = authConfig.Audience,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ClockSkew = System.TimeSpan.Zero,
    };
  });
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
  ...
  
  app.UseAuthentication(); // <---
  app.UseAuthorization();  // <---
  
  ...
}
```

### ASP.NET CORE 6:
in **Program.cs**
```cs
app.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
  option.TokenValidationParameters = new TokenValidationParameters()
  {
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.AccessTokenSecret)),
      ValidIssuer = authConfig.Issuer,
      ValidAudience = authConfig.Audience,
      ValidateIssuerSigningKey = true,
      ValidateIssuer = true,
      ValidateAudience = true,
      ClockSkew = System.TimeSpan.Zero,
  };
});

app.UseAuthentication();
app.UseAuthorization();
```

### Routes
#### `/register` 
POST request body:
```json
{
  "Email": "test@gmail.com",
  "Username": "test",
  "Password": "testtest",
  "ConfirmPassword": "testtest"
}
```
Return HttpResponse 200 if username & email is successfuly registered.


#### `/login`
POST request body
```json
{
  "Username": "test",
  "Password": "testtest"
}
```

Return 200 if username & password are registered:
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6ImNjMWQ4MWI4LWU3NDctNGJmZS1hYzA3LTJjOGQwZmFiNDZmMyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InRlc3RAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6InRlc3QiLCJuYmYiOjE2NDA0NDEwODMsImV4cCI6MTY0MDQ0MTk4MywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjUwMDEifQ.K-BUxZO88kVFHu1_GOTOC_FGof5MNU74uTGZUSgR3as",
  "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NDA0NDEwODMsImV4cCI6MTY0MDgwMTA4MywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjUwMDEifQ.zrPeQRI5m9mstxSAbqEA5uKwhL2sSIgAAXZOSdDjFDg"
}
```


#### `/refresh` 
POST request body:
```json
{
  "RefreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NDA0NDEwODMsImV4cCI6MTY0MDgwMTA4MywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjUwMDEifQ.zrPeQRI5m9mstxSAbqEA5uKwhL2sSIgAAXZOSdDjFDg"
}
```

Return body (200) if given RefreshToken is valid:
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6ImNjMWQ4MWI4LWU3NDctNGJmZS1hYzA3LTJjOGQwZmFiNDZmMyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InRlc3RAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6InRlc3QiLCJuYmYiOjE2NDA0NDExMTIsImV4cCI6MTY0MDQ0MjAxMiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjUwMDEifQ.Fw1wEq1QTs6FgHfVzINx5flbmwOyfKGcgKvu7I613DE",
  "refreshToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2NDA0NDExMTIsImV4cCI6MTY0MDgwMTExMiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjUwMDEifQ.B9o7cAJLimT-0QnOrW_-BgtPyV8rnuqjJixVDbcIu-8"
}
```

#### `/logout`
DELETE request header, Add Authorization as key:

```json
{
"Authorization": "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6ImNjMWQ4MWI4LWU3NDctNGJmZS1hYzA3LTJjOGQwZmFiNDZmMyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6InRlc3RAZ21haWwuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZSI6InRlc3QiLCJuYmYiOjE2NDA0NDExMTIsImV4cCI6MTY0MDQ0MjAxMiwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjUwMDEifQ.Fw1wEq1QTs6FgHfVzINx5flbmwOyfKGcgKvu7I613DE"
}
```

Make sure to put "Bearer " as prefix before the access token
