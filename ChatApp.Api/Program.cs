global using System;

using ChatApp.Api.Data;
using ChatApp.Api.Models;
using ChatApp.Api.Models.Configurations;
using ChatApp.Api.Services.Authenticators;
using ChatApp.Api.Services.Repositories;
using ChatApp.Api.Services.Repositories.RefreshTokenRepositories;
using ChatApp.Api.Services.TokenGenerators;
using ChatApp.Api.Services.TokenValidators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// TODO: configure tls/ssl and http protocol
builder.WebHost.ConfigureKestrel(o =>
{
	o.AddServerHeader = false;
	//o.ConfigureEndpointDefaults(listenOptions =>
	//{
	//	listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
	//});
	//o.ConfigureHttpsDefaults(listenOptions =>
	//{
	//	listenOptions.SslProtocols = SslProtocols.Tls13 | SslProtocols.Tls12;
	//	listenOptions.OnAuthenticate = (context, sslOptions) =>
	//	{
	//		sslOptions.EnabledSslProtocols = SslProtocols.Tls13 | SslProtocols.Tls12;
	//		if (sslOptions.ApplicationProtocols != null)
	//		{
	//			sslOptions.ApplicationProtocols.Clear();
	//		}
	//		else
	//		{
	//			sslOptions.ApplicationProtocols = new();
	//		}

	//		sslOptions.ApplicationProtocols.Add(SslApplicationProtocol.Http2);
	//		sslOptions.ApplicationProtocols.Add(SslApplicationProtocol.Http11);
	//	};
	//});
});

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
builder.Services.AddScoped<IFriendsRepository, FriendsRepository>();
builder.Services.AddScoped<GroupsRepository>();
builder.Services.AddScoped<IGroupMembersRepository, GroupMembersRepository>();

builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "ChatApp.API", Version = "v1" });
});
builder.Services.AddRouting(options =>
{
	options.LowercaseUrls = true;
	options.LowercaseQueryStrings = true;
});
builder.Services.Configure<JsonOptions>(options =>
{
	if (builder.Environment.IsDevelopment())
	{
		options.JsonSerializerOptions.WriteIndented = true;
	}
	else
	{
		options.JsonSerializerOptions.WriteIndented = false;
	}
	options.JsonSerializerOptions.AllowTrailingCommas = true;
});
builder.Services.AddDbContext<ChatAppDbContext>(options =>
{
	if (builder.Environment.IsDevelopment())
	{
		options.UseSqlite(
				builder.Configuration.GetConnectionString("DataSQLiteConnection"));
	}
	else
	{
		//options.UseNpgsql(
		//	builder.Configuration.GetConnectionString("DataPostgreConnection"),
		//	options => { options.EnableRetryOnFailure(5, TimeSpan.FromSeconds(30), null); });
	}
});

#region Auth
builder.Services.AddIdentityCore<User>(option =>
{
	option.User.RequireUniqueEmail = true;
	// For testing only, remove this for production
	option.Password.RequireDigit = false;
	option.Password.RequireNonAlphanumeric = false;
	option.Password.RequireUppercase = false;
	option.Password.RequiredLength = 0;
}).AddEntityFrameworkStores<ChatAppDbContext>();
builder.Services.AddSingleton<IdentityErrorDescriber>();
builder.Services.AddScoped<IRefreshTokenRepository, DatabaseRefreshTokenRepository>();
builder.Services.AddScoped<Authenticator>();
builder.Services.AddSingleton<AccessTokenGenerator>();
builder.Services.AddSingleton<RefreshTokenGenerator>();
builder.Services.AddSingleton<ITokenValidator, RefreshTokenValidator>();
AuthenticationConfiguration authConfig = new AuthenticationConfiguration();
builder.Configuration.Bind("Authentication", authConfig);
builder.Services.AddSingleton(authConfig);
var refreshTokenValidationParameters = new TokenValidationParameters()
{
	IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.RefreshTokenSecret!)),
	ValidIssuer = authConfig.Issuer,
	ValidAudience = authConfig.Audience,
	ValidateIssuerSigningKey = true,
	ValidateIssuer = true,
	ValidateAudience = true,
	ClockSkew = TimeSpan.Zero,
};
builder.Services.AddSingleton(refreshTokenValidationParameters);
builder.Services.AddSingleton<JwtSecurityTokenHandler>();
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
		ClockSkew = TimeSpan.Zero,
	}; ;
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
else
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ChatApp.API v1"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=swagger}/{action=Index}/{id?}");

app.Run();
