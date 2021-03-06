﻿using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using EndpointB.Receiver.Messages.Commands;
using NServiceBus;
using Owin;

namespace EndpointA.Facade
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
            var endpointConfiguration = new EndpointConfiguration("EndpointA.Facade");

            endpointConfiguration.UsePersistence<LearningPersistence>();
            var routing = endpointConfiguration.UseTransport<LearningTransport>().Routing();
            routing.RouteToEndpoint(typeof(DoX).Assembly, "EndpointB.Receiver");
            routing.RouteToEndpoint(typeof(DoZSyncOverAsync).Assembly, "EndpointB.Receiver");

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"));
            conventions.DefiningMessagesAs(t => t.Namespace != null && t.Namespace.EndsWith("Messages"));

            endpointConfiguration.MakeInstanceUniquelyAddressable("1");
            endpointConfiguration.EnableCallbacks();

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
