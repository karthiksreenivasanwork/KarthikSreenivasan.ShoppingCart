/*
 Insert data into T_Products
 NOTE: Ensure that the corresponding images are saved to the API image folder - Backend\API\ShoppingCart.API\ShoppingCart.API\wwwroot\images
	   Otherwise, the staged data below will have no corresponding product images.
 */

DECLARE @ProductCreatedOutputID INT 
exec Sch_ProductManagement.sp_CreateProduct 'Branded Foods', 'Pepsi', 100, 'A product from ABC company', '2.png', @ProductIDOutputParam = @ProductCreatedOutputID OUTPUT
PRINT 'Product Created with ID : ' + CAST(@ProductCreatedOutputID as NVARCHAR);
exec Sch_ProductManagement.sp_CreateProduct 'Households', 'Vim', 50, 'A product from a DEF company', '18.png', @ProductIDOutputParam = @ProductCreatedOutputID OUTPUT
PRINT 'Product Created with ID : ' + CAST(@ProductCreatedOutputID as NVARCHAR);
exec Sch_ProductManagement.sp_CreateProduct 'Kitchen', 'Cooker', 250, 'A product from GHI company', '24.png', @ProductIDOutputParam = @ProductCreatedOutputID OUTPUT
PRINT 'Product Created with ID : ' + CAST(@ProductCreatedOutputID as NVARCHAR);
exec Sch_ProductManagement.sp_CreateProduct 'Fruits', 'Apple', 450, 'A product from JKL company', '11.png', @ProductIDOutputParam = @ProductCreatedOutputID OUTPUT
PRINT 'Product Created with ID : ' + CAST(@ProductCreatedOutputID as NVARCHAR);
exec Sch_ProductManagement.sp_CreateProduct 'Branded Foods', 'Biscuits', 25, 'A product from ABC company', '8.png', @ProductIDOutputParam = @ProductCreatedOutputID OUTPUT
PRINT 'Product Created with ID : ' + CAST(@ProductCreatedOutputID as NVARCHAR);
exec Sch_ProductManagement.sp_CreateProduct 'Kitchen', 'Fortune oil', 300, 'A product from GHI company', '1.png', @ProductIDOutputParam = @ProductCreatedOutputID OUTPUT
PRINT 'Product Created with ID : ' + CAST(@ProductCreatedOutputID as NVARCHAR);
exec Sch_ProductManagement.sp_CreateProduct 'Fruits', 'Banana', 50, 'A product from MNO company', '11.png', @ProductIDOutputParam = @ProductCreatedOutputID OUTPUT
PRINT 'Product Created with ID : ' + CAST(@ProductCreatedOutputID as NVARCHAR);