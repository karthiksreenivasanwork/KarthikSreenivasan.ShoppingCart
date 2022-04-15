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
    Username VARCHAR(200) NOT NULL,
    Password VARCHAR(200) NOT NULL,
    Email VARCHAR(200),
    Phone VARCHAR(20) NOT NULL
	PRIMARY KEY(UserID)
);