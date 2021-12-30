global using System;

using ChatApp.API.Configurations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// TODO: remove kestrel server header
//builder.Host.ConfigureWebHost(
//	host =>
//	{
//		host.ConfigureKestrel(o =>
//		{
//			o.AddServerHeader = false;
//			//o.ConfigureEndpointDefaults(listenOptions =>
//			//{
//			//	listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
//			//});
//			//o.ConfigureHttpsDefaults(listenOptions =>
//			//{
//			//	listenOptions.SslProtocols = SslProtocols.Tls13 | SslProtocols.Tls12;
//			//	listenOptions.OnAuthenticate = (context, sslOptions) =>
//			//	{
//			//		sslOptions.EnabledSslProtocols = SslProtocols.Tls13 | SslProtocols.Tls12;
//			//		if (sslOptions.ApplicationProtocols != null)
//			//		{
//			//			sslOptions.ApplicationProtocols.Clear();
//			//		}
//			//		else
//			//		{
//			//			sslOptions.ApplicationProtocols = new();
//			//		}

//			//		sslOptions.ApplicationProtocols.Add(SslApplicationProtocol.Http2);
//			//		sslOptions.ApplicationProtocols.Add(SslApplicationProtocol.Http11);
//			//	};
//			//});
//		});
//	});

// Add services to the container.
builder.Services.AddControllersWithViews();
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
builder.Services.AddDatabase(builder.Configuration, builder.Environment.IsDevelopment());

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

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
