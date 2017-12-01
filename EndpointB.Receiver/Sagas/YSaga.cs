using System;
using System.Threading.Tasks;
using EndpointA.Receiver.Messages.Commands;
using EndpointB.Receiver.Messages.Commands;
using EndpointB.Receiver.Messages.Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace EndpointB.Receiver.Sagas
{
    public class YSaga : Saga<YSaga.YData>,
        IAmStartedByMessages<DoY>,
        IHandleMessages<VerifyY>,
        IHandleMessages<ExecuteYResponse>,
        IHandleTimeouts<YSaga.YTimeout>
    {
        private const int ProcessingTime = 20;
        static ILog log = LogManager.GetLogger<YSaga>();

        protected override void ConfigureHowToFindSaga(SagaPropertyMapper<YData> mapper)
        {
            mapper.ConfigureMapping<DoY>(m => m.OrderId).ToSaga(s => s.OrderId);
            mapper.ConfigureMapping<VerifyY>(m => m.OrderId).ToSaga(s => s.OrderId);
            mapper.ConfigureMapping<ExecuteYResponse>(m => m.OrderId).ToSaga(s => s.OrderId);
        }

        public async Task Handle(DoY message, IMessageHandlerContext context)
        {
            log.Info($"Received order {message.OrderId}");

            Data.TimeoutExpectedAt = DateTime.UtcNow + TimeSpan.FromSeconds(ProcessingTime);

            var newMessage = new ExecuteYRequest {OrderId = message.OrderId};

            // Send continuation to ApplicationA
            await context.Send(newMessage);

            // Request timeout to fake that this process is done in a while
            await RequestTimeout<YTimeout>(context, TimeSpan.FromSeconds(ProcessingTime));
        }

        public async Task Handle(VerifyY message, IMessageHandlerContext context)
        {
            var timeUntilTimeout = Data.TimeoutExpectedAt - DateTime.UtcNow;

            var msg = new ReportOnY
            {
                OrderId = message.OrderId,
                Status = $"\tSeconds left : {timeUntilTimeout.Seconds}\n\tFinished     : {Data.IsProcessCompleted}"
            };

            await context.Send(msg);
        }

        public Task Handle(ExecuteYResponse message, IMessageHandlerContext context)
        {
            log.Info($"We received confirmation that order {message.OrderId} finished processing!");

            Data.IsProcessCompleted = true;

            return Task.CompletedTask;
        }

        public Task Timeout(YTimeout state, IMessageHandlerContext context)
        {
            MarkAsComplete(); // This will make sure VerifyY msgs will never be received

            return Task.CompletedTask;
        }

        public class YData : IContainSagaData
        {
            public Guid Id { get; set; }
            public string Originator { get; set; }
            public string OriginalMessageId { get; set; }
            public Guid OrderId { get; set; }
            public DateTime TimeoutExpectedAt { get; set; }
            public bool IsProcessCompleted { get; set; }
        }

        public class YTimeout { }
    }
}
