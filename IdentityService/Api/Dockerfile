FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Api/Api.csproj", "Api/"]
COPY ["Logic/Logic.csproj", "Logic/"]
COPY ["Dal/Dal.csproj", "Dal/"]
COPY ["../CoreLib/Domain/Domain.csproj", "../CoreLib/Domain/"]
RUN dotnet restore "Api/Api.csproj"
COPY . .
WORKDIR "/src/Api"
RUN dotnet build "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ДОБАВЛЯЕМ НОВЫЙ STAGE ДЛЯ МИГРАЦИЙ
FROM build AS migrations
WORKDIR "/src"
# Устанавливаем dotnet-ef tool
RUN dotnet tool install --global dotnet-ef --version 9.0.0
ENV PATH="$PATH:/root/.dotnet/tools"
# Создаем миграции
RUN dotnet ef migrations add InitialCreate --project Dal --startup-project Api --output-dir Migrations
# Генерируем SQL скрипт для миграций (опционально)
RUN dotnet ef migrations script --project Dal --startup-project Api --idempotent --output /app/migrations.sql

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
# Копируем скрипт миграций (если нужно)
# COPY --from=migrations /app/migrations.sql ./migrations.sql

# ДОБАВЛЯЕМ ВЫПОЛНЕНИЕ МИГРАЦИЙ ПРИ СТАРТЕ
ENTRYPOINT ["dotnet", "Api.dll", "--migrate"]