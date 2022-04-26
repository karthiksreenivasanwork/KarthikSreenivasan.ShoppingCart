use ShoppingCartDB;

/*
 Create a new users table
 */
CREATE TABLE T_Users(
    /*
     * MAXIMUM OF 6 DIGITS
     * PRIMARY KEY CANNOT HAVE NULL VALUES
     * AUTO INCREMENT BY 1
     */
    UserID NUMERIC(6, 0) IDENTITY(1, 1) NOT NULL,
    Username NVARCHAR(200) NOT NULL UNIQUE,
    Password NVARCHAR(200) NOT NULL,
    Email NVARCHAR(200),
    Phone NVARCHAR(20) NOT NULL
	PRIMARY KEY(UserID)
);