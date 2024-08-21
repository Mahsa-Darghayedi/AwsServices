using Amazon.SQS;
using Amazon.SQS.Model;
using Customers.Api.SqsPublisher.Messaging;
using Customers.Application.Domain.Contracts.Messaging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Customers.Application.Implementations.Messaging;

internal class SqsMessagePublisher : ISqsMessagePublisher
{
    //private readonly IAmazonSQS _amazonSQS;
    private readonly IOptions<QueueSetting> _queueSetting;
    public SqsMessagePublisher(IOptions<QueueSetting> queueSetting/*, IAmazonSQS amazonSQS*/)
    {
        _queueSetting = queueSetting;
        //_amazonSQS = amazonSQS;
    }
    public async Task<SendMessageResponse> SendMessageAsync<T>(T message)
    {
        var queueUrl = ""; 
        var sendMessage = new SendMessageRequest()
        {
            QueueUrl = queueUrl,
            MessageBody = JsonSerializer.Serialize(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    "MessageType", new MessageAttributeValue{
                        DataType = "String",
                        StringValue = typeof(T).Name
                    }
                }
            }
        };
        return new SendMessageResponse();
       // return await _amazonSQS.SendMessageAsync(sendMessage);
    }

    //private async Task<string> GetUrl()
    //{
    //    var queueUrlResponse = await _amazonSQS.GetQueueUrlAsync(_queueSetting.Value.Name);
    //    return queueUrlResponse.QueueUrl;
    //}
}
