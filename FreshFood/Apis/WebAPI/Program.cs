
using Asp.Versioning;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Models.Customer;
using SecurityAPI.Interfaces;
using SecurityAPI.Swagger;
using SecurityAPI.Utils;
using SecurityAPI.Validators;
using System.Reflection;
using System.Text;
using Database.Repositories.Interfaces;
using Database.Repositories;
using Database.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using Database.DbContexts;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using SecurityService.Interfaces;
using CustomerService.Interfaces;
using CacheService.Interfaces;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                .Build();

            #region DbContext

            builder.Services.AddDbContext<FreshFoodContext>(options =>
                 options.UseSqlServer(builder.Configuration.GetConnectionString("FreshFoodDatabase")));

            #endregion

            #region AutoMapper            
            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapProfiles.MapProfiles());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);
            #endregion

            #region Services Registration

            builder.Services.AddScoped<IJwtUtils, JwtUtils>();
            builder.Services.AddScoped<ISecurityService, SecurityService.Services.SecurityService>();
            builder.Services.AddScoped<ICustomerService, CustomerService.Services.CustomerService>();
            builder.Services.AddScoped<ICacheService, CacheService.Services.CacheService>();


            builder.Services.AddScoped(typeof(ISecurityRepository), typeof(SecurityRepository));
            builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));

            builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();

            #endregion

            #region JwtTokens
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opt =>
            {
                opt.SaveToken = true;
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    // Validate the server that generates the token
                    ValidateIssuer = true,

                    // Validate the recipient of the token is authorized to receive.
                    ValidateAudience = true,

                    // Check if the token is not expired and the signing key of the issuer is valid
                    ValidateLifetime = true,

                    // Validate signature of the token
                    ValidateIssuerSigningKey = true,

                    // Stored in appsettings.json
                    ValidIssuer = builder.Configuration["JwtAuth:Issuer"],

                    // Stored in appsettings.json
                    ValidAudience = builder.Configuration["JwtAuth:Audience"],

                    // Stored in appsettings.json
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtAuth:Key"]))
                };
            });

            #endregion

            #region Swagger 

            builder.Services.AddSwaggerGen(option =>
            {
                //option.SwaggerDoc("v1", new OpenApiInfo { Title = "Security API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            builder.Services.AddApiVersioning(setup =>
            {
                //indicating whether a default version is assumed when a client does
                // does not provide an API version.
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.ReportApiVersions = true;
                setup.DefaultApiVersion = new ApiVersion(1.0);
            }).AddApiExplorer(options =>
            {

                // Add the versioned API explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });

            //builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            builder.Services.AddSwaggerGen(options =>
            {
                // Add a custom operation filter which sets default values
                options.OperationFilter<SwaggerDefaultValues>();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
            #endregion          

            ConfigureLogging(configuration);
            builder.Host.UseSerilog();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.ConfigObject.AdditionalItems.Add("syntaxHighlight", false);
                    var descriptions = app.DescribeApiVersions();

                    // Build a swagger endpoint for each discovered API version
                    foreach (var description in descriptions)
                    {
                        var url = $"/swagger/{description.GroupName}/swagger.json";
                        var name = description.GroupName.ToUpperInvariant();
                        options.SwaggerEndpoint(url, name);
                    }
                });
            }

            // Verifies if any HTTP request contains a Jwt token
            //app.UseMiddleware<JwtMiddleware>();

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000"));

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void ConfigureLogging(IConfigurationRoot config)
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                //.Enrich.WithExceptionDetiails()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.Elasticsearch()
                .WriteTo.Elasticsearch(ConfigureElasticSink(config, Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
                .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
                .ReadFrom.Configuration(config)
                .CreateLogger();
        }

        public static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot config, string environment)
        {
            return new ElasticsearchSinkOptions(new Uri(config["ElasticConfiguration:Uri"]))
            {
                AutoRegisterTemplate = true,
                //AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM-dd HH-mm-ss}"
            };
        }
    }
}