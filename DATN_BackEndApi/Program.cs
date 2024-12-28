using CloudinaryDotNet;
using DATN_BackEndApi.Extension.CloudinarySett;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Module;
using DATN_Models.DAO;
using DATN_Models.DAO.Interface;
using DATN_Models.HandleData;
using DATN_Models.Mapper;
using DATN_Models.Models;
using DATN_Services.Orders;
using DATN_Services.Orders.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace DATN_BackEndApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container..

            var _services = builder.Services;
            var _configuration = builder.Configuration;
            _services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            _services.AddEndpointsApiExplorer();
            _services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1.0.0",
                    Title = "Streamie",
                    Description = "Streamie api documents"
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
            _services.AddDistributedMemoryCache();
            _services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            _services.AddAutoMapper(typeof(MapperProfile));



            _services.AddTransient<ILoginDAO, LoginDAO>();

            _services.AddTransient<IMovieDAO, MovieDAO>();
            _services.AddTransient<IActorDAO, ActorDAO>();
            _services.AddTransient<IRoomDAO, RoomDAO>();
            _services.AddTransient<ISeatDAO, SeatDAO>();
            _services.AddTransient<ISeatTypeDAO, SeatTypeDAO>();
            _services.AddTransient<IPricingRuleDAO, PricingRuleDAO>();
            _services.AddTransient<ICommentDAO, CommentDAO>();
            _services.AddTransient<IOrderDAO, OrderDAO>();
            _services.AddTransient<IRatingDAO, RatingDAO>();
            _services.AddTransient<IOrderService, OrderService>();

            _services.AddTransient<ICustomerDAO, CustomerDAO>();
            _services.AddScoped<IUltil, Ultil>();




            #region Nghia_Cloudinary(Ảnh/Video)
            // Cấu hình dv lưu trữ ảnh đám mây (Cloudinary)
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
            builder.Services.AddSingleton<Cloudinary>(serviceProvider =>
            {
                var config = serviceProvider.GetService<IOptions<CloudinarySettings>>().Value;
                var account = new Account(config.CloudName, config.ApiKey, config.ApiSecret);
                return new Cloudinary(account); // Cái này là account của nghĩa.
            });
            builder.Services.AddScoped<ImageService>();


            #endregion





            _services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
            _services.AddHttpContextAccessor();
            _services.AddScoped<UserManager<AppUsers>, UserManager<AppUsers>>();
            _services.AddTransient<UserManager<AppUsers>, UserManager<AppUsers>>();
            _services.AddTransient<RoleManager<AppRoles>, RoleManager<AppRoles>>();
            _services.AddIdentity<AppUsers, AppRoles>()
                 .AddEntityFrameworkStores<DATN_Context>()
                 .AddDefaultTokenProviders();
            _services.AddDbContext<DATN_Context>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Db")));
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
