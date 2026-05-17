using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace HotelReservationSystem.Tests.IntegrationTests.API.Common.Fixture;

public abstract class ControllerFixture : IClassFixture<WebApplicationFactory<Program>>
{
    protected HttpClient Client { get; private set; }   
    protected WebApplicationFactory<Program> Factory { get; private set; }
    
    protected ControllerFixture(WebApplicationFactory<Program> factory)
    {
        Factory = factory;

        Client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                var testConfig = new Dictionary<string, string>
                {
                    {"ConnectionStrings:DefaultConnection", $"{Environment.GetEnvironmentVariable("TESTS_CONN_STRING")}"},                    
                    {"ConnectionStrings:Redis", "localhost:6380"},
                    {"OpenAI:Endpoint", "http://localhost:11435/v1/"},
                    {"OpenAI:ApiKey", "fake-ollama-key"},
                    {"OpenAI:ModelId", "llama3.2:latest"}
                };

                configBuilder.AddInMemoryCollection(testConfig!);
            });
        }).CreateClient();
    }
}