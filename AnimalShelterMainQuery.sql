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