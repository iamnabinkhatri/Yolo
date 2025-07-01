CREATE SCHEMA [yolo];
GO

CREATE TABLE [yolo].[user](
    [id] INTEGER IDENTITY,
    [email] VARCHAR(100) NOT NULL,
    [password] VARCHAR(50) NOT NULL,
    [username] VARCHAR(50) NOT NULL,
    [firstName] VARCHAR(50) NOT NULL,
    [middleName] VARCHAR(50),
    [lastName] VARCHAR(50) NOT NULL,
    [phoneNo] INTEGER NOT NULL,
    [city] VARCHAR(50) NOT NULL,
    [zipCode] INTEGER NOT NULL,
    [state] VARCHAR(50) NOT NULL,
    [country] VARCHAR(50) NOT NULL,
    CONSTRAINT yolo_upk_id PRIMARY KEY([id])
);
GO

ALTER TABLE [yolo].[user] ADD CONSTRAINT yolo_u_unq UNIQUE([username]);
GO
\
ALTER TABLE [yolo].[user] ALTER COLUMN password NVARCHAR(500) NOT NULL;
GO

Drop TABLE [yolo].[user];
GO

