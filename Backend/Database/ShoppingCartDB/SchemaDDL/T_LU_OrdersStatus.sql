IF EXISTS (SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES AS I WHERE I.TABLE_NAME = 'T_LU_OrdersStatus')
	DROP TABLE T_LU_OrdersStatus;
GO

/*
 Products that are checked out from the cart will be associated with an Order.
 This table helps track each order of a customer.
 */
CREATE TABLE T_LU_OrdersStatus(
    /*
     * MAXIMUM OF 6 DIGITS
     * PRIMARY KEY CANNOT HAVE NULL VALUES
     * AUTO INCREMENT BY 1
     */
	OrderLookupID NUMERIC(6,0) IDENTITY(1,1) NOT NULL,
	OrderPurchaseState NUMERIC(2,0) UNIQUE,
	OrderStateDescription VARCHAR(200)
);