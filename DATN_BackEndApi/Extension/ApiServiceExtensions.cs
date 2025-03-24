using CloudinaryDotNet;
using DATN_BackEndApi.Extension.CloudinarySett;
using DATN_BackEndApi.Extension.Vnpay;
using DATN_Helpers.Common;
using DATN_Helpers.Common.interfaces;
using DATN_Helpers.Extensions;
using DATN_Helpers.Module;
using DATN_Models.DAO;
using DATN_Models.DAO.Interface;
using DATN_Models.DAO.Interface.SeatAbout;
using DATN_Models.HandleData;
using DATN_Models.Mapper;
using DATN_Models.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NekBigCore.Services.WebSockets;
using System.Reflection;

namespace DATN_BackEndApi.Extension
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Db");
            var migrationsAssemblyCore = typeof(DATN_Context).GetTypeInfo().Assembly.GetName().Name;

            services.AddScoped<SessionContextInterceptor>();

            services.AddDbContext<DATN_Context>((serviceProvider, options) =>
            {
                options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssemblyCore));
                options.AddInterceptors(serviceProvider.GetRequiredService<SessionContextInterceptor>());
                options.LogTo(Console.WriteLine);
            });
            services.AddScoped<ChangeLogInterceptor>();
            return services;
        }


        public static IServiceCollection AddServiceContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
            var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();

            services.AddControllersWithViews();
            services.AddAutoMapper(typeof(MapperProfile));
            services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));
            services.AddDistributedMemoryCache();
            services.AddHttpContextAccessor();

            return services;
        }

        public static IServiceCollection AddCoreService(this IServiceCollection services)
        {
            // AddTransient
            services.AddTransient<ILoginDAO, LoginDAO>();

            services.AddTransient<IMovieDAO, MovieDAO>();
            services.AddTransient<IMembershipDAO, MembershipDAO>();


            //_services.AddTransient<IMovieDAO, MovieTESTDAO>();
            services.AddTransient<ICinemasDAO, CinemasDAO>();
            services.AddTransient<IShowTimeDAO, ShowTimeDAO>();



            services.AddTransient<IActorDAO, ActorDAO>();
            services.AddTransient<IRoomDAO, RoomDAO>();
            services.AddTransient<IServiceDAO, ServiceDAO>();
            services.AddTransient<ISeatDAO, SeatDAO>();
            services.AddTransient<IPricingRuleDAO, PricingRuleDAO>();
            services.AddTransient<ICommentDAO, CommentDAO>();
            services.AddTransient<IOrderDAO, OrderDAO>();
            services.AddTransient<IEmployeeDAO, EmployeeDAO>();
            services.AddTransient<IVoucherDAO, VoucherDAO>();
            services.AddTransient<ILogDAO, LogDAO>();
            services.AddTransient<IGenreDAO, GenreDAO>();
            services.AddTransient<IServiceTypeDAO, ServiceTypeDAO>();



            services.AddTransient<ICinemasDAO, CinemasDAO>();
            services.AddTransient<IRatingDAO, RatingDAO>();
            services.AddTransient<UserManager<AppUsers>, UserManager<AppUsers>>();
            services.AddTransient<RoleManager<AppRoles>, RoleManager<AppRoles>>();
            services.AddTransient<ICustomerDAO, CustomerDAO>();
            services.AddTransient<ICinemasDAO, CinemasDAO>();
            services.AddTransient<IShowTimeDAO, ShowTimeDAO>();
            services.AddTransient<IStatisticDAO, StatisticDAO>();
            // AddScoped
            services.AddScoped<IUltil, Ultil>();
            services.AddScoped<WebSocketService>();
            services.AddScoped<UserManager<AppUsers>, UserManager<AppUsers>>();

            // AddSingleton
            services.AddSingleton<IWebSocketManager, WebSocketManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var serviceProvider = services.BuildServiceProvider();
            var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();


            services.Configure<VNPayConfig>(configuration.GetSection(VNPayConfig.ConfigName));
            services.AddScoped<IVNPayService, VNPayService>();


            services.AddSingleton<Cloudinary>(serviceProvider =>
            {
                var config = serviceProvider.GetService<IOptions<CloudinarySettings>>().Value;
                var account = new Account(config.CloudName, config.ApiKey, config.ApiSecret);
                return new Cloudinary(account); // Cái này là account của nghĩa.
            });
            services.AddScoped<CloudService>();
            services.AddIdentity<AppUsers, AppRoles>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 0;
            })
               .AddEntityFrameworkStores<DATN_Context>()
               .AddDefaultTokenProviders();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(8);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            HttpContextHelper.Configure(httpContextAccessor);
            return services;
        }
        public static IServiceCollection AddCORS(this IServiceCollection services, string name)
        {
            // CORS
            services.AddCors(builder =>
            {
                builder.AddPolicy(
                    name: name,
                    policy =>
                    {
                        policy.AllowAnyHeader();
                        policy.AllowAnyMethod();
                        policy.AllowAnyOrigin();
                    }
                    );
            });
            return services;
        }
        public static IServiceCollection AddCoreExtention(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            var assemblies = new List<Assembly>
            {
                Assembly.GetExecutingAssembly(),
                Assembly.Load("DATN_Models"),
            };
            services.AddValidatorsFromAssemblies(assemblies);
            return services;
        }
    }
}
