using AutoMapper;
using Delivery.Commands;
using Order.Commands;
using UI.Gateway.Models.Delivery.Commands;
using UI.Gateway.Models.Order.Commands;
using PlaceOrder = Order.Commands.PlaceOrder;

namespace UI.Gateway.Models.Delivery
{
    public class ModelMapper : Profile
    {
        public ModelMapper()
        {
            CreateMap<StartDeliveryViewModel, StartDelivery>();
            CreateMap<ReturnOrderViewModel, ReturnOrder>();
            CreateMap<DeliverOrderViewModel, DeliverOrder>();
        }
    }
}
