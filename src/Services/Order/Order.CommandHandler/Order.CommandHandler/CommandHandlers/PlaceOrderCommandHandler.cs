﻿using System;
using System.Linq;
using domainD;
using Order.Commands;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using SharedKernel;

namespace Order.CommandHandler.CommandHandlers
{
    public class PlaceOrderCommandHandler : ICommandHandler<PlaceOrder>
    {
        private readonly IRepository<Domain.Order> _repository;
        private readonly ILogger _logger;

        public PlaceOrderCommandHandler(IRepository<Domain.Order> repository, ILogger<PlaceOrderCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(PlaceOrder command)
        {
            var order = await _repository.GetAsync(command.OrderId).ConfigureAwait(false);
            var delivery = new Delivery(command.Address.ToAddress(), command.DeliveryDate, command.PhoneNumber);
            order.Place(delivery);
            await _repository.SaveAsync(order).ConfigureAwait(false);
            _logger.LogInformation("Command {command} for order {orderId} succesfully processed", command.GetType(), command.OrderId);
        }
    }
}
