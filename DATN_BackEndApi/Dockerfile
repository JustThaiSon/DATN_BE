# -------------------------------
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy toàn bộ repo (bao gồm .sln và các project)
COPY ../ .  # quan trọng: copy cả thư mục gốc

# Khôi phục dependency từ solution
RUN dotnet restore "DATN_BE.sln"

# Build và publish project Web API
WORKDIR /src/DATN_BackEndApi
RUN dotnet publish "DATN_BackEndApi.csproj" -c Release -o /app/publish

# -------------------------------
# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "DATN_BackEndApi.dll"]
