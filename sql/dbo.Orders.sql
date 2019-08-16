USE [OrdersRead]
GO

/****** Object: Table [dbo].[Orders] Script Date: 14/08/2019 11:45:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Orders] (
    [Id]       UNIQUEIDENTIFIER NOT NULL,
    [UserId]   UNIQUEIDENTIFIER NOT NULL,
    [PlacedOn] DATETIME2 (7)    NOT NULL,
	[Status]	VARCHAR(20)		NOT NULL,
	[Comment]	NVARCHAR(128)	NULL,
    [Version]  BIGINT           NOT NULL
);


