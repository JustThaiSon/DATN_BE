# -------------------------------
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY DATN_BackEndApi/*.csproj ./DATN_BackEndApi/
RUN dotnet restore ./DATN_BackEndApi/*.csproj

# Copy the rest
COPY DATN_BackEndApi/. ./DATN_BackEndApi/
WORKDIR /src/DATN_BackEndApi

# Publish
RUN dotnet publish -c Release -o /app/publish

# -------------------------------
# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "DATN_BackEndApi.dll"]
