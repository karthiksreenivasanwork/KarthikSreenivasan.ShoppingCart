use ShoppingCartDB;

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
	ProductCategoryName NVARCHAR(200) NOT NULL UNIQUE,

	PRIMARY KEY (ProductCategoryID)
);