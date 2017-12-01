using System;
using System.Threading.Tasks;
using System.Web.Http;
using EndpointB.Receiver.Messages.Commands;
using NServiceBus;

namespace EndpointA.Facade.Controllers
{
    public class HomeController : ApiController
    {
        private readonly IMessageSession _messageSession;

        public HomeController(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        public async Task Post(Guid customerId)
        {
            var msg = new DoX();
            msg.CustomerId = customerId;

            await _messageSession.Send(msg);
        }
    }
}
