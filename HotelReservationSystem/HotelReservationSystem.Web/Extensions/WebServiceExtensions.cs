using Microsoft.AspNetCore.Identity;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.RateLimiting;
using HotelReservationSystem.Infrastructure.Data;
using Microsoft.AspNetCore.RateLimiting;
using HotelReservationSystem.Application.Interfaces;
using HotelReservationSystem.Application.Services;
using HotelReservationSystem.Core.Domain.Entities;


namespace HotelReservationSystem.Web.Extensions;

/// <summary>
/// Extension methods for registering Web layer services
/// </summary>
public static class WebServiceExtensions
{
    /// <summary>
    /// Registers MVC and related services for the Web layer
    /// </summary>
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddIdentityServices();
        services.AddPolicyBasedAuthorization();
        services.AddValidationServices();
        services.AddRateLimitingServices();
        services.AddCookieAndSessionAuthentication();

        services.AddMvc(options =>
        {
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        });

        services.AddScoped<IFileService, FileService>();
        services.AddHttpClient();

        return services;
    }

    /// <summary>
    /// Registers Identity services
    /// </summary>
    private static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentity<Guest, IdentityRole>()
            .AddEntityFrameworkStores<HotelDbContext>()
            .AddDefaultTokenProviders();

        const string AllowedUserNameCharacters = 
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 6;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 5;
            
            options.SignIn.RequireConfirmedEmail = false;
            options.User.RequireUniqueEmail = true;

            options.User.AllowedUserNameCharacters = AllowedUserNameCharacters;
        });

        return services;
    }

    /// <summary>
    /// Configures cookie authentication
    /// </summary>
    /// 
    private static IServiceCollection AddCookieAndSessionAuthentication(this IServiceCollection services)
    {
        services.ConfigureApplicationCookie(options =>
        {
            options.SlidingExpiration = false;
        });

        services.AddSession(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.Expiration = TimeSpan.FromHours(1);
        });

        return services;
    }

    private static IServiceCollection AddPolicyBasedAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireManager", policy => policy.RequireRole("Manager"));
            options.AddPolicy("RequireRecepcionist", policy => policy.RequireRole("Recepcionist"));
            options.AddPolicy("RequireGuest", policy => policy.RequireRole("Guest"));

            options.AddPolicy("RequireStaff", policy =>
                policy.RequireRole("Manager", "Recepcionist"));
            options.AddPolicy("RequireAnyUser", policy =>
                policy.RequireRole("Manager", "Recepcionist", "Guest"));
        });

        return services;
    }

    /// <summary>
    /// Registers FluentValidation services
    /// </summary>
    private static IServiceCollection AddValidationServices(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation();
        services.AddValidatorsFromAssemblyContaining<Program>();

        return services;
    }

    /// <summary>
    /// Registers rate limiting services
    /// </summary>
    private static IServiceCollection AddRateLimitingServices(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                return RateLimitPartition.GetFixedWindowLimiter("GlobalLimiter", _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 100,
                    Window = TimeSpan.FromMinutes(1),
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = 0
                });
            });

            options.AddFixedWindowLimiter("LoginPolicy", opt =>
            {
                opt.PermitLimit = 10;
                opt.Window = TimeSpan.FromMinutes(1);
                opt.QueueLimit = 0;
            });
        });

        return services;
    }
}