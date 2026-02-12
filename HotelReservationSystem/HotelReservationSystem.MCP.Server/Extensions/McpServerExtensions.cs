using Microsoft.Extensions.DependencyInjection;
using ModelContextProtocol.Server;
using System.Reflection;

namespace HotelReservationSystem.MCP.Server.Extensions;

public static class McpServerExtensions
{
    public static IMcpServerBuilder AddToolsFromAssembly(this IMcpServerBuilder builder, Assembly assembly)
    {
        var services = builder.Services;
        var toolTypes = assembly.GetTypes()
            .Where(t => t.GetMethods().Any(m => m.GetCustomAttribute<McpServerToolAttribute>() != null))
            .ToList();

        foreach (var type in toolTypes)
        {
            services.AddScoped(type);
        }

        return builder;
    }

    public static IMcpServerBuilder AddResourcesFromAssembly(this IMcpServerBuilder builder, Assembly assembly)
    {
        var services = builder.Services;
        var resourceTypes = assembly.GetTypes()
            .Where(t => t.GetMethods().Any(m => m.GetCustomAttribute<McpServerResourceAttribute>() != null))
            .ToList();

        foreach (var type in resourceTypes)
        {
            services.AddScoped(type);
        }

        return builder;
    }
}
