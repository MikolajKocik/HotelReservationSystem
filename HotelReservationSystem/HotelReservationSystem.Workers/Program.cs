using HotelReservationSystem.Workers.Configuration;
using Microsoft.Extensions.Hosting;

var builder = new HostApplicationBuilder();
builder.Services.RegisterHostedServices();

var app = builder.Build();
await app.RunAsync();