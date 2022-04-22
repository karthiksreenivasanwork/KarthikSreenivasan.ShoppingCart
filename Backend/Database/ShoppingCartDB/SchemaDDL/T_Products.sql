IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES AS I WHERE I.TABLE_NAME = 'T_Products')
	DROP TABLE T_Products;
GO
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES AS I WHERE I.TABLE_NAME = 'T_LU_ProductCategories')
BEGIN
/*
 Create a products table
 */
CREATE TABLE T_Products(
    /*
     * MAXIMUM OF 6 DIGITS
     * PRIMARY KEY CANNOT HAVE NULL VALUES
     * AUTO INCREMENT BY 1
     */
	ProductID NUMERIC(6,0) IDENTITY(1,1) NOT NULL,
	ProductCategoryID NUMERIC(6,0),
	ProductName VARCHAR(200) NOT NULL UNIQUE,
	ProductPrice NUMERIC(6, 0),
	ProductDescription VARCHAR(MAX),
	ProductImageName VARCHAR(200)

	PRIMARY KEY (ProductID)
	FOREIGN KEY (ProductCategoryID) REFERENCES T_LU_ProductCategories (ProductCategoryID)
);
END
ELSE
PRINT 'Please create the reference table `T_LU_ProductCategories` before creating T_Products';
GO