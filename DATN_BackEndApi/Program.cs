
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace DATN_BackEndApi
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
            builder.Services.AddSwaggerGen();
            builder.Services.AddLocalization(options => options.ResourcesPath = "DATN_Helpers/ResourceFiles");
            var app = builder.Build();

            var supportedCultures = new[] { "en", "vi" };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("vi"),
                SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToArray(),
                SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToArray()
            });
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
