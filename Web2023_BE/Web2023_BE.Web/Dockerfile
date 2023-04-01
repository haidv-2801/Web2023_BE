#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Web2023_BE.Web/Web2023_BE.Web.csproj", "Web2023_BE.Web/"]
COPY ["Web2023_BE.HostBase/Web2023_BE.HostBase.csproj", "Web2023_BE.HostBase/"]
COPY ["Web2023_BE.Extension/Web2023_BE.Extension.csproj", "Web2023_BE.Extension/"]
COPY ["Web2023_BE.Cache.Redis/Web2023_BE.Cache.Redis.csproj", "Web2023_BE.Cache.Redis/"]
COPY ["Web2023_BE.Cache/Web2023_BE.Cache.csproj", "Web2023_BE.Cache/"]
COPY ["Web2023_BE.ApplicationCore/Web2023_BE.ApplicationCore.csproj", "Web2023_BE.ApplicationCore/"]
COPY ["Web2023_BE.Domain.Shared/Web2023_BE.Domain.Shared.csproj", "Web2023_BE.Domain.Shared/"]
COPY ["Web2023_BE.Infarstructure/Web2023_BE.Infarstructure.csproj", "Web2023_BE.Infarstructure/"]
RUN dotnet restore "Web2023_BE.Web/Web2023_BE.Web.csproj"
COPY . .
WORKDIR "/src/Web2023_BE.Web"
RUN dotnet build "Web2023_BE.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web2023_BE.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web2023_BE.Web.dll"]