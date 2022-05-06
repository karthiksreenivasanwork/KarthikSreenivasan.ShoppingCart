DECLARE @UsernameInputParam NVARCHAR(200) = 'karthik';
DECLARE @CartIDOutput INT;

DECLARE @PepsiProductIDParam NUMERIC(6,0) = (select p.ProductID from T_Products as p where p.ProductName = 'Pepsi');
DECLARE @FortuneOilItemParam NUMERIC(6,0) = (select p.ProductID from T_Products as p where p.ProductName = 'Fortune Oil');
DECLARE @VimItemParam NUMERIC(6,0) = (select p.ProductID from T_Products as p where p.ProductName = 'Vim');

EXEC Sch_CartManagement.sp_CreateCartItems @UsernameInputParam, @PepsiProductIDParam, @CartIDOutputParam = @CartIDOutput OUTPUT
PRINT CAST(@CartIDOutput as varchar) + ' - Card ID for product `Pepsi` added successfully';

EXEC Sch_CartManagement.sp_CreateCartItems @UsernameInputParam, @FortuneOilItemParam, @CartIDOutputParam = @CartIDOutput OUTPUT
PRINT CAST(@CartIDOutput as varchar) + ' - Card ID for product `Fortune Oil` added successfully';

EXEC Sch_CartManagement.sp_CreateCartItems @UsernameInputParam, @VimItemParam, @CartIDOutputParam = @CartIDOutput OUTPUT
PRINT CAST(@CartIDOutput as varchar) + ' - Card ID for product `Vim` added successfully';

EXEC Sch_CartManagement.sp_GetCartItemsForUser 'karthik';