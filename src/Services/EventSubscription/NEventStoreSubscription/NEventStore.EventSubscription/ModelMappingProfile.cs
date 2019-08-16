using System;
using System.Collections.Generic;
using System.Text;
using Order.Events;
using ReadModel.Delivery;
using ReadModel.Order;

namespace NEventStore.EventSubscription
{
    public class ModelMappingProfile : AutoMapper.Profile
    {
        public ModelMappingProfile()
        {
            CreateMap<ProductAddedToOrder, OrderItemIdentity>()
                .ForMember(d => d.OrderId, o => o.MapFrom(s => s.AggregateRootId));
            CreateMap<ProductAddedToOrder, OrderItem>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s));

            CreateMap<ProductAddedToOrder, ReadModel.Order.Order>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.AggregateRootId))
                .ForMember(d => d.PlacedOn, o => o.MapFrom(s => s.CreatedOn))
                .ForMember(d => d.Products,
                    o => o.MapFrom((source, dest, res, ctx) =>
                        new List<OrderItem> {ctx.Mapper.Map<OrderItem>(source)}));

            CreateMap<Delivery.Events.OrderPlaced, ReadModel.Delivery.Order>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.AggregateRootId))
                .ForMember(d => d.PlacedOn, o => o.MapFrom(s => s.CreatedOn))
                .ForMember(d => d.DeliveredAt, o => o.Ignore())
                .ForMember(d => d.DeliveredAt, o => o.Ignore())
                .ForMember(d => d.DeliveryAddress, o => o.MapFrom(s => s.DeliveryInfo.DeliveryAddress))
                .ForMember(d => d.DeliveryDate, o => o.MapFrom(s => s.DeliveryInfo.DeliveryDate))
                .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.DeliveryInfo.PhoneNumber))
                .ForMember(d => d.Status, o => o.MapFrom(s => DeliveryStatus.Placed));

            CreateMap<SharedKernel.Address, Address>();
        }
    }
}
