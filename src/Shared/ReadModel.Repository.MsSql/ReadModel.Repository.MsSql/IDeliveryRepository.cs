using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadModel.Repository.MsSql
{
    public interface IDeliveryRepository : IReadModelRepository<Delivery.Order, Guid>
    {
        Task<IEnumerable<Delivery.Order>> GetFromOrderIdsAsync(params Guid[] orderIds);
    }
}
