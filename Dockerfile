# Base image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ScamShieldAI.csproj", "."]
RUN dotnet restore "./ScamShieldAI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ScamShieldAI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ScamShieldAI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ScamShieldAI.dll"]