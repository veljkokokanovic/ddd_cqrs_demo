using AutoMapper;
using Order.Commands;
using UI.Gateway.Models.Order.Commands;

namespace UI.Gateway.Models.Order
{
    public class ModelMapper : Profile
    {
        public ModelMapper()
        {
            CreateMap<AddProductToOrderViewModel, AddProductToOrder>();
            CreateMap<CancelOrderViewModel, CancelOrder>();
            CreateMap<PlaceOrderViewModel, PlaceOrder>();
            CreateMap<RemoveProductViewModel, RemoveProduct>();
            CreateMap<SetProductQuantityViewModel, SetProductQuantity>();
        }
    }
}
