using System.Text;
using System.Text.Json.Serialization;
using Core.Api;
using Core.Api.Interfaces;
using Core.Configuration;
using Dal;
using Dal.Sagas;
using Logic;
using Logic.Consumers;
using Logic.Interfaces;
using Logic.Interfaces.Configuration;
using Logic.Semaphore;
using MassTransit;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using StackExchange.Redis;
using EnvConfiguration = Logic.EnvConfiguration;
using RedisDistributedSemaphore = Logic.Semaphore.RedisDistributedSemaphore;

EnvConfigLoader.Load<EnvConfiguration>();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();
builder.Services.AddHttpContextAccessor();


builder.Services.AddMassTransit(x =>
{
    x.AddSagaStateMachine<UserUpdateSaga, UserUpdateSagaState>()
        .EntityFrameworkRepository(r =>
        {
            r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
            r.AddDbContext<DbContext, AppDbContext>((provider, options) =>
            {
                var config = provider.GetRequiredService<IEnvConfiguration>();
                options.UseNpgsql(config.Database.ConnectionString);
            });
        });
    
    x.AddConsumer<UserUpdateCoordinator>();
    x.AddConsumer<ChatWithUserUpdater>();
    x.AddConsumer<UserUpdateFinalizer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        
        cfg.ReceiveEndpoint("user-update-saga", e =>
        {
            e.ConfigureSaga<UserUpdateSagaState>(context);
        });
        
        cfg.ReceiveEndpoint("user-update-coordinator", e =>
        {
            e.ConfigureConsumer<UserUpdateCoordinator>(context);
        });

        cfg.ReceiveEndpoint("chat-with-user-updater", e =>
        {
            e.ConfigureConsumer<ChatWithUserUpdater>(context);
        });
        

        cfg.ReceiveEndpoint("user-update-finalizer", e =>
        {
            e.ConfigureConsumer<UserUpdateFinalizer>(context);
        });

        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.AddMassTransitHostedService(); 
builder.Services.AddSingleton<IEnvConfiguration, EnvConfiguration>();
builder.Services.AddTransient<ICurrentUser, CurrentUser>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IPermissionService, PermissionService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        using var sp = builder.Services.BuildServiceProvider();
        var config = sp.GetRequiredService<IEnvConfiguration>();
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

builder.Services.AddStackExchangeRedisCache(options =>
{
    using var sp = builder.Services.BuildServiceProvider();
    var config = sp.GetRequiredService<IEnvConfiguration>();
    options.Configuration = config.Redis.ConnectionString;
    options.InstanceName = config.Redis.InstanceName;
});

builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
{
    var config = provider.GetRequiredService<IEnvConfiguration>();
    return ConnectionMultiplexer.Connect(config.Redis.ConnectionString);
});

builder.Services.AddTransient<IDistributedSemaphore>(provider =>
{
    var redis = provider.GetRequiredService<IConnectionMultiplexer>();
    var database = redis.GetDatabase();
    return new RedisDistributedSemaphore(database, "my_semaphore", 5);
});

builder.Services.AddTransient<IAuthService, AuthService>();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    using var sp = builder.Services.BuildServiceProvider();
    var config = sp.GetRequiredService<IEnvConfiguration>();
    options.UseNpgsql(config.Database.ConnectionString);
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        await context.Database.MigrateAsync();
        Console.WriteLine("Миграции успешно применены");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Произошла ошибка при применении миграций");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapHealthChecks("/health");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllers();
app.Run();