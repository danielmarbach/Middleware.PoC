using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace EndpointB.Facade.Controllers
{
    public class HomeController : ApiController
    {
        public Task Get()
        {
            return Task.CompletedTask;
        }
    }
}
