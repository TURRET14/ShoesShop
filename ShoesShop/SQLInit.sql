CREATE DATABASE Emelyanenko_ShoesShop;

GO

USE Emelyanenko_ShoesShop;

CREATE TABLE UserRole (ID INT PRIMARY KEY IDENTITY(0, 1), Name VARCHAR(50) NOT NULL);
CREATE TABLE ShopUser (ID INT PRIMARY KEY IDENTITY(0, 1), UserRoleID INT REFERENCES UserRole NOT NULL, UserFIO VARCHAR(150) NOT NULL, UserLogin VARCHAR(150) NOT NULL, UserPassword VARCHAR(150) NOT NULL);

CREATE TABLE ProductSupplier (ID INT PRIMARY KEY IDENTITY(0, 1), Name VARCHAR(50) NOT NULL);
CREATE TABLE ProductManufacturer (ID INT PRIMARY KEY IDENTITY(0, 1), Name VARCHAR(50) NOT NULL);
CREATE TABLE ProductCategory (ID INT PRIMARY KEY IDENTITY(0, 1), Name VARCHAR(50) NOT NULL);
CREATE TABLE Product (ID INT PRIMARY KEY IDENTITY(0, 1), ProductArticle VARCHAR(50) NOT NULL, ProductName VARCHAR(100) NOT NULL, ProductUnit VARCHAR(50), ProductPrice INT NOT NULL, ProductSupplierID INT REFERENCES ProductSupplier NOT NULL, ProductManufacturerID INT REFERENCES ProductManufacturer, ProductCategoryID INT REFERENCES ProductCategory, ProductDiscount INT NOT NULL, ProductInStock INT NOT NULL, ProductDescription VARCHAR(200) NOT NULL, ProductPhotoPath VARCHAR(200));

CREATE TABLE PickupPoint (ID INT PRIMARY KEY IDENTITY(0, 1), PostIndex INT, City VARCHAR(50), Street VARCHAR(50), HouseIndex INT);
CREATE TABLE OrderStatus (ID INT PRIMARY KEY IDENTITY(0, 1), Name VARCHAR(50) NOT NULL);
CREATE TABLE ShopOrder (ID INT PRIMARY KEY IDENTITY (0,1), OrderNumber INT NOT NULL, ProductID INT REFERENCES Product, ProductAmount INT NOT NULL, OrderDate DATE NOT NULL, DeliveryDate DATE NOT NULL, PickupPointID INT REFERENCES PickupPoint NOT NULL, ClientID INT REFERENCES ShopUser NOT NULL, Code INT NOT NULL, OrderStatusID INT REFERENCES OrderStatus NOT NULL);

GO

INSERT INTO UserRole (Name)
SELECT DISTINCT Роль_Сотрудника FROM UserImport;

GO

INSERT INTO ShopUser (UserRoleID, UserFIO, UserLogin, UserPassword)
SELECT (SELECT ID FROM UserRole WHERE UserRole.Name = UserImport.Роль_Сотрудника), ФИО, Логин, Пароль FROM UserImport;

GO

INSERT INTO OrderStatus (Name)
SELECT DISTINCT Статус_Заказа FROM Заказ_Import;

GO

INSERT INTO PickupPoint (PostIndex, City, Street, HouseIndex)
SELECT PostIndex, City, Street, HouseIndex FROM ПунктыВыдачи_Import;

GO

INSERT INTO ProductCategory (Name)
SELECT DISTINCT Категория_Товара FROM TovarImport;

GO

INSERT INTO ProductManufacturer (Name)
SELECT DISTINCT Производитель FROM TovarImport;

GO

INSERT INTO ProductSupplier (Name)
SELECT DISTINCT Поставщик FROM TovarImport;

GO

INSERT INTO Product (ProductArticle, ProductName, ProductUnit, ProductPrice, ProductSupplierID, ProductManufacturerID, ProductCategoryID, ProductDiscount, ProductInStock, ProductDescription, ProductPhotoPath)
SELECT Артикул, Наименование_Товара, Единица_Измерения, Цена, (SELECT ID FROM ProductSupplier WHERE ProductSupplier.Name = TovarImport.Поставщик), (SELECT ID FROM ProductManufacturer WHERE Productmanufacturer.Name = TovarImport.Производитель), (SELECT ID FROM ProductCategory WHERE ProductCategory.Name = TovarImport.Категория_Товара), Действующая_Скидка, Кол_Во_На_Складе, Описание_Товара, Фото FROM TovarImport;

GO

INSERT INTO ShopOrder (OrderNumber, ProductID, ProductAmount, OrderDate, DeliveryDate, PickupPointID, ClientID, Code, OrderStatusID)
SELECT Номер_Заказа, (SELECT ID FROM Product WHERE Product.ProductArticle = ЗаказImport.Артикул_Заказа), Количество, Дата_Заказа, Дата_Доставки, Адрес_Пункта_Выдачи, (SELECT ID FROM ShopUser WHERE ShopUser.UserFIO = ЗаказImport.ФИО_Авторизированного_Клиента), Код_Для_Получения, (SELECT ID FROM OrderStatus WHERE OrderStatus.Name = ЗаказImport.Статус_Заказа) FROM Заказ_Import;