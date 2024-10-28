using AspNetCoreRateLimit;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddInMemoryRateLimiting();
// Load in general configuration from appsettings.json
builder.Services.Configure<IpRateLimitOptions>(options => builder.Configuration.GetSection("IpRateLimitingSettings").Bind(options));
builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseIpRateLimiting();
// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
