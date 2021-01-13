DROP DATABASE IF EXISTS [aspnet5-Benchmarks];
CREATE DATABASE [aspnet5-Benchmarks];
GO

USE [aspnet5-Benchmarks];
GO

DROP TABLE IF EXISTS [world];
CREATE TABLE  [world] (
  [id] INT IDENTITY (1, 1) NOT NULL,
  [randomNumber] INT NOT NULL DEFAULT 0,
  PRIMARY KEY  (id)
)
GO

DECLARE @cnt INT = 0;
DECLARE @max INT = 10000;

WHILE @cnt < @max
BEGIN
	INSERT INTO [world] ([randomNumber]) VALUES ( ABS(CHECKSUM(NewId())) % 10000 );
	SET @cnt = @cnt + 1;
END;
GO


