# Use the ASP.NET Core runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CustomerApi.csproj", "./"]
RUN dotnet restore "./CustomerApi.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "CustomerApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CustomerApi.csproj" -c Release -o /app/publish

# Final stage/image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerApi.dll"]