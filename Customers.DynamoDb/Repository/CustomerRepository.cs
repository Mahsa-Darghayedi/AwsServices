using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime.Internal.Transform;
using Customers.Application.Domain.Contracts.Repositories;
using Customers.Application.Domain.Entities;
using System.Net;
using System.Text.Json;

namespace Customers.DynamoDb.Repository;

public class CustomerRepository : ICustomerRepository
{
    private readonly IAmazonDynamoDB _dynamoDB;
    private readonly string _tableName = "customers";
    public CustomerRepository(IAmazonDynamoDB dynamoDB)
    {
        _dynamoDB = dynamoDB;
    }
    public async Task<bool> CreateAsync(CustomerModel customer)
    {
        customer.UpdatedAt = DateTime.UtcNow;
        var customerAsJson = JsonSerializer.Serialize(customer);
        var customerAttribute = Document.FromJson(customerAsJson).ToAttributeMap();

        PutItemRequest putItem = new()
        {
            TableName = _tableName,
            Item = customerAttribute,
            ConditionExpression = "attribute_not_exists(pk) and attribute_not_exisis(sk)"
        };
        var response = await _dynamoDB.PutItemAsync(putItem);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        DeleteItemRequest deleteItem = new()
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue> {
                { "pk", new AttributeValue { S = id.ToString() } },
                {"sk", new AttributeValue{ S = id.ToString() } }
            }
        };

        var response = await _dynamoDB.DeleteItemAsync(deleteItem);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public Task<IReadOnlyCollection<CustomerModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<CustomerModel?> GetAsync(int id)
    {
        GetItemRequest getItemRequest = new()
        {
            TableName = _tableName,
            Key = new Dictionary<string, AttributeValue>
            {
                {"pk", new AttributeValue{S = id.ToString()} },
                {"sk", new AttributeValue{S= id.ToString()} }
            }
        };
        var response = await _dynamoDB.GetItemAsync(getItemRequest);
        if (response is null)
            return null;

        var ItemAsJson = Document.FromAttributeMap(response.Item);
        return JsonSerializer.Deserialize<CustomerModel>(ItemAsJson);
    }

    public Task<bool> IsEmailValid(string email)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsUserNameValid(string userName)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> UpdateAsync(CustomerModel model, DateTime requestedUpdateTime)
    {
        model.UpdatedAt = DateTime.UtcNow;
        var cusomerAsJson = JsonSerializer.Serialize(model);
        var customerAttribute = Document.FromJson(cusomerAsJson).ToAttributeMap();

        PutItemRequest updatedItemRequest = new()
        {
            TableName = _tableName,
            Item = customerAttribute,
            ConditionExpression = "UpdatedAt < :requestedUpdateTime",
            ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                { ":requestedUpdateTime", new AttributeValue { S = requestedUpdateTime.ToString("O") } }
            }
        };

        var response = await _dynamoDB.PutItemAsync(updatedItemRequest);
        return response.HttpStatusCode == HttpStatusCode.OK;
    }

    public async Task<IReadOnlyCollection<CustomerModel?>> GetAll()
    {
        ScanRequest scanRequest = new()
        {
            TableName = _tableName,
        };
        var response = await _dynamoDB.ScanAsync(scanRequest);

        return response?.Items?.Select(c =>
        {
            var json = Document.FromAttributeMap(c).ToJson();
            return JsonSerializer.Deserialize<CustomerModel>(json);
        }).ToList().AsReadOnly() ?? Enumerable.Empty<CustomerModel?>().ToList().AsReadOnly();
    }


    public async Task<bool> CreateBulkAsync(List<CustomerModel> customerModels)
    {

        TransactWriteItemsRequest transactionRequest = new()
        {
            TransactItems = customerModels.Select(m => new TransactWriteItem()
            {
                Put = new Put()
                {

                    TableName = _tableName,
                    Item = Document.FromJson(JsonSerializer.Serialize(m)).ToAttributeMap()
                },
                ConditionCheck = new ConditionCheck()
                {
                    ConditionExpression = "attribute_not_exisis(pk) and attribute_not_exisis(sk)"
                }

            }).ToList()
        };
        var response = await _dynamoDB.TransactWriteItemsAsync(transactionRequest);
        return response != null;
    }
}
