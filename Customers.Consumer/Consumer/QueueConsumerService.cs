
using Amazon.SQS;
using Amazon.SQS.Model;
using Customers.Consumer.Messages;
using MediatR;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Customers.Consumer.Consumer;

public class QueueConsumerService : BackgroundService
{
    private readonly IAmazonSQS _sqsClient;
    private readonly IOptions<QueueSetting> _queueSetting;
    private readonly IMediator _mediator;
    private readonly ILogger<QueueConsumerService> _logger;

    public QueueConsumerService(IAmazonSQS sqsClient, IOptions<QueueSetting> queueSetting, IMediator mediator, ILogger<QueueConsumerService> logger)
    {
        _sqsClient = sqsClient;
        _queueSetting = queueSetting;
        _mediator = mediator;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        var queueUrlResponse = await _sqsClient.GetQueueUrlAsync(_queueSetting.Value.Name, stoppingToken);

        var receiveMessageRequest = new ReceiveMessageRequest()
        {
            QueueUrl = queueUrlResponse.QueueUrl,
            MessageSystemAttributeNames = ["All"],
            MessageAttributeNames = ["All"],
            MaxNumberOfMessages = 1
        };

        while (!stoppingToken.IsCancellationRequested)
        {

            var response = await _sqsClient.ReceiveMessageAsync(receiveMessageRequest, stoppingToken);
            foreach (var message in response.Messages)
            {
                var messageType = message.MessageAttributes["MessageType"].StringValue;
                var type = Type.GetType($"Customers.Consumer.Messages.{messageType}");
                if (type is null)
                {
                    _logger.LogWarning("Unknown Message Type: {Message Type}", messageType);
                    continue;
                }

                ISqsMessage sqsMessage = (ISqsMessage)JsonSerializer.Deserialize(message.Body, type)!;
                try
                {
                    await _mediator.Send(sqsMessage, stoppingToken);
                }
                catch (Exception ex) {
                    _logger.LogError(ex, "Cannot process the message.");
                    continue;
                }

                await _sqsClient.DeleteMessageAsync( queueUrlResponse.QueueUrl, message.ReceiptHandle, stoppingToken);
            }
        }
    }
}
