/*
 Insert data into T_Orders
 */
 DECLARE @OrderIDOutput INT
 exec Sch_CartManagement.sp_CreateOrReturnExistingOrder 1, @OrderIDOutputParam = @OrderIDOutput OUTPUT
 print 'Order created with Order ID :' + CAST(@OrderIDOutput as NVARCHAR);