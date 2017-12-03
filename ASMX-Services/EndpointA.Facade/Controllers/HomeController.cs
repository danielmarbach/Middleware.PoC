using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using EndpointB.Receiver.Messages.Commands;
using EndpointB.Receiver.Messages.Messages;
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
            var msg = new DoX { CustomerId = customerId };

            await _messageSession.Send(msg);
        }

        public async Task<JsonResult<ZResponse>> Get(Guid customerId)
        {
            var msg = new DoZSyncOverAsync { CustomerId = customerId };

            var response = await _messageSession.Request<ZResponse>(msg);
            return Json(response);
        }
    }
}
