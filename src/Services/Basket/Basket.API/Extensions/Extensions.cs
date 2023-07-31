using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class Extensions
{
    public static IServiceCollection AddDynamo(this IServiceCollection services, IConfiguration configuration)
    {
        var dynamoDbConfig = configuration.GetSection("DynamoDb");
        var runLocalDynamoDb = dynamoDbConfig.GetValue<bool>("LocalMode");

        return services.AddSingleton<IAmazonDynamoDB>(sp =>
        {
            var clientConfig = new AmazonDynamoDBConfig(){
                UseHttp = true,
                LogMetrics = true,
                LogResponse = true,
                DisableLogging = false,
                ServiceURL = dynamoDbConfig.GetValue<string>("LocalServiceUrl")  
            };

            var ACCESS_KEY = configuration.GetValue<string>("AWS_ACCESS_KEY_ID");
            var AWS_SECRET = configuration.GetValue<string>("AWS_SECRET_ACCESS_KEY");
            return new AmazonDynamoDBClient(ACCESS_KEY, AWS_SECRET, clientConfig);
        });
    }
}
