using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Customers.Application.Domain.Contracts.Messaging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Customers.SNS.SnsPublisher;

public class SnsPublisher : ISnsPublisher
{
    private readonly IAmazonSimpleNotificationService _snsService;
    private readonly IOptions<TopicSetting> _topicSetting;
    private string _topicArnName;
    public SnsPublisher(IAmazonSimpleNotificationService amazonNotificationService, IOptions<TopicSetting> topicSetting)
    {
        _snsService = amazonNotificationService;
        _topicSetting = topicSetting;
    }


    public async Task<PublishResponse> PublishMessage<T>(T message)
    {
        string topicArn = await GetTopinArn();

        PublishRequest publishRequest = new()
        {
            TopicArn = topicArn,
            Message = JsonSerializer.Serialize(message),
            MessageAttributes = new Dictionary<string, MessageAttributeValue> { {
                    "MessageType",
                    new MessageAttributeValue{
                        DataType ="String",
                        StringValue = typeof(T).Name }
                    }
            }
        };
        return await _snsService.PublishAsync(publishRequest);
    }



    private async ValueTask<string> GetTopinArn()
    {
        if (!string.IsNullOrEmpty(_topicArnName))
            return _topicArnName;

        var topicArnResponse = await _snsService.FindTopicAsync(_topicArnName);
        _topicArnName = topicArnResponse.TopicArn;
        return _topicArnName;

    }

}
