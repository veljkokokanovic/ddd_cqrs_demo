USE [OrdersRead]
GO

/****** Object: Table [dbo].[OrderItems] Script Date: 14/08/2019 11:46:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderItems] (
    [OrderId]  UNIQUEIDENTIFIER NOT NULL,
    [Sku]      VARCHAR (50)     NOT NULL,
    [Quantity] INT              NOT NULL,
    [Price]    DECIMAL (18, 2)  NULL
);


