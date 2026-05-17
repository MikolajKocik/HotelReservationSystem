FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["HotelReservationSystem.Workers/HotelReservationSystem.Workers.csproj", "HotelReservationSystem.Workers/"]
COPY ["HotelReservationSystem.Application/HotelReservationSystem.Application.csproj", "HotelReservationSystem.Application/"]
COPY ["HotelReservationSystem.Infrastructure/HotelReservationSystem.Infrastructure.csproj", "HotelReservationSystem.Infrastructure/"]
COPY ["HotelReservationSystem.Core/HotelReservationSystem.Core.csproj", "HotelReservationSystem.Core/"]

RUN dotnet restore "HotelReservationSystem.Workers/HotelReservationSystem.Workers.csproj"

COPY . .
WORKDIR "/src/HotelReservationSystem.Workers"

RUN dotnet build "HotelReservationSystem.Workers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HotelReservationSystem.Workers.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HotelReservationSystem.Workers.dll"]
