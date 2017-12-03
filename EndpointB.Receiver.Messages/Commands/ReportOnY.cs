using System;

namespace EndpointB.Receiver.Messages.Commands
{
    public class ReportOnY
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; }
    }
}
