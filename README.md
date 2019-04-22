### Сборка
* Создать БД из [script.sql](https://github.com/SalmonLoki/CustomSearch/blob/master/script.sql)
* Изменить значение параметра *Data Source* строки *connectionString* в [App.config](https://github.com/SalmonLoki/CustomSearch/blob/master/CustomSearch/App.config),
используя синтаксис *имя_сервера\имя_экземпляра*
* Добавить свои значения *bingSubscriptionKey*, *bingCustomConfig*, *yandexSubscriptionKey*, *yandexUser*, 
*googleSubscriptionKey*, *googleSearchEngineId*, *googleAppName*
в в [App.config](https://github.com/SalmonLoki/CustomSearch/blob/master/CustomSearch/App.config)
* Собрать проект

--------------------------------------------------------------------------------
### Работа с приложением
* Поиск онлайн: ввести ключевые слова в строку поиска *Online Search*, нажать на кнопку *Search*
* Поиск по сохраненным в БД значениям: ввести ключевые слова в строку поиска *Search in saved results*,
нажать на соответствующую кнопку *Search*
* Выбранный результат отображается в встроенном браузере
