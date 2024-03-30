# Заметки

Это простенький проект с заметками, чтобы можно было подглядывать, когда будешь делать ДЗ.

## БД

### Секреты

Для работы с БД используется строка подключения, которая задается через `appsettings.json`.

Так как хранить пароль в `appsettings.json`, который мы выкладываем на гит - не самая лучшая идея,
мы будем использовать [secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-8.0&tabs=windows).

Для создания секретов приложения:
1. Открываем терминал.
2. Вбиваем `dotnet user-secrets init`.
3. Проверяем `.csproj` файл, должна появиться секция `<UserSecretsId>`.
4. Для установки секрета используем: `dotnet user-secrets set "PostgreSqlConnection:Password" "12345"`. Или другой пароль, который у нас.
5. Проверяем секрет через: `dotnet user-secrets list`.
6. Теперь в приложении можно обращаться к этому секрету, как будто он у нас в `appsettings.json` в указанной секции.

*Если нужен проект, проставляем его через флаг `--project`*.

**Ожидаемые секреты**

- `PostgreSqlConnection:Password` = `value`
- `PasswordHashProvider:Salt` = `stringWithLen32symbol`
- `JwtTokenGenerator:Secret` = `stringWithLen32symbol`
- `JwtTokenGenerator:Issuer` = `currproj`
- `JwtTokenGenerator:Audience` = `currproj`


### Миграции

Чтобы можно было не терять данные при изменении моделей в ходе жизни проекта используются миграции.
Подробнее про них можно почитать [тут](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli).

Для того, чтобы добавить миграциию используем `dotnet ef migrations add migName`. Так же используем `--project`, если нужно.
Затем миграцию можно накатить `dotnet ef database update`, но если БД уже создана, а мы накатываем миграцию, которая ее
создает, то мы получим ошибку.

## JWT токены

Нужны, чтобы пользователь не хранил на своей стороне пароль, а мог авторизоваться через токен.

Хоть в `appsettings.json` поля `Audience` и `Issuer` и не используются, но они должны быть заполнены (через секреты).
