using Blazored.LocalStorage;
using BlazorStrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.ResponseCompression;
using Middleware.Server.Blazor.Configuration;
using Middleware.Server.Blazor.Hubs;
using Middleware.Server.Blazor.Providers;
using Middleware.Server.Blazor.Services;
using Middleware.Server.Blazor.Services.Authentication;
using Middleware.Server.Blazor.Services.Base;
using Middleware.Server.Blazor.Services.Connection;
using Middleware.Server.Blazor.Services.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//builder.Services.AddHttpClient<IClient, Client>(cl => cl.BaseAddress = new Uri("https://localhost:5000"));
builder.Services.AddHttpClient<IClient, Client>(cl => cl.BaseAddress = new Uri("https://middlewarethesisoisp.azurewebsites.net"));
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ApiAuthenticationStateProvider>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri("https://middlewarethesisoisp.azurewebsites.net/api/") });
builder.Services.AddScoped<IActuatorHttpRepository, ActuatorHttpRepository>();
builder.Services.AddScoped<INodeService, NodeService>();
builder.Services.AddScoped<IConnectionService, ConnecitonService>();
builder.Services.AddAutoMapper(typeof(MapperConfig));
builder.Services.AddBlazorStrap();
//ScadaHub using SignalR

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

builder.Services.AddScoped<IAuthenticationServices, AuthenticationService>();
builder.Services.AddScoped<AuthenticationStateProvider>(p =>
p.GetRequiredService<ApiAuthenticationStateProvider>());

var app = builder.Build();

app.UseResponseCompression();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();

app.MapHub<ScadaHub>("/chathub");

app.MapFallbackToPage("/_Host");

app.Run();
