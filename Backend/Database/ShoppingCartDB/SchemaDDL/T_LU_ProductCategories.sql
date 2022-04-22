IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES AS I WHERE I.TABLE_NAME = 'T_Products')
	DROP TABLE T_Products;
GO
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES AS I WHERE I.TABLE_NAME = 'T_LU_ProductCategories')
	DROP TABLE T_LU_ProductCategories;
GO

/*
 * Each product belongs to a category.
 * Each category type would be defined in this table.
 */
CREATE TABLE T_LU_ProductCategories(
    /*
     * MAXIMUM OF 6 DIGITS
     * PRIMARY KEY CANNOT HAVE NULL VALUES
     * AUTO INCREMENT BY 1
     */
	ProductCategoryID NUMERIC(6,0) IDENTITY(1,1) NOT NULL,
	ProductCategoryName VARCHAR(200) NOT NULL UNIQUE,

	PRIMARY KEY (ProductCategoryID)
);