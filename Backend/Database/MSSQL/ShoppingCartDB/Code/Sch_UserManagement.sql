IF NOT EXISTS(SELECT NAME FROM SYS.SCHEMAS WHERE NAME = 'Sch_UserManagement')
BEGIN
	EXEC('CREATE SCHEMA Sch_UserManagement')
	PRINT 'Schema ''Sch_UserManagement'' created.'
END
ELSE
BEGIN
	PRINT 'Schema ''Sch_UserManagement'' schema already exists.'
END
GO

CREATE OR ALTER PROCEDURE Sch_UserManagement.sp_CreateUser
@UsernameParam VARCHAR(200),
@PasswordParam VARCHAR(200),
@EmailParam varchar(200),
@PhoneParam varchar(20),
@UserIDOutputParam NUMERIC(6, 0) OUTPUT
AS
BEGIN
INSERT INTO T_Users (Username, Password, Email, Phone)
VALUES (
		@UsernameParam,
		@PasswordParam,
		@EmailParam,
		@PhoneParam
	);
SELECT @UserIDOutputParam = SCOPE_IDENTITY();
END
GO

CREATE OR ALTER PROCEDURE Sch_UserManagement.sp_ReturnHashedPasswordOfRegisteredUser
@UsernameInputParam VARCHAR(200),
@HashedPasswordOutputParam VARCHAR(200) OUTPUT
AS
BEGIN
SELECT @HashedPasswordOutputParam = u.Password
FROM T_Users u 
where u.Username = @UsernameInputParam;
END
GO

CREATE OR ALTER PROCEDURE Sch_UserManagement.sp_UserExists
@UsernameInputParam VARCHAR(200),
@UserSearchCountOutputParam int OUTPUT 
AS
BEGIN
SELECT @UserSearchCountOutputParam = COUNT(*) 
FROM T_Users u 
where u.Username = @UsernameInputParam;
END
GO

PRINT 'Script to create stored procedures for the schema `Sch_UserManagement` completed.'