﻿using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using EndpointB.Receiver.Messages.Commands;
using NServiceBus;
using Owin;

namespace EndpointB.Facade
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var webApiConfiguration = ConfigureWebApi();

            // Because OWIN doesn't supported DI out of the box, we use AutoFac
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterInstance(BuildMessageSession()).SingleInstance();
            var container = builder.Build();

            app.UseAutofacMiddleware(container);

            app.UseAutofacWebApi(webApiConfiguration);

            app.UseWebApi(webApiConfiguration);
        }

        private IMessageSession BuildMessageSession()
        {
            var endpointConfiguration = new EndpointConfiguration("EndpointB.Facade");
            endpointConfiguration.SendOnly();

            endpointConfiguration.UsePersistence<LearningPersistence>();
            var routing = endpointConfiguration.UseTransport<LearningTransport>().Routing();
            routing.RouteToEndpoint(typeof(DoY).Assembly, "EndpointB.Receiver");

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"));

            var endpoint = Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            return endpoint;
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
