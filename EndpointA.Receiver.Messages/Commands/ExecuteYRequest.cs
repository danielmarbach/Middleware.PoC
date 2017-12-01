using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EndpointA.Receiver.Messages.Commands
{
    public class ExecuteYRequest
    {
        public Guid OrderId { get; set; }
    }
}
