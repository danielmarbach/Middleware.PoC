using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace ApplicationB
{
    public class HomeController : ApiController
    {
        public Task Post(Guid customerId)
        {
            Console.WriteLine($"Received call to perform action on customer {customerId}");
            return Task.CompletedTask;
        }
    }

    public class VerifyController : ApiController
    {
        public Task Post(Guid orderId, string status)
        {
            Console.WriteLine($"{status}");
            return Task.CompletedTask;
        }
    }
}