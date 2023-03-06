drop table fuelprices;
create database fuelpricesdb;
use fuelpricesdb;
create table fuelprices(
	Date_Price date not null,
	Price_Fuel decimal (4,3),
    primary key (Date_Price)
    ); 

insert into fuelprices (Date_Price, Price_Fuel)
values (20230102, 4.583);
