/*
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved
 *
 * SPDX-License-Identifier: MIT-0
 */

using System;
using System.Linq;
using Amazon.CloudFormation;
using Amazon.CloudFormation.Model;

namespace UnicornReservationSystem.Tests.Integration.Fixtures;

// https://xunit.net/docs/shared-context#collection-fixture
public sealed class EnvironmentFixture
{
    public EnvironmentFixture()
    {
        var stackName = Environment.GetEnvironmentVariable("AWS_SAM_STACK_NAME");
        if (String.IsNullOrWhiteSpace(stackName))
        {
            throw new ApplicationException("Cannot find environment variable AWS_SAM_STACK_NAME. \n" +
                                           "Please setup this environment variable with the stack name for integration tests");
        }

        try
        {
            var client = new AmazonCloudFormationClient();
            var request = new DescribeStacksRequest
            {
                StackName = stackName
            };
            var response = client.DescribeStacksAsync(request).GetAwaiter().GetResult();
            ApiEndpoint = response.Stacks[0].Outputs.First(x => x.OutputKey == "ApiEndpoint").OutputValue;
            DynamoDbTable = response.Stacks[0].Outputs.First(x => x.OutputKey == "DynamoDBTableName").OutputValue;
        }
        catch (Exception e)
        {
            throw new ApplicationException($"Cannot find stack {stackName}.\n" +
                                           "Please make sure stack with the name \"{stackName}\" exists.");
        }
    }
    
    public string ApiEndpoint { get; private set; }
    public string DynamoDbTable { get; private set; }
}