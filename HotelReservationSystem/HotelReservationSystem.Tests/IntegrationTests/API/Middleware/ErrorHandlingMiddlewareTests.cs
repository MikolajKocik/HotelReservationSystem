using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using HotelReservationSystem.Web.Middleware.MiddlewareExtensions;
using System.Net;
using Microsoft.AspNetCore.Builder;
using FluentAssertions;

namespace HotelReservationSystem.Tests.IntegrationTests.API.Middleware;

public sealed class ErrorHandlingMiddlewareTests
{
    [Fact]
    public async Task Middleware_Returns500_WhenExceptionIsThrown()
    {
        using var host = await new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder
                    .UseTestServer()
                    .Configure(app =>
                    {
                        app.UseErrorHandlingMiddleware();
                        app.Run(_ => throw new Exception());
                    });
            })
            .StartAsync();

        HttpResponseMessage response = await host.GetTestClient().GetAsync("/");

        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task Middleware_Returns404_WhenKeyNotFoundExceptionIsThrown()
    {
        using var host = await new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder
                    .UseTestServer()
                    .Configure(app =>
                    {
                        app.UseErrorHandlingMiddleware();
                        app.Run(_ => throw new KeyNotFoundException("Resource not found"));
                    });
            })
            .StartAsync();

        HttpResponseMessage response = await host.GetTestClient().GetAsync("/nonexistent-endpoint");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Middleware_Returns401_WhenUnauthorizedAccessExceptionIsThrown()
    {
        using var host = await new HostBuilder()
            .ConfigureWebHost(webBuilder =>
            {
                webBuilder
                    .UseTestServer()
                    .Configure(app =>
                    {
                        app.UseErrorHandlingMiddleware();
                        app.Run(_ => throw new UnauthorizedAccessException());
                    });
            })
            .StartAsync();

        HttpResponseMessage response = await host.GetTestClient().GetAsync("/protected-resource");

        string body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Unauthorized access.");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
