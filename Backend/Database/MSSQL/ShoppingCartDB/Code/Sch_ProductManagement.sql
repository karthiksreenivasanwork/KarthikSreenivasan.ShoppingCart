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
@ProductImageNameParam VARCHAR(200) AS
BEGIN
INSERT INTO T_Products (ProductCategoryID, ProductName, ProductPrice, ProductDescription, ProductImageName)
VALUES (
		(SELECT lpc.ProductCategoryID FROM T_LU_ProductCategories lpc where lpc.ProductCategoryName = @ProductCategoryNameParam),
		@ProductNameParam,
		@ProductPriceParam,
		@ProductDescriptionParam,
		@ProductImageNameParam
	);
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

CREATE OR ALTER PROCEDURE Sch_ProductManagement.sp_ProductExists
@ProductNameInputParam VARCHAR(200),
@ProductSearchCountOutputParam int OUTPUT 
AS
BEGIN
SELECT @ProductSearchCountOutputParam = COUNT(*) 
FROM T_Products p 
where p.ProductName = @ProductNameInputParam;
END
GO

PRINT 'Script to create stored procedures for the schema `Sch_ProductManagement` completed.'