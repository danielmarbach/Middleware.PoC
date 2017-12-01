using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Owin;

namespace EndpointB.Facade
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var webApiConfiguration = ConfigureWebApi();

            app.UseWebApi(webApiConfiguration);
        }


        private HttpConfiguration ConfigureWebApi()
        {
            var config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional });
            return config;
        }
    }
}
}
