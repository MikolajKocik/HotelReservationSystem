using HotelReservationSystem.MCP.Server.Resources;
using HotelReservationSystem.MCP.Server.Services;
using HotelReservationSystem.MCP.Server.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HotelReservationSystem.MCP.Server.Extensions;
using HotelReservationSystem.MCP.Server.Interfaces;

namespace HotelReservationSystem.MCP.Server;

public static class McpServerConfiguration
{
    public static IServiceCollection AddHotelMcpServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("DiscordClient", client =>
        {
            var discordUrl = configuration["StaffSettings:DiscordWebhookUrl"];
            if (!string.IsNullOrEmpty(discordUrl))
            {
                client.BaseAddress = new Uri(discordUrl);
            }
        });

        services.AddMcpServer(options =>
        {
            options.ServerInfo = new ModelContextProtocol.Protocol.Implementation
            {
                Name = "Aurora Hotel Server",
                Version = "1.0.0"
            };
        })
        .AddToolsFromAssembly(typeof(ReceptionTools).Assembly)
        .AddResourcesFromAssembly(typeof(HotelInfoResources).Assembly);

        services.AddScoped<IAgentService, AgentService>();

        return services;
    }
}
