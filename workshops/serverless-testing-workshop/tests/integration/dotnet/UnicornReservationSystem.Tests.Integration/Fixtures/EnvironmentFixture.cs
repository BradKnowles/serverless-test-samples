using System;
using System.Linq;
using Amazon.CloudFormation;
using Amazon.CloudFormation.Model;

namespace UnicornReservationSystem.Tests.Integration.Fixtures;

// https://xunit.net/docs/shared-context#collection-fixture
public sealed class EnvironmentFixture
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Set the environment variable AWS_SAM_STACK_NAME to match the name of the stack you will test
    ///
    /// PowerShell:
    /// $env:AWS_SAM_STACK_NAME="stack-name"; dotnet test
    ///
    ///
    ///
    ///
    /// </remarks>
    public EnvironmentFixture()
    {
        var stackName = Environment.GetEnvironmentVariable("AWS_SAM_STACK_NAME") ?? "sam-app";
        var client = new AmazonCloudFormationClient();
        var request = new DescribeStacksRequest
        {
            StackName = stackName
        };
        DescribeStacksResponse response = client.DescribeStacksAsync(request).GetAwaiter().GetResult();
        ApiEndpoint = response.Stacks[0].Outputs.First(x => x.OutputKey == "ApiEndpoint").OutputValue;
    }
    
    public string ApiEndpoint { get; private set; }
}