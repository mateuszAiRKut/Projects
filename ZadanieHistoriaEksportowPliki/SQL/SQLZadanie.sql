create database Zadanie;
use Zadanie;

create table HistoriaEksportow
(ID int not null primary key identity(1,1),
NazwaEksportu varchar(50) not null,
DataEksportu datetime not null,
NazwaUzytkownika varchar(50) not null,
NazwaLokalu varchar(50) not null);

INSERT INTO HistoriaEksportow
Values ('eksport1', SMALLDATETIMEFROMPARTS(2020,05,07,12,17), 'AdamK','dom');

INSERT INTO HistoriaEksportow
Values ('eksport2', SMALLDATETIMEFROMPARTS(2020,06,07,13,17), 'PatrykW','biuro');

INSERT INTO HistoriaEksportow
Values ('eksport3', SMALLDATETIMEFROMPARTS(2020,06,17,11,17), 'TomaszW','biblioteka');

INSERT INTO HistoriaEksportow
Values ('eksport4', SMALLDATETIMEFROMPARTS(2020,05,12,17,17), 'BartekO','uczelnia');

INSERT INTO HistoriaEksportow
Values ('eksport5', SMALLDATETIMEFROMPARTS(2020,04,12,10,17), 'GrzegorzO','sklep');

INSERT INTO HistoriaEksportow
Values ('eksport6', SMALLDATETIMEFROMPARTS(2020,02,02,11,47), 'TomaszW','biuro');

INSERT INTO HistoriaEksportow
Values ('eksport7', SMALLDATETIMEFROMPARTS(2020,06,20,11,47), 'MarekW','biuro');

INSERT INTO HistoriaEksportow
Values ('eksport8', SMALLDATETIMEFROMPARTS(2020,06,20,11,30), 'MateuszW','park');

select * 
from HistoriaEksportow;

select * 
from HistoriaEksportow
where NazwaLokalu = 'biuro' and DataEksportu >= '2020-05-08 12:17' and DataEksportu <= '2020-06-24 12:00'

create procedure dbo.Stworz_HistoriaEksportow
as
begin
	if OBJECT_ID(N'dbo.HistoriaEksportow', N'U') IS NULL
	begin
		create table HistoriaEksportow
		(ID int not null primary key identity(1,1),
		NazwaEksportu varchar(50) not null,
		DataEksportu datetime not null,
		NazwaUzytkownika varchar(50),
		NazwaLokalu varchar(50));
	end
end

create procedure dbo.Wstaw_Eksport
	@NazwaEksportu varchar(10),
	@DataEksportu datetime,
	@NazwaUzytkownika varchar(50),
	@NazwaLokalu varchar(50)
as
begin
	insert into HistoriaEksportow values(@NazwaEksportu, @DataEksportu, @NazwaUzytkownika, @NazwaLokalu)
end