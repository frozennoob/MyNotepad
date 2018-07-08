# MyNotepad

08.07.2018 <br>
Добавлена поддержка NHibernate и PostgreSQL
Скрипт для создания БД:
<pre>
CREATE TABLE notes (
	id SERIAL,
	lastName  	  VARCHAR (50) NOT NULL,
	firstName  	  VARCHAR (50) NULL,
	fathersName 	VARCHAR (50) NULL,
	phoneNumber 	VARCHAR (30) NULL,
	email       	VARCHAR (50) NULL,
	birthDate  	  DATE         NULL
);
</pre>	
<hr>
Выгружаем файлы в отдельную папку. В Visual Studio переходим в меню: Файл - Открыть - Решение или проект... Выбираем файл проекта в той папке куда мы его загрузили. После того как Visual Studio откроет наш проект переходим в меню: Сборка - Пакетная сборка, в открывшемся окне ставим галочку напротив конфигурации Release и нажимаем кнопку Сборка.

После этого открываем в проводнике папку с нашим проектом, куда мы его загрузили, внутри нее переходим в bin\release и запускаем файл MyNotepad.exe.
