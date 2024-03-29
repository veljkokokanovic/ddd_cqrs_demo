﻿using domainD;
using Order.Commands;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Order.CommandHandler.CommandHandlers
{
    public class CancelOrderCommandHandler : ICommandHandler<CancelOrder>
    {
        private readonly IRepository<Domain.Order> _repository;
        private readonly ILogger _logger;

        public CancelOrderCommandHandler(IRepository<Domain.Order> repository, ILogger<CancelOrderCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task HandleAsync(CancelOrder command)
        {
            var order = await _repository.GetAsync(command.OrderId).ConfigureAwait(false);
            order.Cancel();
            await _repository.SaveAsync(order).ConfigureAwait(false);
            _logger.LogInformation("Command {command} for order {orderId} succesfully processed", command.GetType(), command.OrderId);
        }
    }
}
