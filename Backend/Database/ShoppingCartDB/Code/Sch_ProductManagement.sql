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
@ProductImageParam VARCHAR(200) AS
BEGIN
INSERT INTO T_Products (ProductCategoryID, ProductName, ProductPrice, ProductDescription, ProductImage)
VALUES (
		(SELECT lpc.ProductCategoryID FROM T_LU_ProductCategories lpc where lpc.ProductCategoryName = @ProductCategoryNameParam),
		@ProductNameParam,
		@ProductPriceParam,
		@ProductDescriptionParam,
		@ProductImageParam
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
	   p.ProductImage FROM T_Products as p
	   join T_LU_ProductCategories as pc
	   on p.ProductCategoryID = pc.ProductCategoryID
	   order by p.ProductCategoryID;
END
GO