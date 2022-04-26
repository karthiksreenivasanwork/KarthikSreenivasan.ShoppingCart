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
	* CREATE A NEW ORDER FOR A USER ONLY IF THEY DO NOT HAVE AN ACTVE ORDER
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
@UserIDParam NUMERIC(6,0),
@ProductIDParam NUMERIC(6,0)
AS
BEGIN
	/*
	* ToDo - Use transaction to create an order for a cart and and add cart items to it.
	* In case any issue was encountered while creating an order, then the whole transaction
	* would be rolled back.
	*/
	DECLARE @UserIDInputParam NUMERIC(6,0) = (select u.UserID from T_Users as u where u.UserID = @UserIDParam);
	DECLARE @OrderIDOutput NUMERIC(6,0);
	EXEC Sch_CartManagement.sp_CreateOrReturnExistingOrder @UserIDInputParam, @OrderIDOutputParam = @OrderIDOutput OUTPUT

	INSERT INTO T_Cart (ProductID, OrderID)
	VALUES (
			@ProductIDParam,
			@OrderIDOutput
		);
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
	select c.CartID,  o.OrderID, u.UserID, p.Productname, p.ProductPrice
	from T_Products p
	join T_Cart c
	on c.ProductID = p.ProductID
	join T_Orders o
	on c.OrderID = o.OrderID
	join T_Users u
	on u.UserID = o.UserID
	where o.OrderPurchaseState in
	(select os.OrderPurchaseState from T_LU_OrdersStatus as os where os.OrderStateDescription = 'In Progress')
	and u.Username = @UsernameParam;
END;

PRINT 'Script to create stored procedures for the schema `Sch_CartManagement` completed.'