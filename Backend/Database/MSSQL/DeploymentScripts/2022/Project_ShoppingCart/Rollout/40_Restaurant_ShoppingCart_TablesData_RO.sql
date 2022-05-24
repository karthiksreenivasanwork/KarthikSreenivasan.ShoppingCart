use ShoppingCartDB;
:setvar path "KarthikSreenivasan.ShoppingCart\Backend\Database\MSSQL\DeploymentScripts\2022\Project_ShoppingCart\Rollout\"

-- It is mandatory to stage the lookup data for the below lookup tables to work correctly.
:r $(path)Insert_Into_T_LU_ProductCategories.sql
:r $(path)Insert_Into_T_LU_OrdersStatus.sql

-- The below scripts hold optional data that gets populated organically while the application is deployed and used.
--:r $(path)Insert_Into_T_Users.sql --Dummy user data for creating a dummy order.
--:r $(path)Insert_Into_T_Products.sql --Removing the staging of these products as they will be added from the Angular app.
--:r $(path)Insert_Into_T_Orders.sql
--:r $(path)Insert_Into_T_Cart.sql