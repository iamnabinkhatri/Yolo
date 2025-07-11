CREATE SCHEMA [yolo];
GO

CREATE TABLE [yolo].[userRole](
    [id] INTEGER IDENTITY,
    [roleType] VARCHAR(50) NOT NULL,
    CONSTRAINT yolo_usr_pk PRIMARY KEY([id]),
);
GO

ALTER TABLE [yolo].[userRole] ADD CONSTRAINT yolo_usrl_unq UNIQUE([roleType]);
GO

CREATE TABLE [yolo].[user](
    [id] INTEGER IDENTITY,
    [email] VARCHAR(100) NOT NULL,
    [password] NVARCHAR(500) NOT NULL,
    [username] VARCHAR(50) NOT NULL,
    [firstName] VARCHAR(50) NOT NULL,
    [middleName] VARCHAR(50),
    [lastName] VARCHAR(50) NOT NULL,
    [phoneNo] INTEGER NOT NULL,
    [city] VARCHAR(50) NOT NULL,
    [zipCode] INTEGER NOT NULL,
    [state] VARCHAR(50) NOT NULL,
    [country] VARCHAR(50) NOT NULL,
    [roleId] Integer NOT NULL,
    CONSTRAINT yolo_usr_pkid PRIMARY KEY([id]),
    CONSTRAINT yolo_usr_unq UNIQUE([username]),
    CONSTRAINT yolo_usr_fk FOREIGN KEY(roleId) REFERENCES [yolo].[userRole](id)
);
GO

CREATE TABLE [yolo].[player](
    [id] INTEGER IDENTITY,
    [userId] INTEGER NOT NULL,
    [nickname] VARCHAR(100),
    [playerNumber] INTEGER NOT NULL,
    CONSTRAINT yolo_plyrId_pk PRIMARY KEY ([id]),
    CONSTRAINT yolo_plyr_fk FOREIGN KEY ([userId]) REFERENCES [yolo].[user](id)
);
GO

ALTER TABLE [yolo].[player] ADD CONSTRAINT yolo_plyr_unq UNIQUE([userId]);
GO

CREATE TABLE [yolo].[playerStatics](
    [id] INTEGER IDENTITY,
    [playerId] INTEGER NOT NULL,
    [goals] INTEGER,
    [assists] INTEGER,
    [save] INTEGER,
    [attendance] CHAR(3) NOT NULL,
    CONSTRAINT yolo_plyr_st_pkid PRIMARY KEY ([id]),
    CONSTRAINT yolo_plyr_st_fk FOREIGN KEY ([playerId]) REFERENCES [yolo].[player](id),
    CONSTRAINT yolo_plyr_st_chk CHECK ([attendance] IN ('YES', 'NO'))
);
GO

CREATE TABLE [yolo].[playerRole](
    [id] INTEGER IDENTITY,
    [playerRole] VARCHAR(50) NOT NULL,
    CONSTRAINT yolo_plyr_rl_pkid PRIMARY KEY ([id])
);
GO

CREATE TABLE [yolo].[poll](
    [id] INTEGER IDENTITY,
    [title] NVARCHAR(255) NOT NULL,
    [description] NVARCHAR(MAX) ,
    [created_by] INTEGER NOT NULL, -- created by which user/player
    [created_at] DATETIME DEFAULT GETDATE(),
    [close_at] DATETIME,
    [is_closed] CHAR(1) DEFAULT('N'),
    [latitude] DECIMAL(9,6),
    [longitude] DECIMAL(9,6),
    [is_shareable] CHAR(1) DEFAULT ('Y')
    CONSTRAINT yolo_poll_fk FOREIGN KEY([created_by]) REFERENCES [yolo].[user](id),
    CONSTRAINT yolo_poll_pkid PRIMARY KEY ([id]),
    CONSTRAINT yolo_poll_close_chk CHECK ([is_closed] IN ('Y', 'N')),
    CONSTRAINT yolo_poll_shareable_chk CHECK ([is_shareable] IN ('Y', 'N'))
);
GO

CREATE TABLE [yolo].[pollOption](
    [id] INTEGER IDENTITY,
    [pollId] INTEGER NOT NULL,
    [option] NVARCHAR(255) NOT NULL,
    CONSTRAINT yolo_poll_option_pkid PRIMARY KEY ([id]),
    CONSTRAINT yolo_poll_option_fk FOREIGN KEY ([pollId]) REFERENCES [yolo].[poll](id) 
);
GO

CREATE TABLE [yolo].[vote](
    [id] INTEGER IDENTITY,
    [pollId] INTEGER NOT NULL,
    [optionId] INTEGER NOT NULL,
    [userId] INTEGER NOT NULL,
    [voted_at] DATETIME DEFAULT GETDATE(),
    CONSTRAINT yolo_vote_pkid PRIMARY KEY ([id]),
    CONSTRAINT yolo_vote_fk_poll FOREIGN KEY ([pollId]) REFERENCES [yolo].[poll](id),
    CONSTRAINT yolo_vote_fk_option FOREIGN KEY ([optionId]) REFERENCES [yolo].[pollOption](id),
    CONSTRAINT yolo_vote_fk_usr FOREIGN KEY ([userId]) REFERENCES [yolo].[user](id),
    CONSTRAINT yolo_vote_unq_users UNIQUE ([pollId],[userId])
);
GO

ALTER TABLE [yolo].[user] ADD CONSTRAINT yolo_u_unq UNIQUE([username]);
GO

ALTER TABLE [yolo].[user] ALTER COLUMN password NVARCHAR(500) NOT NULL;
GO

Drop TABLE [yolo].[user];
GO

