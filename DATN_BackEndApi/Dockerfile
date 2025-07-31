FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

WORKDIR /src/DATN_BackEndApi
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port
EXPOSE 80
ENTRYPOINT ["dotnet", "DATN_BackEndApi.dll"]
