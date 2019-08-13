using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReadModel.Repository.MsSql
{
    public interface IOrderRepository : IReadModelRepository<Order.Order, Guid>
    {
        Task<IEnumerable<Order.Order>> GetUserOrdersAsync(Guid userId);
    }
}
