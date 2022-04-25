use ShoppingCartDB;
:setvar path "D:\GitLab\karthiksreenivasanwork\Repos\KarthikSreenivasan.ShoppingCart\Backend\Database\DeploymentScripts\2022\Project_ShoppingCart\Rollout\"

:r $(path)Insert_Into_T_Users.sql --Dummy user data for creating a dummy order.

:r $(path)Insert_Into_T_LU_ProductCategories.sql
:r $(path)Insert_Into_T_Products.sql

:r $(path)Insert_Into_T_LU_OrdersStatus.sql
:r $(path)Insert_Into_T_Orders.sql
:r $(path)Insert_Into_T_Cart.sql