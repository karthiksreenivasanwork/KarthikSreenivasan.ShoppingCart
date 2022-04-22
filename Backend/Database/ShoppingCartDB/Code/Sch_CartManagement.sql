IF NOT EXISTS (SELECT S.NAME FROM SYS.SCHEMAS S WHERE S.NAME = 'Sch_CartManagement')
BEGIN
	EXEC('CREATE SCHEMA Sch_CartManagement')
	PRINT 'SCHEMA ''Sch_CartManagement'' CREATED';
END
ELSE
BEGIN
	PRINT 'SCHEMA ''Sch_CartManagement'' already exists'; 
END
GO