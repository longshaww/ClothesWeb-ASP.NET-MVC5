/*
Created		10/04/2022
Modified		11/04/2022
Project		
Model		
Company		
Author		
Version		
Database		MS SQL 7 
*/


Create table [Product] (
	[nameProduct] Nvarchar(255) NOT NULL,
	[idCollection] Nvarchar(255) NOT NULL,
	[idProduct] Nvarchar(255) NOT NULL,
	[sizeM] Integer NOT NULL,
	[sizeL] Integer NOT NULL,
	[sizeXL] Integer NOT NULL,
	[price] Float NOT NULL,
	[isNew] Bit Default 0 NOT NULL,
Primary Key  ([idProduct])
) 
go

Create table [Collection] (
	[idCollection] Nvarchar(255) NOT NULL,
	[nameCollection] Nvarchar(255) NOT NULL,
Primary Key  ([idCollection])
) 
go

Create table [User] (
	[idUser] Nvarchar(255) NOT NULL,
	[idPermission] Nvarchar(255) NOT NULL,
	[username] Nvarchar(255) NOT NULL,
	[password] Nvarchar(255) NOT NULL,
	[gender] Bit NOT NULL,
	[identityCard] Integer NOT NULL,
	[address] Ntext NOT NULL,
	[email] Nvarchar(255) NOT NULL,
	[URLAvatar] Text NULL,
	[phone] Integer NOT NULL,
Primary Key  ([idUser])
) 
go

Create table [ImageProduct] (
	[idImage] Nvarchar(255) NOT NULL,
	[idProduct] Nvarchar(255) NOT NULL,
	[URLImage] Text NOT NULL,
Primary Key  ([idImage])
) 
go

Create table [Permission] (
	[idPermission] Nvarchar(255) NOT NULL,
	[namePermission] Nvarchar(255) NOT NULL,
	[level] Integer Default 1 NOT NULL,
Primary Key  ([idPermission])
) 
go

Create table [Voucher] (
	[idVoucher] Nvarchar(255) NOT NULL,
	[percent] Nvarchar(255) NOT NULL,
	[dateStart] Datetime NOT NULL,
	[dateEnd] Datetime NOT NULL,
Primary Key  ([idVoucher])
) 
go

Create table [Customer] (
	[idCustomer] Nvarchar(255) NOT NULL,
	[name] Nvarchar(255) NOT NULL,
	[email] Nvarchar(255) NOT NULL,
	[address] Nvarchar(255) NOT NULL,
	[phone] Nvarchar(255) NOT NULL,
Primary Key  ([idCustomer])
) 
go

Create table [Bill] (
	[idBill] Nvarchar(255) NOT NULL,
	[idUser] Nvarchar(255) NULL,
	[idCustomer] Nvarchar(255) NULL,
	[Shipping] Float NOT NULL,
	[Total] Float NOT NULL,
	[PTTT] Nvarchar(255) NOT NULL,
	[status] Bit NOT NULL,
	[createdAt] Datetime NOT NULL,
	[TotalQty] Integer NOT NULL,
Primary Key  ([idBill])
) 
go

Create table [DetailBIll] (
	[idDetailBill] Nvarchar(255) NOT NULL,
	[idProduct] Nvarchar(255) NOT NULL,
	[idBill] Nvarchar(255) NOT NULL,
	[qty] Integer NOT NULL,
	[ProductTotal] Float NOT NULL,
	[Size] Nvarchar(255) NULL,
Primary Key  ([idDetailBill],[idBill])
) 
go


Alter table [ImageProduct] add  foreign key([idProduct]) references [Product] ([idProduct]) 
go
Alter table [DetailBIll] add  foreign key([idProduct]) references [Product] ([idProduct]) 
go
Alter table [Product] add  foreign key([idCollection]) references [Collection] ([idCollection]) 
go
Alter table [Bill] add  foreign key([idUser]) references [User] ([idUser]) 
go
Alter table [User] add  foreign key([idPermission]) references [Permission] ([idPermission]) 
go
Alter table [Bill] add  foreign key([idCustomer]) references [Customer] ([idCustomer]) 
go
Alter table [DetailBIll] add  foreign key([idBill]) references [Bill] ([idBill]) 
go


Set quoted_identifier on
go


Set quoted_identifier off
go


