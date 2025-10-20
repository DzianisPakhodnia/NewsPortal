# Используем SDK вместо runtime
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Копируем csproj и восстанавливаем зависимости
COPY *.csproj ./
RUN dotnet restore

# Копируем весь проект
COPY . ./

# Сборка проекта
RUN dotnet build -c Release -o /app/build

# Публикация
RUN dotnet publish -c Release -o /app/publish

# Финальный образ (можно использовать runtime, если EF уже применены)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NewsPortal.dll"]
