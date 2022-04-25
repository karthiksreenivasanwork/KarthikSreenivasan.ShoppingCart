use ShoppingCartDB;

/*
 Products that are checked out from the cart will be associated with an Order.
 This table helps track each order of a customer.
 */
CREATE TABLE T_Orders(
    /*
     * MAXIMUM OF 6 DIGITS
     * PRIMARY KEY CANNOT HAVE NULL VALUES
     * AUTO INCREMENT BY 1
     */
	OrderID NUMERIC(6,0) IDENTITY(1,1) NOT NULL,
	UserID NUMERIC(6,0),
	OrderPurchaseState NUMERIC(2,0),
	OrderDate DATETIME,

    PRIMARY KEY (OrderID),
	FOREIGN KEY (UserID) REFERENCES T_Users (UserID),
	FOREIGN KEY (OrderPurchaseState) REFERENCES T_LU_OrdersStatus (OrderPurchaseState)
);