FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /app

# Копируем проект
COPY NewsPortal/NewsPortal.csproj ./NewsPortal/
RUN dotnet restore ./NewsPortal/NewsPortal.csproj

COPY NewsPortal/. ./NewsPortal/

WORKDIR /app/NewsPortal

# Сборка и публикация (можно оставить, но внутри SDK контейнера можно и build и ef)
RUN dotnet build -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish

# Установка dotnet-ef
RUN dotnet tool install --global dotnet-ef --version 8.0.0
ENV PATH="$PATH:/root/.dotnet/tools"

# Запуск приложения
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "/app/publish/NewsPortal.dll"]
