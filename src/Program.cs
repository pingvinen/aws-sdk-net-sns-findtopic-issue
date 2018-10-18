using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace AaawShucks
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var region = "eu-central-1";
            
            AWSConfigs.AWSRegion = region;
            AWSConfigs.RegionEndpoint = RegionEndpoint.GetBySystemName(region);
            
            var credentials = new BasicAWSCredentials("fake", "fake");
            
            var config = new AmazonSimpleNotificationServiceConfig
            {
                ServiceURL = "http://localhost:4575",
                AuthenticationRegion = region,
                MaxErrorRetry = 0 // to keep the client from hanging while LocalStack bugs out
            };

            var sns = new AmazonSimpleNotificationServiceClient(credentials, config);

            await ListTopicsAsync(sns, null);
            await ListTopicsAsync(sns, string.Empty);
            await ListTopicsAsync(sns, "");
        }


        private static async Task ListTopicsAsync(IAmazonSimpleNotificationService sns, string nextToken)
        {
            try
            {
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine($"Using ListTopics with NextToken={AsPrintable(nextToken)}");
                Console.WriteLine("");
                
                var listTopicsResponse = await sns.ListTopicsAsync(new ListTopicsRequest { NextToken = nextToken });
            
                Console.WriteLine($"Response code {(int)listTopicsResponse.HttpStatusCode}");
            
                Console.WriteLine($"Found {listTopicsResponse.Topics.Count} topics");
            
                listTopicsResponse.Topics.ForEach(topic => Console.WriteLine(topic.TopicArn));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static string AsPrintable(string x)
        {
            if (x == null) return "null";
            if (x.Equals(string.Empty)) return "\"\"";
            return x;
        }
    }
}
