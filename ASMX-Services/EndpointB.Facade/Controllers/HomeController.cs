using System;
using System.Threading.Tasks;
using System.Web.Http;
using EndpointB.Receiver.Messages.Commands;
using NServiceBus;

namespace EndpointB.Facade.Controllers
{
    public class HomeController : ApiController
    {
        private readonly IMessageSession _messageSession;

        public HomeController(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        public async Task Post(Guid orderId)
        {
            var msg = new DoY { OrderId = orderId };

            await _messageSession.Send(msg);
        }

        public async Task Get(Guid orderId)
        {
            var msg = new VerifyY { OrderId = orderId };

            await _messageSession.Send(msg);
        }
    }
}
