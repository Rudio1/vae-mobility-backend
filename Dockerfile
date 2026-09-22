FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["nuget.config", "./"]
COPY ["Directory.Build.props", "./"]
COPY ["src/VaeMobility.WebApi/VaeMobility.WebApi.csproj", "src/VaeMobility.WebApi/"]
COPY ["src/VaeMobility.Application/VaeMobility.Application.csproj", "src/VaeMobility.Application/"]
COPY ["src/VaeMobility.Domain/VaeMobility.Domain.csproj", "src/VaeMobility.Domain/"]
COPY ["src/VaeMobility.Infra.Data/VaeMobility.Infra.Data.csproj", "src/VaeMobility.Infra.Data/"]
COPY ["src/VaeMobility.Infra.ExternalApis/VaeMobility.Infra.ExternalApis.csproj", "src/VaeMobility.Infra.ExternalApis/"]

RUN dotnet restore "src/VaeMobility.WebApi/VaeMobility.WebApi.csproj"

COPY . .

RUN dotnet publish "src/VaeMobility.WebApi/VaeMobility.WebApi.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

RUN cp src/VaeMobility.WebApi/appsettings.example.json /app/publish/appsettings.json

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "VaeMobility.WebApi.dll"]
