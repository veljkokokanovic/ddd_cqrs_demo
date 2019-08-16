using AutoMapper;
using Delivery.Commands;
using UI.Gateway.Models.Delivery.Commands;

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
