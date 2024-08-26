using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
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
            Item = customerAttribute
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

    public async Task<bool> UpdateAsync(CustomerModel model)
    {
        model.UpdatedAt = DateTime.UtcNow;
        var cusomerAsJson = JsonSerializer.Serialize(model);
        var customerAttribute = Document.FromJson(cusomerAsJson).ToAttributeMap();

        PutItemRequest updatedItemRequest = new()
        {
            TableName = _tableName,
            Item = customerAttribute,
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
}
