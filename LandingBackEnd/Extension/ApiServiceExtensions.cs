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
using DATN_Services.Service;
using DATN_Services.Service.Interfaces;
using DATN_Services.WebSockets;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NekBigCore.Services.WebSockets;
using System.Reflection;
namespace DATN_BackEndApi.Extension
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            var connectionString = configuration.GetConnectionString("Db");

            // Core
            var migrationsAssemblyCore = typeof(DATN_Context).GetTypeInfo().Assembly.GetName().Name;
            services.AddDbContext<DATN_Context>(builder =>
            {
                builder.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssemblyCore));

                builder.LogTo(Console.WriteLine);
            });

            return services;
        }
        public static IServiceCollection AddServiceContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
            var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();

            services.AddControllersWithViews();
            services.AddAutoMapper(typeof(MapperProfile));
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

            services.AddTransient<IActorDAO, ActorDAO>();
            services.AddTransient<IRoomDAO, RoomDAO>();
            services.AddTransient<IServiceDAO, ServiceDAO>();
            services.AddTransient<ISeatDAO, SeatDAO>();
            services.AddTransient<IPricingRuleDAO, PricingRuleDAO>();
            services.AddTransient<ICommentDAO, CommentDAO>();
            services.AddTransient<IOrderDAO, OrderDAO>();
            services.AddSingleton<SeatStatusService>();
            services.AddTransient<IRatingDAO, RatingDAO>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<UserManager<AppUsers>, UserManager<AppUsers>>();
            services.AddTransient<RoleManager<AppRoles>, RoleManager<AppRoles>>();
            services.AddTransient<ICustomerDAO, CustomerDAO>();
            services.AddTransient<IServiceTypeDAO, ServiceTypeDAO>();
            services.AddTransient<IEmployeeDAO, EmployeeDAO>();
            services.AddTransient<ICinemasDAO, CinemasDAO>();
            services.AddTransient<IRoomTypeDAO, RoomTypeDAO>();
            services.AddTransient<IUserVoucherDAO, UserVoucherDAO>();
            services.AddTransient<IVoucherDAO, VoucherDAO>();
            services.AddTransient<IVoucherUIDAO, VoucherUIDAO>();

            // AddScoped
            services.AddScoped<IUltil, Ultil>();
            services.AddScoped<WebSocketService>();
            services.AddScoped<UserManager<AppUsers>, UserManager<AppUsers>>();
            services.AddScoped<IVNPayService, VNPayService>();

            // AddSingleton
            services.AddSingleton<IWebSocketManager, WebSocketManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var serviceProvider = services.BuildServiceProvider();
            var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();

            services.AddIdentity<AppUsers, AppRoles>()
               .AddEntityFrameworkStores<DATN_Context>()
               .AddDefaultTokenProviders();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(8);
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
