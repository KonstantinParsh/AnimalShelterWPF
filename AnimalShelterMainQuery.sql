--create database Lab4AnimalShelter

--use Lab4AnimalShelter

--create table Roles(
--role_id int primary key identity,
--role_name nvarchar(50) not null
--)

--insert into Roles (role_name) values
--(N'Администратор'),
--(N'Клиент'),
--(N'Волонтер'),
--(N'Ветеринарный врач')

--create table Users(
--userId int primary key identity not null,
--fullName nvarchar(150) not null,
--phone nvarchar(20) not null,
--login nvarchar(20) not null,
--password nvarchar(10) not null,
--role_id int not null
--constraint FK_Users_Roles foreign key (role_id) references Roles(role_id)
--)

--insert into Users (fullName, phone, login, password, role_id)
--values
--('Иван Иванович Иванов', '+79992341343', 'admin', 'admin', 1),
--('Анна Бананова', '+79113166769', 'client1', 'password', 2)

--create table AnimalCard(
--animalId int primary key identity not null,
--name nvarchar(50) not null,
--species nvarchar(50) not null,
--gender int not null,
--breed nvarchar(100) not null,
--color nvarchar(50) not null,
--age_months int not null,
--an_weight decimal(5,2) not null,
--description nvarchar(200) not null,
--photopath nvarchar(255) not null,
--vaccinations nvarchar(200)
--)

--create table MedicalRecords(
--med_id int primary key identity not null,
--manipulation_date date not null,
--manipulation_type nvarchar(255),
--animal_id int,
--constraint FK_Medical_Animal foreign key (animal_id) references AnimalCard(animalId)
--)

--create table Requests(
--request_id int primary key identity not null,
--userid int,
--animal_id int,
--request_date date default getdate(),
--request_type nvarchar(50),
--status nvarchar(50) default N'В обработке',
--constraint FK_Request_User foreign key (userid) references Users(userId),
--constraint FK_Request_Animal foreign key (animal_id) references AnimalCard(animalId)
--)

--INSERT INTO AnimalCard (name, species, gender, breed, color, age_months, an_weight, description, photopath, vaccinations)
--VALUES 
--('Барсик', 'Кот', 1, 'Европейская короткошерстная', 'Полосатый', 24, 8.50, 'Огромный пушистый добряк, обожает спать на клавиатуре.', 'Images/barsik.jpg', 'От бешенства, комплексная'),
--('Мурка', 'Кошка', 0, 'Дворовая', 'Белый с черными пятнами', 12, 3.20, 'Очень активная и игривая кошечка, отличная мышеловка.', 'Images/murka.jpg', 'Комплексная'),
--('Рекс', 'Собака', 1, 'Немецкая овчарка', 'Чепрачный', 36, 32.00, 'Умный и преданный пес. Знает базовые команды.', 'Images/reks.jpg', 'Бешенство, чумка, парвовирус'),
--('Белла', 'Собака', 0, 'Лабрадор-ретривер', 'Бежевый', 6, 15.50, 'Маленький ураган. Любит всех людей на свете и грызть тапки.', 'Images/bella.jpg', 'Первичная вакцинация пройдена'),
--('Снежок', 'Кот', 1, 'Европейская короткошерстная', 'Макрель', 48, 5.10, 'Спокойный, слегка ленивый аристократ. Требует ухода за шерстью.', 'Images/snezhok.jpg', 'От бешенства'),
--('Кеша', 'Попугай', 1, 'Волнистый', 'Зелено-желтый', 8, 0.05, 'Болтливый парень, умеет говорить "Кеша хороший" и имитировать звонок телефона.', 'Images/kesha.jpg', 'Не требуются'),
--('Багира', 'Кошка', 0, 'Сибирская', 'Белый с черными пятнами', 18, 4.80, 'Грациозная и независимая дама. Не любит сидеть на руках.', 'Images/bagira.jpg', 'Комплексная'),
--('Чарли', 'Собака', 1, 'Французский бульдог', 'Светло-коричневый', 20, 12.30, 'Смешная сосиска на коротких лапках. Обожает долгие прогулки.', 'Images/charli.jpg', 'Все по возрасту'),
--('Пушок', 'Кролик', 1, 'Польский кролик', 'Белый', 10, 2.10, 'Ручной декоративный кролик. Обожает морковку и свежую зелень.', 'Images/pushok.jpg', 'От миксоматоза и ВГБК'),
--('Люси', 'Собака', 0, 'Такса', 'Коричневый', 40, 7.50, 'Хитрая, но очень ласковая. Любит зарываться в одеяла.', 'Images/lucy.jpg', 'Комплексная');

--UPDATE AnimalCard
--SET photopath = '/' + photopath
--WHERE photopath NOT LIKE '/%';

--select * from Users;

--select * from AnimalCard;

--select * from Requests;
--ALTER TABLE Requests ALTER COLUMN animal_id INT NULL;

--ALTER TABLE Requests ADD 
--TempName NVARCHAR(100) NULL,
--TempSpecies NVARCHAR(50) NULL,
--TempGender INT NULL,
--TempBreed NVARCHAR(100) NULL;

--ALTER TABLE Requests ADD
--    TempColor NVARCHAR(100) NULL,
--    TempAgeMonths INT NULL,
--    TempWeight DECIMAL(5,2) NULL,
--    TempDescription NVARCHAR(MAX) NULL,
--    TempPhotoPath NVARCHAR(MAX) NULL,
--    TempVaccinations NVARCHAR(MAX) NULL;

--Select * from Requests where status = N'В обработке'

--select * from Users