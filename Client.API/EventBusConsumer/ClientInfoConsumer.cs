using Amazon.Runtime.Internal.Util;
using Common;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Client.API.EventBusConsumer
{
    public class ClientInfoConsumer : IConsumer<ClientEvent>
    {
        private readonly ILogger<ClientInfoConsumer> _logger;
        private readonly EncryptionService _encryptionService;

        public ClientInfoConsumer(ILogger<ClientInfoConsumer> logger, EncryptionService encryptionService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
        }

        public async Task Consume(ConsumeContext<ClientEvent> context)
        {
            var name = _encryptionService.DecryptString(context.Message.Name ?? throw new ArgumentNullException(nameof(context)));
            _logger.LogInformation("ClientInfoConsumer consumed successfully. Client Name: {0}", name);
        }
    }
}
