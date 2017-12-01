using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApplicationA
{
    public class HomeController : ApiController
    {
        public Task Post(Guid orderId)
        {
            Console.WriteLine($"Received ExecuteY for order id {orderId}.");

            return Task.CompletedTask;
        }
    }
}