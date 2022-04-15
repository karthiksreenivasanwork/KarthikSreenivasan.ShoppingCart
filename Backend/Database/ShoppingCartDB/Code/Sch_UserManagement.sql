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
@PhoneParam varchar(20) AS
BEGIN
INSERT INTO T_Users (Username, Password, Email, Phone)
VALUES (
		@UsernameParam,
		@PasswordParam,
		@EmailParam,
		@PhoneParam
	);
END
GO
PRINT 'Stored procedure ''Sch_UserManagement.sp_CreateUser'' created or altered.'