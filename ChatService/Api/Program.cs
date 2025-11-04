using System.Text;
using Core.Api;
using Core.Api.HttpService;
using Core.Api.Interfaces;
using Core.Configuration;
using Core.Configuration.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProfileConnectionLib;
using ProfileConnectionLib.ConnectionServices.Interfaces;
using Scalar.AspNetCore;

// Load Config
EnvConfigLoader.Load<EnvConfigurationWithAuth>();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpRequestService();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IConfigurationWithAuth, EnvConfigurationWithAuth>();
builder.Services.AddTransient<IIdentityConnectionService, IdentityConnectionService>();
builder.Services.AddTransient<ICurrentUser, CurrentUser>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        using var sp = builder.Services.BuildServiceProvider();
        var config = sp.GetRequiredService<IConfigurationWithAuth>();
        var issuer = config.Auth.Issuer;
        var audience = config.Auth.Audience;
        var key = config.Auth.Key;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapHealthChecks("/health");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();