# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем csproj отдельно для кеширования зависимостей
COPY NewsPortal/NewsPortal.csproj ./NewsPortal/
RUN dotnet restore ./NewsPortal/NewsPortal.csproj

# Копируем остальной код проекта
COPY NewsPortal/. ./NewsPortal/

# Сборка и публикация
WORKDIR /src/NewsPortal
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

# Финальный образ
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "NewsPortal.dll"]
