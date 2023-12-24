#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Items.Service.Query/Items.Service.Query.Api/Items.Service.Query.Api.csproj", "Items.Service.Query/Items.Service.Query.Api/"]
COPY ["CQRS-ES/CQRS.Core/CQRS.Core.csproj", "CQRS-ES/CQRS.Core/"]
COPY ["Items.Service.Common/Items.Service.Common/Items.Service.Common.csproj", "Items.Service.Common/Items.Service.Common/"]
COPY ["Items.Service.Query/Items.Service.Query.Application/Items.Service.Query.Application.csproj", "Items.Service.Query/Items.Service.Query.Application/"]
COPY ["Items.Service.Query/Items.Service.Query.Domain/Items.Service.Query.Domain.csproj", "Items.Service.Query/Items.Service.Query.Domain/"]
COPY ["Items.Service.Query/Items.Service.Query.Infrastructure/Items.Service.Query.Infrastructure.csproj", "Items.Service.Query/Items.Service.Query.Infrastructure/"]
RUN dotnet restore "Items.Service.Query/Items.Service.Query.Api/Items.Service.Query.Api.csproj"
COPY . .
WORKDIR "/src/Items.Service.Query/Items.Service.Query.Api"
RUN dotnet build "Items.Service.Query.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Items.Service.Query.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Items.Service.Query.Api.dll"]