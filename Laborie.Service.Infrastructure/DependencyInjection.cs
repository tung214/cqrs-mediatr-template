using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MongoDB.Driver;
using Laborie.Service.Infrastructure.Configurations;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization.Conventions;
using Laborie.Service.Infrastructure.Services;
using Laborie.Service.Application.Services;
using Laborie.Service.Domain.Repositories.Mongo;
using Laborie.Service.Infrastructure.Repositories.Mongo;

namespace Laborie.Service.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDistributeCache(configuration);
        services.AddMemoryCache();
        // services.AddResponseCacheService(configuration);

        services.AddMongo(configuration);

        services.AddCognitoAuthentication(configuration);

        services.AddConfigCORS(configuration);

        services.AddDependencyInjection();

        return services;
    }

    public static IServiceCollection AddDistributeCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
      {
          options.Configuration = configuration["ConnectionStrings:Redis"];
      });
        return services;
    }

    public static IServiceCollection AddCognitoAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // var client = new HttpClient();
        // var json = client.GetStringAsync(configuration["Authentication:Issuer"] + "/.well-known/jwks.json").Result;
        // var keys = JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
        // var securityKey = (IEnumerable<SecurityKey>)keys;
        var secretKey = configuration["Authentication:SecretKey"];
        services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
#pragma warning disable CS8604 // Possible null reference argument.

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "your_issuer",
                ValidAudience = "your_audience",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey))
                // IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                //     {
                //         return securityKey;
                //     },                
            };
#pragma warning restore CS8604 // Possible null reference argument.

        });

        services.AddAuthorization();

        return services;
    }

    public static void AddConfigCORS(this IServiceCollection services, IConfiguration configuration)
    {
        string ProductionCORS = "productionCORS";
        string DevelopmentCORS = "developmentCORS";

        var originAllowAccess = configuration["OriginAllowAccess:Url"]?.Split(";");

        services.AddCors(option =>
        {
            option.AddPolicy(ProductionCORS,
                builder => builder
                .WithOrigins(originAllowAccess ?? ["*"])
                .AllowAnyMethod()
                .AllowAnyHeader());
        });
        services.AddCors(option =>
        {
            option.AddPolicy(DevelopmentCORS,
                builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
        });
    }

    public static IServiceCollection AddMongo(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Contexts.MongoDbContext>(context =>
        {
            context.Databases.AddMongoDatabase(configuration, "Laborie");
            context.Databases.AddMongoDatabase(configuration, "Shop");
        });

        services.AddTransient(typeof(Domain.Repositories.Mongo.IMongoRepository<>), typeof(Repositories.Mongo.MongoRepository<>));

        // Thiết lập tùy chỉnh global cho DateTime serialization
        BsonSerializer.RegisterSerializer(typeof(DateTime), new DateTimeSerializer(DateTimeKind.Local));
        // Thiết lập tùy chỉnh global cho IgnoreExtraElements
        var conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
        ConventionRegistry.Register("IgnoreExtraElements", conventionPack, type => true);

        return services;
    }

    public static void AddMongoDatabase(this Dictionary<string, IMongoDatabase> databases, IConfiguration configuration, string configName)
    {
        var dbOptions = configuration.GetOptions<MongoDatabaseConfig>($"MongoDBSettings:{configName}");
        var mongoDb = new MongoClient(dbOptions.ConnectionString)
                .GetDatabase(dbOptions.DatabaseName);
        databases.Add(configName, mongoDb);
    }

    public static TOptions GetOptions<TOptions>(this IConfiguration configuration, string sectionName)
           where TOptions : new()
    {
        var options = new TOptions();
        configuration.GetSection(sectionName).Bind(options);
        return options;
    }


    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<ICacheService, CacheService>();    

        services.AddTransient<IAgencyAddressRepository, AgencyAddressRepository>();    

        return services;
    }
}
