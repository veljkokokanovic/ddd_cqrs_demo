USE [DeliveriesRead]
GO
DELETE FROM Deliveries

USE [OrdersRead]
GO
DELETE FROM OrderItems
DELETE FROM Orders

USE  [OrderSagas]
GO
DELETE FROM SagaInstance

USE [EventStore]
GO
DELETE FROM Commits