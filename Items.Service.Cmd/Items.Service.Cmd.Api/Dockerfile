#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Items.Service.Cmd/Items.Service.Cmd.Api/Items.Service.Cmd.Api.csproj", "Items.Service.Cmd/Items.Service.Cmd.Api/"]
COPY ["CQRS-ES/CQRS.Core/CQRS.Core.csproj", "CQRS-ES/CQRS.Core/"]
COPY ["Items.Service.Common/Items.Service.Common/Items.Service.Common.csproj", "Items.Service.Common/Items.Service.Common/"]
COPY ["Items.Service.Cmd/Items.Service.Cmd.Application/Items.Service.Cmd.Application.csproj", "Items.Service.Cmd/Items.Service.Cmd.Application/"]
COPY ["Items.Service.Cmd/Items.Service.Cmd.Domain/Items.Service.Cmd.Domain.csproj", "Items.Service.Cmd/Items.Service.Cmd.Domain/"]
COPY ["Items.Service.Cmd/Items.Service.Cmd.Infrastructure/Items.Service.Cmd.Infrastructure.csproj", "Items.Service.Cmd/Items.Service.Cmd.Infrastructure/"]
RUN dotnet restore "Items.Service.Cmd/Items.Service.Cmd.Api/Items.Service.Cmd.Api.csproj"
COPY . .
WORKDIR "/src/Items.Service.Cmd/Items.Service.Cmd.Api"
RUN dotnet build "Items.Service.Cmd.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Items.Service.Cmd.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Items.Service.Cmd.Api.dll"]