/*
 Insert data into T_Cart
 
 Note: To know which user's cart each cart item belong to
 can be found in th orders table.
 */
 exec Sch_CartManagement.sp_CreateCartItems 1, 1; --Product ID, Order ID
 exec Sch_CartManagement.sp_CreateCartItems 5, 1;