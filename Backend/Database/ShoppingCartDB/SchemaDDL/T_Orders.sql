IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES AS I WHERE I.TABLE_NAME = 'T_Cart')
	DROP TABLE  T_Cart; --Foreign key reference must be removed before the main table
GO
IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES AS I WHERE I.TABLE_NAME = 'T_Orders')
	DROP TABLE T_Orders;
GO

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
	CartD NUMERIC(6,0),
	OrderPurchaseState NUMERIC(2,0) UNIQUE,
	OrderDate DATETIME,

    PRIMARY KEY (OrderID),
	FOREIGN KEY (UserID) REFERENCES T_Users (UserID),
	FOREIGN KEY (OrderPurchaseState) REFERENCES T_LU_OrdersStatus (OrderPurchaseState)
);
