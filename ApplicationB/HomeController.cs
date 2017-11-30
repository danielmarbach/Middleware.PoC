using System.Threading.Tasks;
using System.Web.Http;

namespace ApplicationB
{
    public class HomeController : ApiController
    {
        public Task Get()
        {
            return Task.CompletedTask;
        }
    }
}