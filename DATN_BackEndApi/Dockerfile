# -------------------------------
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sao chép toàn bộ project cha (lùi ra khỏi DATN_BackEndApi)
COPY ../ ./

# Di chuyển vào thư mục chứa file .csproj chính
WORKDIR /src/DATN_BackEndApi

# Khôi phục package và biên dịch
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# -------------------------------
# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "DATN_BackEndApi.dll"]
