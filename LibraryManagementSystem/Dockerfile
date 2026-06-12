# -----------------------
# 1️⃣ Base runtime image
# -----------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# -----------------------
# 2️⃣ Build stage
# -----------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["*.sln", "./"]
COPY ["src/LMS.API/LMS.API.csproj", "src/LMS.API/"]
COPY ["src/LMS.Application/LMS.Application.csproj", "src/LMS.Application/"]
COPY ["src/LMS.Domain/LMS.Domain.csproj", "src/LMS.Domain/"]
COPY ["src/LMS.Infrastructure/LMS.Infrastructure.csproj", "src/LMS.Infrastructure/"]

RUN dotnet restore "src/LMS.API/LMS.API.csproj"

COPY . .

RUN dotnet build "src/LMS.API/LMS.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# -----------------------
# 3️⃣ Publish stage
# -----------------------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "src/LMS.API/LMS.API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# -----------------------
# 4️⃣ Final stage
# -----------------------
FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "LMS.API.dll"]
