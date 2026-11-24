# Test-task-for-66bit

## Запуск

### Через Docker Compose

Из корня репозитория, где находится `compose.yaml`, выполните команду.

```bash
docker compose up --build
```

- Веб-приложение — [http://localhost:8081](http://localhost:8081)
- API — `http://localhost:8080`
- Swagger — [http://localhost:8080/swagger](http://localhost:8080/swagger)

Docker Compose запускает PostgreSQL и передаёт API строку подключения. Миграции применяются при старте API.

## Стек и структура проекта

ASP.NET Core 6, EF Core 7, PostgreSQL 16, Blazor WebAssembly, SignalR и Docker Compose.

- `API` — HTTP-эндпоинты, Swagger и хаб SignalR.
- `Application` — сервисы, DTO, валидация и интерфейсы для работы с данными.
- `Data` — репозитории EF Core, миграции и файловое хранилище фотографий.
- `Models` — сущности склада и справочников.
- `frontend` — страницы и компоненты Blazor, HTTP-клиенты и подписка на обновления через SignalR.
