use ShoppingCartDB;
:setvar path "D:\GitLab\karthiksreenivasanwork\Repos\KarthikSreenivasan.ShoppingCart\Backend\Database\MSSQL\ShoppingCartDB\SchemaDDL\"

:r $(path)T_Users.sql

:r $(path)T_LU_ProductCategories.sql
:r $(path)T_Products.sql

:r $(path)T_LU_OrdersStatus.sql
:r $(path)T_Orders.sql

:r $(path)T_Cart.sql