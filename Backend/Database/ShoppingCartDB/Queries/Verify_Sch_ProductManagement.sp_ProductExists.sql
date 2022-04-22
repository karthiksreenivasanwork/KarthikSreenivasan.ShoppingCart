/*
Execute the stored procedure sp_ProductExists to verify if it already exists in the database.
*/
DECLARE @ProductSearchCountOutputVariable INT
EXEC Sch_ProductManagement.sp_ProductExists 'Pepsi', @ProductSearchCountOutputParam = @ProductSearchCountOutputVariable output
/*
Result Description
------------------
1 - Indicates that the data search in the database was successful and 0 otherwise
*/
PRINT @ProductSearchCountOutputVariable; 