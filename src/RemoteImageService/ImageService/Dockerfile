# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Устанавливаем необходимые инструменты
RUN apt-get update && apt-get install -y \
    curl \
    unzip \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src

# Копируем .proto файл
COPY RemoteImageService/image_processor.proto ../

# Копируем файлы проекта
COPY RemoteImageService/ImageService/ .

# Восстанавливаем зависимости
RUN dotnet restore "ImageService.csproj"

# Собираем проект
RUN dotnet build "ImageService.csproj" -c Release -o /app/build

# Публикуем
RUN dotnet publish "ImageService.csproj" -c Release -o /app/publish

# Этап рантайма
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

# Устанавливаем ImageMagick (для работы Magick.NET)
RUN apt-get update && apt-get install -y \
    imagemagick \
    libgdiplus \
    libc6-dev \
    && rm -rf /var/lib/apt/lists/*

# Настройка ImageMagick для безопасности
RUN sed -i '/<policy domain="coder" rights="none" pattern="PDF" \/>/a\ \ <policy domain="coder" rights="none" pattern="MVG" \/>' /etc/ImageMagick-6/policy.xml

WORKDIR /app

# Копируем опубликованные файлы
COPY --from=build /app/publish .

# Экспортируем порты
EXPOSE 8080
EXPOSE 8081

# Запуск приложения
ENTRYPOINT ["dotnet", "ImageService.dll"]