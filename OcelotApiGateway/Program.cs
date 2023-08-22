using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;
using OcelotApiGateway.Aggregators;
using OcelotApiGateway.Extensions;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.WebHost.ConfigureAppConfiguration((hostingContext, config) =>
{
    config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath);
    config.AddOcelotConfigs("OcelotConfigs");
});


builder.Services
    .AddOcelot(builder.Configuration)
    .AddSingletonDefinedAggregator<ArticlesWriterAggregator>()
    //.AddCacheManager(x =>
    // {
    //     x.WithRedisConfiguration("redis",
    //             config =>
    //             {
    //                 config.WithAllowAdmin()
    //                 .WithDatabase(0)
    //                 .WithEndpoint("localhost", 6379);
    //             })
    //     .WithJsonSerializer()
    //     .WithRedisCacheHandle("redis");
    // });
    ;
builder.Services.AddAuthentication(options =>
 {
     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
     options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
 })
// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    var secretkey = Encoding.UTF8.GetBytes("LongerThan-16Char-SecretKey");
    var encryptionkey = Encoding.UTF8.GetBytes("16CharEncryptKey");

    var validationParameters = new TokenValidationParameters
    {
        ClockSkew = TimeSpan.Zero,
        RequireSignedTokens = true,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretkey),

        RequireExpirationTime = true,
        ValidateLifetime = true,


        TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey)
    };

    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = validationParameters;
});
var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

await app.UseOcelot();

app.UseAuthentication();
app.UseAuthorization();
app.Run();