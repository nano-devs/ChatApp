global using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

using NET6ChatAppServerApi.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "NET6ChatAppServerAPI", Version = "v1" });
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
	app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NET6ChatAppServerAPI v1"));
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
