--exec Sch_CartManagement.spDeleteItemFromCart 3, 28;
select max(c.cartID), count(c.cartID) from T_Cart c where c.OrderID = 1 and c.ProductID = 28;
select p.ProductName, c.* from T_Cart c  join T_Products p
on c.ProductID = p.ProductID
where c.OrderID = 1 and c.ProductID = 28;

--select * from T_Cart
--select * from T_Orders

