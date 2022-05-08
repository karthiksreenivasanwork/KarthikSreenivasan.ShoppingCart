IF NOT EXISTS (SELECT S.NAME FROM SYS.SCHEMAS S WHERE S.NAME = 'Sch_CartManagement')
BEGIN
	EXEC('CREATE SCHEMA Sch_CartManagement')
	PRINT 'SCHEMA ''Sch_CartManagement'' CREATED';
END
ELSE
BEGIN
	PRINT 'SCHEMA ''Sch_CartManagement'' already exists'; 
END
GO

CREATE OR ALTER PROCEDURE Sch_CartManagement.sp_CreateOrReturnExistingOrder
@UserIDParam NUMERIC(6,0),
@OrderIDOutputParam NUMERIC(6,0) Output
AS
BEGIN
	DECLARE @OrderStateDescription NVARCHAR(200) = 'In Progress';
	/*
	* CREATE A NEW ORDER FOR A USER ONLY IF THEY DO NOT HAVE AN ACTIVE ORDER
	* Additionally, whenever a new order is created, it has - In Progress by default which means check out is not yet complete.
	*/
	IF NOT EXISTS(SELECT * FROM T_Orders AS o where o.UserID = @UserIDParam and o.OrderPurchaseState in
	(select os.OrderPurchaseState from T_LU_OrdersStatus as os where os.OrderStateDescription = @OrderStateDescription))
		BEGIN
			INSERT INTO T_Orders (UserID, OrderPurchaseState, OrderDate)
			VALUES (
					@UserIDParam,
					(SELECT O.OrderPurchaseState FROM T_LU_OrdersStatus AS O WHERE O.OrderStateDescription = @OrderStateDescription),
					((SELECT cast(GETDATE() as Date)))
				);
		END

	SELECT @OrderIDOutputParam = o.OrderID FROM T_Orders AS o where o.UserID = @UserIDParam and o.OrderPurchaseState in
	(select os.OrderPurchaseState from T_LU_OrdersStatus as os where os.OrderStateDescription = @OrderStateDescription);
END
GO

CREATE OR ALTER PROCEDURE Sch_CartManagement.sp_CreateCartItems
@UserNameParam NVARCHAR(200),
@ProductIDParam NUMERIC(6,0)
AS
BEGIN
	DECLARE @CartItemDetails TABLE (  
	CartID NUMERIC(6,0),
	OrderID NUMERIC(6,0),
	UserID NUMERIC(6,0),
	Username NVARCHAR(200),
	ProductID NUMERIC(6,0)
	);

	/*
	* ToDo - Use transactions to create an order for a cart and and add cart items to it.
	* In case any issue was encountered while creating an order, then the whole transaction
	* would be rolled back.
	*/
	DECLARE @UserIDInputParam NUMERIC(6,0) = (select u.UserID from T_Users as u where u.Username = @UserNameParam);
	DECLARE @OrderIDOutput NUMERIC(6,0);
	EXEC Sch_CartManagement.sp_CreateOrReturnExistingOrder @UserIDInputParam, @OrderIDOutputParam = @OrderIDOutput OUTPUT

	INSERT INTO T_Cart (ProductID, OrderID)
	VALUES (
			@ProductIDParam,
			@OrderIDOutput
		);
	
	INSERT INTO @CartItemDetails (
			CartID,
			OrderID,
			UserID,
			Username,
			ProductID)
	VALUES (
			SCOPE_IDENTITY(),
			@OrderIDOutput,
			@UserIDInputParam,
			@UserNameParam,
			@ProductIDParam
		);

SELECT tcd.*, p.ProductName, p.ProductPrice FROM @CartItemDetails tcd join T_Products p
on tcd.ProductID = p.ProductID;

END
GO

CREATE OR ALTER PROCEDURE Sch_CartManagement.sp_GetCartItemsForUser
@UsernameParam NVARCHAR(200)
AS
BEGIN
	/*
	This returns the list of products for specific user (ID = 1) who has not completed
	the checkout.
	*/
	select  o.OrderID, p.ProductID, p.ProductName,count(p.Productname) 'Quantity', sum(p.ProductPrice) 'TotalAmount', p.ProductImageName, u.UserID, u.Username
	from T_Products p
	join T_Cart c
	on c.ProductID = p.ProductID
	join T_Orders o
	on c.OrderID = o.OrderID
	join T_Users u
	on u.UserID = o.UserID
	where o.OrderPurchaseState in
	(select os.OrderPurchaseState from T_LU_OrdersStatus as os where os.OrderStateDescription = 'In Progress')
	and u.Username = @UsernameParam
	group by p.ProductName, p.ProductID, p.ProductPrice, p.ProductImageName, u.UserID, u.Username, o.OrderID;
END;
GO

CREATE OR ALTER PROCEDURE Sch_CartManagement.spRemoveProductQtyFromCart
@OrderIDParam NUMERIC(6,0),
@ProductIDParam NUMERIC(6,0)
AS
BEGIN
DECLARE @CartItemDeleteTableVariable TABLE (  
	CartID NUMERIC(6,0),
	OrderID NUMERIC(6,0),
	ProductID NUMERIC(6,0)
	);
DELETE FROM T_Cart 
OUTPUT
DELETED.CartID,
DELETED.OrderID,
DELETED.ProductID
INTO @CartItemDeleteTableVariable
WHERE CartID IN
(SELECT MAX(c.cartID) FROM T_Cart c WHERE c.OrderID = @OrderIDParam and c.ProductID = @ProductIDParam);

SELECT tc.*, p.ProductPrice, p.ProductName FROM @CartItemDeleteTableVariable tc join T_Products p
on tc.ProductID = p.ProductID;
END
GO

CREATE OR ALTER PROCEDURE Sch_CartManagement.spRemoveProductFromCart
@OrderIDParam NUMERIC(6,0),
@ProductIDParam NUMERIC(6,0)
AS
BEGIN
DECLARE @CartItemDeleteTableVariable TABLE (  
	CartID NUMERIC(6,0),
	OrderID NUMERIC(6,0),
	ProductID NUMERIC(6,0)
	);
DELETE FROM T_Cart 
OUTPUT
DELETED.CartID,
DELETED.OrderID,
DELETED.ProductID
INTO @CartItemDeleteTableVariable
WHERE CartID IN
(SELECT c.cartID FROM T_Cart c WHERE c.OrderID = @OrderIDParam and c.ProductID = @ProductIDParam);

SELECT tc.*, p.ProductPrice, p.ProductName FROM @CartItemDeleteTableVariable tc join T_Products p
on tc.ProductID = p.ProductID;
END
GO

PRINT 'Script to create stored procedures for the schema `Sch_CartManagement` completed.'