USE [DeliveryRead]
GO

/****** Object: Table [dbo].[Deliveries] Script Date: 14/08/2019 11:46:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Deliveries] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
	[OrderId]          UNIQUEIDENTIFIER NOT NULL,
    [AddressLine1]     NVARCHAR (256)   NOT NULL,
	[AddressLine2]     NVARCHAR (256)   NOT NULL,
	[PostCode]     NVARCHAR (20)   NOT NULL,
    [PhoneNumber] VARCHAR (20)     NOT NULL,
    [UserId]   UNIQUEIDENTIFIER NOT NULL,
    [PlacedOn] DATETIME2 (7)    NOT NULL,
	[StartedAt] DATETIME2 (7)    NULL,
	[DeliveredAt] DATETIME2 (7)     NULL,
	[DeliveryDate] DATETIME2 (7)    NOT NULL,
	[Status]	VARCHAR(20)		NOT NULL,
    [Version]  BIGINT           NOT NULL

);


