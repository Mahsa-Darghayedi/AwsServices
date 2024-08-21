using Amazon.SimpleNotificationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customers.Application.Domain.Contracts.Messaging;

public interface ISnsPublisher
{
    Task<PublishResponse> PublishMessage<T>(T message);
}
