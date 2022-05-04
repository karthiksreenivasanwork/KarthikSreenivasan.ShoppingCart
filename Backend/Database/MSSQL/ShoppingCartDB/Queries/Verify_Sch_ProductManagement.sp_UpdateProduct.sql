/*
Execute the stored procedure sp_UpdateProduct to update data of an existing product.
Parameters: Product ID, Category Name, Product Price and Product Description
*/
EXEC Sch_ProductManagement.sp_UpdateProduct 6, 'Households', 1000, 'testing update'; 