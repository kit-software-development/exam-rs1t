# Crypto wallet

## Описание
Клиент-серверное приложение, реализующие функциональность Bitcoin-кошелька. По сути выполняет 3 функции:
* Генерация адреса, на который можно получать деньги
* Отправка денег на другой адрес
* Просмотр истории транзакций

Доступ к приложению осуществляется с помощью авторизации по емейлу и паролю.

Приложение работает с блокчейном TestNet. (У биткоина есть две публичные сети - MainNet и TestNet. MainNet работает с реальными биткоинами, а Testnet сделан специально для разработки и тестирования, в нем быстрее проходят транзации, а TestNet-биткоины не имеют ценности и их можно получить бесплатно. В остальном отличий нет и сеть можно поменять строчкой в конфиге)

## Реализация

* Бэкенд - `ASP.NET Core`
* БД - `PostgreSQL` и `Entity Framework Core`
* Фронтенд - `AngularJS`, минимально простой и без наворотов
* Библиотеки - `NBitcoin` - обертка над Bitcoin Core для .NET, `QBitNinja.Client` - .NET клиент для bitcoin block explorer-а, `Swagger` - для описания API приложения

## Запуск приложения локально

Перейти в папку с проектом и выполнить в терминале
```
dotnet run
```
Сайт будет доступен на `https://localhost:5001/`. Также можно посмотреть API через Swagger на `https://localhost:5001/swagger/index.html`
