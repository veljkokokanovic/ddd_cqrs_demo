using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ReadModel.Order;
using System.Transactions;
using Dapper.Contrib.Extensions;

namespace ReadModel.Repository.MsSql
{
    public class OrderRepository : RepositoryBase<Order.Order, Guid>, IOrderRepository
    {
        public OrderRepository(IConfiguration config) : base(config)
        {
        }

        public override async Task<Order.Order> GetAsync(Guid id)
        {
            var sql = "SELECT * FROM OrderItems where OrderId = @orderId";

            using (var connection = new SqlConnection(Configuration[Repository.Configuration.OrderReadModelConnectionString]))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                var order = await connection.GetAsync<Order.Order>(id).ConfigureAwait(false);
                if (order != null)
                {
                    var items = await connection.QueryAsync(sql, new { OrderId = id }).ConfigureAwait(false);
                    order.Products = items.Select(i => new OrderItem
                    {
                        Id = new OrderItemIdentity { OrderId = i.OrderId, Sku = i.Sku },
                        Quantity = i.Quantity,
                        Price = i.Price
                    }).ToList();
                }
                return order;
            }
        }

        public override async Task<IQueryable<Order.Order>> GetAsync(params Guid[] id)
        {
            throw new NotImplementedException();
        }

        public override async Task SaveAsync(Order.Order order)
        {
            var deletesql = "DELETE FROM OrderItems WHERE OrderId = @OrderId; DELETE FROM Orders WHERE Id = @OrderId;";
            var orderSql = "INSERT INTO Orders (Id, UserId, PlacedOn, [Status], Version) VALUES (@Id, @UserId, @PlacedOn, @Status, @Version);";
            var orderItemSql = "INSERT INTO OrderItems(OrderId, Sku, Quantity, Price) VALUES (@OrderId, @Sku, @Quantity, @Price);";

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            { 
                using (var connection = new SqlConnection(Configuration[Repository.Configuration.OrderReadModelConnectionString]))
                {
                    await connection.ExecuteAsync(deletesql, new {OrderId = order.Id}).ConfigureAwait(false);
                    await connection. ExecuteAsync(orderSql, new {order.Id, order.UserId, order.PlacedOn, Status = order.Status.ToString(), order.Version})
                        .ConfigureAwait(false);
                    var products = order.Products.Select(p => new {p.Id.OrderId, p.Id.Sku, p.Quantity, p.Price});
                    await connection.ExecuteAsync(orderItemSql, products).ConfigureAwait(false);
                }
                transaction.Complete();
            }
        }

        public override async Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override async Task<IQueryable<Order.Order>> FindAsync(Func<Order.Order, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Order.Order>> GetUserOrdersAsync(Guid userId)
        {
            var sqlOrders = "SELECT * FROM Orders where UserId = @userId;";
            var sqlItems =
                "SELECT oi.* FROM OrderItems oi JOIN Orders o ON oi.OrderId = o.Id WHERE o.UserId = @userId;";

            using (var connection = new SqlConnection(Configuration[Repository.Configuration.OrderReadModelConnectionString]))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                var orders = await connection.QueryAsync<Order.Order>(sqlOrders, new{ userId }).ConfigureAwait(false);
                var itemsD = await connection.QueryAsync(sqlItems, new { userId }).ConfigureAwait(false);
                var items = itemsD.Select(i => new OrderItem
                    {
                        Id = new OrderItemIdentity {OrderId = i.OrderId, Sku = i.Sku},
                        Quantity = i.Quantity,
                        Price = i.Price
                    })
                    .GroupBy(k => k.Id.OrderId)
                    .ToDictionary(k => k.Key, v => v.Select(a => a));

                return orders.Select(o =>
                {
                    o.Products = items.ContainsKey(o.Id) ? items[o.Id].ToList() : new List<OrderItem>();
                    return o;
                });
            }
        }
    }
}
