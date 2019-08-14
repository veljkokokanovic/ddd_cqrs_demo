USE [DeliveryRead]
GO

/****** Object: Table [dbo].[Deliveries] Script Date: 14/08/2019 11:46:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Deliveries] (
    [Id]          UNIQUEIDENTIFIER NOT NULL,
    [Address]     NVARCHAR (256)   NOT NULL,
    [PhoneNumber] VARCHAR (20)     NOT NULL,
    [Status]      VARCHAR (20)     NOT NULL
);


