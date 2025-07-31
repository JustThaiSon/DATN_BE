# -------------------------------
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sao chép toàn bộ source code lên container
COPY ../ . 

# Khôi phục dependency từ file solution
RUN dotnet restore "DATN_BE.sln"

# Build và publish riêng project Web API
WORKDIR /src/DATN_BackEndApi
RUN dotnet publish "DATN_BackEndApi.csproj" -c Release -o /app/publish

# -------------------------------
# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "DATN_BackEndApi.dll"]
