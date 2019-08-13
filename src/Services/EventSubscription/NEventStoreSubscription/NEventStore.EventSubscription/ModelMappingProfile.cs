using System;
using System.Collections.Generic;
using System.Text;
using Order.Events;
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

        }
    }
}
