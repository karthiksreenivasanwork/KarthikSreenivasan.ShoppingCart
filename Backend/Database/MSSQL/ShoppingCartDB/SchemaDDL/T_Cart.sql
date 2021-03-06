use ShoppingCartDB;

/*
 Create a cart table to track user's purchase.
 */
CREATE TABLE T_Cart(
    /*
     * MAXIMUM OF 6 DIGITS
     * PRIMARY KEY CANNOT HAVE NULL VALUES
     * AUTO INCREMENT BY 1
     */
	CartID NUMERIC(6,0) IDENTITY(1,1) NOT NULL,
	ProductID NUMERIC(6,0),
	OrderID NUMERIC(6,0),

	PRIMARY KEY (CartID),
	FOREIGN KEY (ProductID) REFERENCES T_Products (ProductID) ON DELETE CASCADE,
	FOREIGN KEY (OrderID) REFERENCES T_Orders (OrderID) --Will be updated on checkout
);