ALTER TABLE T_Cart
ADD CONSTRAINT FK_T_PRODUCTS_T_Cart_CASCADE
FOREIGN KEY (ProductID) REFERENCES T_Products (ProductID) ON DELETE CASCADE;