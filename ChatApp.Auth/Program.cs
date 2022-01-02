using ChatApp.Auth.Models;
using ChatApp.Auth.Models.Configurations;
using ChatApp.Auth.Services.Authenticators;
using ChatApp.Auth.Services.RefreshTokenRepositories;
using ChatApp.Auth.Services.TokenGenerators;
using ChatApp.Auth.Services.TokenValidators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddIdentityCore<User>(option =>
{
    option.User.RequireUniqueEmail = true;

    // For testing only, remove this for production
    option.Password.RequireDigit = false;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireUppercase = false;
    option.Password.RequiredLength = 0;
}).AddEntityFrameworkStores<AuthenticationDbContext>();

builder.Services.AddSingleton<IdentityErrorDescriber>();

string connectionString = builder.Configuration.GetConnectionString("sqlite");
builder.Services.AddDbContext<AuthenticationDbContext>(options =>
{
    options.UseSqlite(connectionString);
});

builder.Services.AddScoped<IRefreshTokenRepository, DatabaseRefreshTokenRepository>();
builder.Services.AddScoped<Authenticator>();
builder.Services.AddSingleton<AccessTokenGenerator>();
builder.Services.AddSingleton<RefreshTokenGenerator>();
builder.Services.AddSingleton<ITokenValidator, RefreshTokenValidator>();

AuthenticationConfiguration authConfig = new AuthenticationConfiguration();
builder.Configuration.Bind("Authentication", authConfig);
builder.Services.AddSingleton(authConfig);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.AccessTokenSecret!)),
        ValidIssuer = authConfig.Issuer,
        ValidAudience = authConfig.Audience,
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ClockSkew = System.TimeSpan.Zero,
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
