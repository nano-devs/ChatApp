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

### ASP.NET CORE 6:
Required dependencies:
- Microsoft.AspNetCore.Authentication.JwtBearer

in **Program.cs**
```cs
AuthenticationConfiguration authConfig = new AuthenticationConfiguration();
builder.Configuration.Bind("Authentication", authConfig);
builder.Services.AddSingleton(authConfig);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
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

To add authorization to controllers, simply add `[Authorize]` to the controller class or method:

```cs
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class PrivateChatController : ControllerBase {}

// or
public class PrivateChatController : ControllerBase 
{
  [Authorize]
  [HttpPost]
  public async Task<object> AddChat(Guid userId, Guid friendId, string message)
}
```
