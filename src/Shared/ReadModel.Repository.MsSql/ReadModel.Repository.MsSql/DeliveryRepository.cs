using Dapper;
using Microsoft.Extensions.Configuration;
using ReadModel.Delivery;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace ReadModel.Repository.MsSql
{
    public class DeliveryRepository : RepositoryBase<Delivery.Order, Guid>, IDeliveryRepository
    {
        public DeliveryRepository(IConfiguration config) : base(config)
        {
        }

        public override async Task<Delivery.Order> GetAsync(Guid id)
        {
            var sql = "SELECT * FROM Deliveries where Id = @id";

            using (var connection = new SqlConnection(Configuration[Repository.Configuration.DeliveryReadModelConnectionString]))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                var order = await connection.QuerySingleOrDefaultAsync(sql, new { id }).ConfigureAwait(false);
                return order != null ? DynamicToOrder(order) : null;
            }
        }

        public override async Task<IQueryable<Delivery.Order>> GetAsync(params Guid[] id)
        {
            throw new NotImplementedException();
        }

        public override async Task SaveAsync(Delivery.Order order)
        {
            var deletesql = "DELETE FROM Deliveries WHERE Id = @id;";
            var orderSql = "INSERT INTO Deliveries " +
                           "([Id],[OrderId],[AddressLine1],[AddressLine2],[PostCode],[PhoneNumber],[UserId],[PlacedOn],[StartedAt],[DeliveredAt],[DeliveryDate],[Status],[Version]) " +
                           "VALUES (@Id,@OrderId,@AddressLine1,@AddressLine2,@PostCode,@PhoneNumber,@UserId,@PlacedOn,@StartedAt,@DeliveredAt,@DeliveryDate,@Status,@Version);";

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            { 
                using (var connection = new SqlConnection(Configuration[Repository.Configuration.DeliveryReadModelConnectionString]))
                {
                    await connection.ExecuteAsync(deletesql, new {id = order.Id}).ConfigureAwait(false);
                    await connection.ExecuteAsync(
                            orderSql, 
                            new
                            {
                                order.Id,
                                OrderId = order.ReferenceOrderId,
                                AddressLine1 = order.DeliveryAddress.Line1,
                                AddressLine2 = order.DeliveryAddress.Line2,
                                order.DeliveryAddress.PostCode,
                                order.PhoneNumber,
                                order.UserId,
                                order.PlacedOn,
                                StartedAt = order.DeliveryStartedAt,
                                order.DeliveredAt,
                                order.DeliveryDate,
                                Status = order.Status.ToString(),
                                order.Version
                            })
                        .ConfigureAwait(false);
                }
                transaction.Complete();
            }
        }

        public override async Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public override async Task<IQueryable<Delivery.Order>> FindAsync(Func<Delivery.Order, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Delivery.Order>> GetFromOrderIdsAsync(params Guid[] orderIds)
        {
            var sql = "SELECT * FROM Deliveries where OrderId IN @ids";
            using (var connection = new SqlConnection(Configuration[Repository.Configuration.DeliveryReadModelConnectionString]))
            {
                await connection.OpenAsync().ConfigureAwait(false);
                var orders = await connection.QueryAsync(sql, new {ids = orderIds});
                return new List<Delivery.Order>(orders.Select(DynamicToOrder));
            }
        }

        private static Delivery.Order DynamicToOrder(dynamic order)
        {
            return new Delivery.Order
            {
                UserId = order.UserId,
                Id = order.Id,
                DeliveryAddress = new Address
                {
                    PostCode = order.PostCode,
                    Line2 = order.AddressLine2,
                    Line1 = order.AddressLine1,
                },
                Version = order.Version,
                PhoneNumber = order.PhoneNumber,
                DeliveryDate = order.DeliveryDate,
                Status = order.Status,
                PlacedOn = order.PlacedOn,
                DeliveredAt = order.DeliveredAt,
                DeliveryStartedAt = order.StartedAt,
                ReferenceOrderId = order.OrderId
            };
        }
    }
}
