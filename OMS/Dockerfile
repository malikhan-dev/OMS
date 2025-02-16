#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OMS/OMS.csproj", "OMS/"]
COPY ["OMS.Application.Commands/OMS.Application.Commands.csproj", "OMS.Application.Commands/"]
COPY ["OMS.Application.Contracts/OMS.Application.Contracts.csproj", "OMS.Application.Contracts/"]
COPY ["OMS.Domain/OMS.Domain.csproj", "OMS.Domain/"]
COPY ["OMS.Application.Queries/OMS.Application.Queries.csproj", "OMS.Application.Queries/"]
COPY ["OMS.Application.Services/OMS.Application.Services.csproj", "OMS.Application.Services/"]
COPY ["OMS.Infrastructure.Messaging.Masstransit/OMS.Infrastructure.Messaging.Masstransit.csproj", "OMS.Infrastructure.Messaging.Masstransit/"]
COPY ["OMS.Infrastructure.Persistance.EF/OMS.Infrastructure.Persistance.EF.csproj", "OMS.Infrastructure.Persistance.EF/"]
RUN dotnet restore "./OMS/OMS.csproj"
COPY . .
WORKDIR "/src/OMS"
RUN dotnet build "./OMS.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./OMS.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OMS.dll"]