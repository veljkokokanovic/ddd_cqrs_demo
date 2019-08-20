USE EventStore
SELECT * FROM Commits

USE OrdersRead
SELECT * FROM Orders
SELECT * FROM OrderItems

USE DeliveriesRead
SELECT * FROM Deliveries

USE OrderSagas
SELECT * FROM SagaInstance