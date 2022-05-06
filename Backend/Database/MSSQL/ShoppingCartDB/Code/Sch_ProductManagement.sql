IF NOT EXISTS(SELECT NAME FROM SYS.SCHEMAS WHERE NAME = 'Sch_ProductManagement')
BEGIN
	EXEC('CREATE SCHEMA Sch_ProductManagement')
	PRINT 'Schema ''Sch_ProductManagement'' created.'
END
ELSE
BEGIN
	PRINT 'Schema ''Sch_ProductManagement'' schema already exists.'
END
GO

CREATE OR ALTER PROCEDURE Sch_ProductManagement.sp_CreateProductCategory
@ProductCategoryNameParam VARCHAR(200)
AS
BEGIN
	IF NOT EXISTS (SELECT ProductCategoryName FROM T_LU_ProductCategories pc
	WHERE pc.ProductCategoryName = @ProductCategoryNameParam)
	BEGIN
		INSERT INTO T_LU_ProductCategories (ProductCategoryName)
		VALUES (@ProductCategoryNameParam);
	END
	ELSE
		PRINT @ProductCategoryNameParam + 'already exists';
END
GO

CREATE OR ALTER PROCEDURE Sch_ProductManagement.sp_GetAllProductCategories
AS
BEGIN
SELECT pc.ProductCategoryID,
	   pc.ProductCategoryName
	   FROM T_LU_ProductCategories as pc order by pc.ProductCategoryID;
END
GO

CREATE OR ALTER PROCEDURE Sch_ProductManagement.sp_CreateProduct
@ProductCategoryNameParam VARCHAR(200),
@ProductNameParam VARCHAR(200),
@ProductPriceParam NUMERIC(6, 0),
@ProductDescriptionParam VARCHAR(MAX),
@ProductImageNameParam VARCHAR(200),
@ProductIDOutputParam NUMERIC(6, 0) OUTPUT
AS
BEGIN
INSERT INTO T_Products (ProductCategoryID, ProductName, ProductPrice, ProductDescription, ProductImageName)
VALUES (
		(SELECT lpc.ProductCategoryID FROM T_LU_ProductCategories lpc where lpc.ProductCategoryName = @ProductCategoryNameParam),
		@ProductNameParam,
		@ProductPriceParam,
		@ProductDescriptionParam,
		@ProductImageNameParam
	);
SELECT @ProductIDOutputParam = SCOPE_IDENTITY()
END
GO

CREATE OR ALTER PROCEDURE Sch_ProductManagement.sp_UpdateProduct
@ProductIDParam NUMERIC(6, 0),
@ProductCategoryNameParam VARCHAR(200),
@ProductPriceParam NUMERIC(6, 0),
@ProductDescriptionParam VARCHAR(MAX)
AS
BEGIN

DECLARE @ProductUpdateTableVariable TABLE (  
	ProductCategoryIDUpdated NUMERIC(6, 0),
	ProductPriceUpdated NUMERIC(6, 0),
	ProductDescriptionUpdated VARCHAR(MAX)
	);  

UPDATE T_Products SET 
ProductCategoryID = (SELECT lpc.ProductCategoryID FROM T_LU_ProductCategories lpc where lpc.ProductCategoryName = @ProductCategoryNameParam),
ProductPrice = @ProductPriceParam,
ProductDescription = @ProductDescriptionParam
OUTPUT 
INSERTED.ProductCategoryID,
INSERTED.ProductPrice,
INSERTED.ProductDescription
INTO @ProductUpdateTableVariable
WHERE ProductID = @ProductIDParam

SELECT * FROM @ProductUpdateTableVariable;
END
GO


CREATE OR ALTER PROCEDURE Sch_ProductManagement.sp_DeleteProduct
@ProductIDParam NUMERIC(6, 0)
AS
BEGIN
DECLARE @ProductDeleteTableVariable TABLE (  
	ProductID NUMERIC(6,0),
	ProductCategoryID NUMERIC(6,0),
	ProductName NVARCHAR(200),
	ProductPrice NUMERIC(6, 0),
	ProductDescription NVARCHAR(MAX),
	ProductImageName NVARCHAR(200)
	);
DELETE FROM T_Products 
OUTPUT
DELETED.*
INTO @ProductDeleteTableVariable
WHERE ProductID = @ProductIDParam;

SELECT * FROM @ProductDeleteTableVariable;
END
GO

CREATE OR ALTER PROCEDURE Sch_ProductManagement.sp_GetAllProducts
AS
BEGIN
SELECT p.ProductID,
	   p.ProductCategoryID,
	   pc.ProductCategoryName,
	   p.ProductName,
	   p.ProductPrice,
	   p.ProductDescription,
	   p.ProductImageName FROM T_Products p
	   join T_LU_ProductCategories pc
	   on p.ProductCategoryID = pc.ProductCategoryID
	   order by p.ProductCategoryID;
END
GO

CREATE OR ALTER PROCEDURE Sch_ProductManagement.sp_GetProductsByCategory
@ProductCategoryIDParam NUMERIC(6, 0)
AS
BEGIN
SELECT p.ProductID,
	   p.ProductCategoryID,
	   pc.ProductCategoryName,
	   p.ProductName,
	   p.ProductPrice,
	   p.ProductDescription,
	   p.ProductImageName FROM T_Products p
	   join T_LU_ProductCategories pc
	   on p.ProductCategoryID = pc.ProductCategoryID
	   where p.ProductCategoryID = @ProductCategoryIDParam;
END
GO

CREATE OR ALTER PROCEDURE Sch_ProductManagement.sp_ProductExistsByName
@ProductNameInputParam VARCHAR(200),
@ProductSearchCountOutputParam int OUTPUT 
AS
BEGIN
SELECT @ProductSearchCountOutputParam = COUNT(*) 
FROM T_Products p 
where p.ProductName = @ProductNameInputParam;
END
GO

CREATE OR ALTER PROCEDURE Sch_ProductManagement.sp_ProductExistsByID
@ProductIDParam NUMERIC(6, 0),
@ProductSearchCountOutputParam int OUTPUT 
AS
BEGIN
SELECT @ProductSearchCountOutputParam = COUNT(*) 
FROM T_Products p 
where p.ProductID = @ProductIDParam;
END
GO

PRINT 'Script to create stored procedures for the schema `Sch_ProductManagement` completed.'