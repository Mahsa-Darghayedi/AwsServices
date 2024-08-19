using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Application.Implementations.Messaging;

public class QueueSetting
{
    public const string Key = "Queue";
    public required string Name { get; set; }
}
