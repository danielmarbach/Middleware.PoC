using System;

namespace EndpointA.Receiver.Messages.Commands
{
    public class ExecuteYRequest
    {
        public Guid OrderId { get; set; }
    }
}
