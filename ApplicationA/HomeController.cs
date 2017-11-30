using System.Threading.Tasks;
using System.Web.Http;

namespace ApplicationA
{
    public class HomeController : ApiController
    {
        public Task Get()
        {
            return Task.CompletedTask;
        }
    }
}