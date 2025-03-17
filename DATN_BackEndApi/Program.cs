using DATN_BackEndApi.Extension;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.OpenApi.Models;
using Serilog;

namespace DATN_BackEndApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Logger
            var builderLog = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())  // location of the exe file
                                .AddJsonFile("logsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builderLog.Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
            builder.Host.UseSerilog();
            // Add services to the container..

            var _services = builder.Services;
            var _configuration = builder.Configuration;
            var MyAllowSpecificOrigins = "_myAllowOrigins";
            _services.AddCoreExtention();
            _services.AddDatabase(_configuration);
            _services.AddServiceContext(_configuration);
            _services.AddCORS(MyAllowSpecificOrigins);
            _services.AddCoreService();
            _services.AddMemoryCache();

            _services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState
                        .Where(ms => ms.Value.Errors.Count > 0)
                        .ToDictionary(
                            ms => ms.Key,
                            ms => ms.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );
                    throw new ValidationException(
                        errors.SelectMany(kv => kv.Value.Select(error => new ValidationFailure(kv.Key, error)))
                    );
                };
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            _services.AddEndpointsApiExplorer();
            _services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = "Cinema",
                    Description = "Cinema api documents"
                });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token!",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                //options.OperationFilter<FileUploadOperationFilter>();
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseCors(MyAllowSpecificOrigins);
            app.UseStaticFiles();
            app.UseWebSockets(new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(30),
            });
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.MapControllers();

            app.Run();
        }
    }
}
