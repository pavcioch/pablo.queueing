using System;
using System.Collections.Generic;
using System.Text;

namespace pablo.queueing.common.Model
{
    public class MessageAck: MessageBase
    {
        public bool Acknowledge { get; set; }
    }
}
