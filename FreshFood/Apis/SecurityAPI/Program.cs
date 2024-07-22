
using Asp.Versioning;
using AutoMapper;
using Customer.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Models.Customer;
using Security.Interfaces;
using Security.Services;
using Customer.Services;
using SecurityAPI.Interfaces;
using SecurityAPI.Swagger;
using SecurityAPI.Utils;
using SecurityAPI.Validators;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using Database.Repositories.Interfaces;
using Database.Repositories;
using Database.Repositories.Repositories;
using Microsoft.EntityFrameworkCore;
using Database.DbContexts;

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
            builder.Services.AddScoped<ISecurityService, SecurityService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();


            builder.Services.AddScoped(typeof(ISecurityRepository), typeof(SecurityRepository));
            builder.Services.AddScoped(typeof(ICustomerRepository), typeof(CustomerRepository));

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

            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            builder.Services.AddSwaggerGen(options =>
            {
                // Add a custom operation filter which sets default values
                options.OperationFilter<SwaggerDefaultValues>();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
            #endregion

                var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
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
    }
}