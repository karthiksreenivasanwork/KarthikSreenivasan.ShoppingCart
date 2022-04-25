using ShoppingCartDB; 

/*
	This returns the list of products for specific user (ID = 1) who has not completed
	the checkout.
*/
select u.UserID, c.CartID, o.OrderID, p.Productname, p.ProductPrice
from T_Products as p
join T_Cart as c
on c.ProductID = p.ProductID
join T_Orders as o
on c.OrderID = o.OrderID
join T_Users u
on u.UserID = o.UserID
where o.OrderPurchaseState in
(select os.OrderPurchaseState from T_LU_OrdersStatus as os where os.OrderStateDescription = 'In Progress')
and u.UserID = 1;