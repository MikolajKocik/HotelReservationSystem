using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using FluentAssertions;
namespace HotelReservationSystem.Tests.IntegrationTests.Database;

public sealed class DatabaseConnectionTests
{
    private readonly IConfiguration configuration;

    public DatabaseConnectionTests()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();

        this.configuration = builder.Build();
    }

    [Fact]
    public async Task Connection_Should_Open_Successfully()
    {
        string? conn = this.configuration.GetConnectionString("Default")
            ?? this.configuration.GetConnectionString("ConnectionString");

        conn.Should().NotBeNullOrWhiteSpace("a SQL Server connection string must be configured in ConnectionStrings:Default");

        using var connection = new SqlConnection(conn);
        await connection.OpenAsync();

        string query = "SELECT 1";
        using var command = new SqlCommand(query, connection);

        object? result = await command.ExecuteScalarAsync();

        result.Should().Be(1);
    }
}
