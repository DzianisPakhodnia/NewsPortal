FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем csproj и восстанавливаем зависимости
COPY *.csproj ./
RUN dotnet restore NewsPortal.csproj

# Копируем весь проект
COPY . ./

# Сборка
RUN dotnet build NewsPortal.csproj -c Release -o /app/build

# Публикация
RUN dotnet publish NewsPortal.csproj -c Release -o /app/publish

# Финальный runtime-образ
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "NewsPortal.dll"]
