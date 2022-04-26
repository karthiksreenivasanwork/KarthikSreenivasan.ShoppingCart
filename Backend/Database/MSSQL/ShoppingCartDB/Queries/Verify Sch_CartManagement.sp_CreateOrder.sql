DECLARE @UserIDInputParam NUMERIC(6,0) = (select u.UserID from T_Users as u where u.Username = 'dharini');
--DECLARE @OrderIDOutput NUMERIC(6,0);
--EXEC Sch_CartManagement.sp_CreateOrReturnExistingOrder @UserIDInputParam, @OrderIDOutputParam = @OrderIDOutput OUTPUT

DECLARE @PepsiProductIDParam NUMERIC(6,0) = (select p.ProductID from T_Products as p where p.ProductName = 'Pepsi');
DECLARE @CookerProductIDParam NUMERIC(6,0) = (select p.ProductID from T_Products as p where p.ProductName = 'Cooker');
DECLARE @AppleProductIDParam NUMERIC(6,0) = (select p.ProductID from T_Products as p where p.ProductName = 'Apple');

EXEC Sch_CartManagement.sp_CreateCartItems @UserIDInputParam, @PepsiProductIDParam
EXEC Sch_CartManagement.sp_CreateCartItems @UserIDInputParam, @CookerProductIDParam
EXEC Sch_CartManagement.sp_CreateCartItems @UserIDInputParam, @AppleProductIDParam

EXEC Sch_CartManagement.sp_GetCartItemsForUser 'dharini'