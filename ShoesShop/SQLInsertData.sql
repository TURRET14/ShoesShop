USE Emelyanenko_ShoesShop;

INSERT INTO UserRole (Name)
SELECT DISTINCT Роль_Сотрудника FROM UserImport;


INSERT INTO ShopUser (UserRoleID, UserFIO, UserLogin, UserPassword)
SELECT (SELECT ID FROM UserRole WHERE UserRole.Name = UserImport.Роль_Сотрудника), ФИО, Логин, Пароль FROM UserImport;


INSERT INTO OrderStatus (Name)
SELECT DISTINCT Статус_Заказа FROM ЗаказImport;

INSERT INTO PickupPoint (PostIndex, City, Street, HouseIndex)
SELECT PostIndex, City, Street, HouseIndex FROM ПунктыВыдачиImport;


INSERT INTO ProductCategory (Name)
SELECT DISTINCT Категория_Товара FROM TovarImport;


INSERT INTO ProductManufacturer (Name)
SELECT DISTINCT Производитель FROM TovarImport;


INSERT INTO ProductSupplier (Name)
SELECT DISTINCT Поставщик FROM TovarImport;


INSERT INTO Product (ProductArticle, ProductName, ProductUnit, ProductPrice, ProductSupplierID, ProductManufacturerID, ProductCategoryID, ProductDiscount, ProductInStock, ProductDescription, ProductPhotoPath)
SELECT Артикул, Наименование_Товара, Единица_Измерения, Цена, (SELECT ID FROM ProductSupplier WHERE ProductSupplier.Name = TovarImport.Поставщик), (SELECT ID FROM ProductManufacturer WHERE ProductManufacturer.Name = TovarImport.Производитель), (SELECT ID FROM ProductCategory WHERE ProductCategory.Name = TovarImport.Категория_Товара), Действующая_Скидка, Кол_Во_На_Складе, Описание_Товара, Фото FROM TovarImport;

SET IDENTITY_INSERT ShopOrder ON
INSERT INTO ShopOrder (ID, OrderDate, DeliveryDate, PickupPointID, ClientID, Code, OrderStatusID)
SELECT Номер_Заказа, Дата_Заказа, Дата_Доставки, Адрес_Пункта_Выдачи, (SELECT ID FROM ShopUser WHERE ShopUser.UserFIO = ЗаказImport.ФИО_Авторизированного_Клиента), Код_Для_Получения, (SELECT ID FROM OrderStatus WHERE OrderStatus.Name = ЗаказImport.Статус_Заказа) FROM ЗаказImport;


INSERT INTO ShopOrderDetail (ShopOrderID, ProductID, Amount)
SELECT Номер_Заказа, (SELECT ID FROM Product WHERE Product.ProductArticle = ДеталиЗаказаImport.Товар), Количество FROM ДеталиЗаказаImport;