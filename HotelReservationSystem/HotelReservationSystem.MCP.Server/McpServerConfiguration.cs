using HotelReservationSystem.MCP.Server.Resources;
using HotelReservationSystem.MCP.Server.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HotelReservationSystem.MCP.Server.Extensions;

namespace HotelReservationSystem.MCP.Server;

public static class McpServerConfiguration
{
    public static IServiceCollection AddHotelMcpServer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("SlackClient", client =>
        {
            var slackUrl = configuration["StaffSettings:SlackWebhookUrl"];
            if (!string.IsNullOrEmpty(slackUrl))
            {
                client.BaseAddress = new Uri(slackUrl);
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

        return services;
    }
}
