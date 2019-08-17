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

            CreateMap<ReadModel.Order.Order, OrderViewModel>()
                .ForMember(d => d.OrderId, o => o.MapFrom(s => s.Id));

            CreateMap<ReadModel.Order.OrderItem, OrderItemViewModel>()
                .ForMember(d => d.Sku, o => o.MapFrom(s => s.Id.Sku));

            CreateMap<ReadModel.Delivery.Order, OrderViewModel>()
                .ForMember(d => d.DeliveryAddress, o => o.MapFrom(s => s.ToString()))
                .ForMember(d => d.Status, o => o.MapFrom((s, d, m, ctx) => {
                    switch(s.Status)
                    {
                        case ReadModel.Delivery.DeliveryStatus.Delivered:
                            return OrderStatus.Delivered;
                        case ReadModel.Delivery.DeliveryStatus.Delivering:
                            return OrderStatus.Delivering;
                        case ReadModel.Delivery.DeliveryStatus.Returned:
                            return OrderStatus.Returned;
                        default:
                            return d.Status;
                    }
                }));
        }
    }
}
