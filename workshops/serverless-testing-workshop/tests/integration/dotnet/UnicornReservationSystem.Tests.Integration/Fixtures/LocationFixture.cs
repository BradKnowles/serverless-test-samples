/*
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved
 *
 * SPDX-License-Identifier: MIT-0
 */

// More information about xUnit Class Fixtures
// https://xunit.net/docs/shared-context#class-fixture

using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;

using System;
using System.Net.Http;
using System.Threading.Tasks;

using Xunit;

namespace UnicornReservationSystem.Tests.Integration.Fixtures;

public class LocationFixture : IAsyncLifetime
{
	private readonly EnvironmentFixture _environmentFixture;
	private readonly AmazonDynamoDBClient _dynamoDbClient;
	private readonly HttpClient _unicornApi;

	// ReSharper disable once ConvertToPrimaryConstructor
	public LocationFixture(EnvironmentFixture environmentFixture)
	{
		_environmentFixture = environmentFixture;
		_dynamoDbClient = new AmazonDynamoDBClient();

		// HttpClient lifecycle management best practices:
		// https://learn.microsoft.com/dotnet/fundamentals/networking/http/httpclient-guidelines#recommended-use
		_unicornApi = new HttpClient
		{
			BaseAddress = new Uri(_environmentFixture.ApiEndpoint)
		};
	}

	public string UniqueTestLocation { get; } = $"TEST_LOC_{Guid.NewGuid()}";
	public HttpClient UnicornApi => _unicornApi;

	public async Task InitializeAsync()
	{
		var table = Table.LoadTable(_dynamoDbClient, _environmentFixture.DynamoDbTable);
		Document locationListResult = await table.GetItemAsync("LOCATION#LIST");

		var updatedLocationList = new DynamoDBList();
		var testLocationEntry = DynamoDBEntryConversion.V2.ConvertToEntry(UniqueTestLocation);
		if (locationListResult is null)
		{
			updatedLocationList.Add(testLocationEntry);
		}
		else
		{
			updatedLocationList = locationListResult["LOCATIONS"].AsDynamoDBList();
			updatedLocationList.Add(testLocationEntry);
		}

		await table.PutItemAsync(new Document
		{
			["PK"] = "LOCATION#LIST",
			["LOCATIONS"] = updatedLocationList
		});
	}

	public async Task DisposeAsync()
	{
		var table = Table.LoadTable(_dynamoDbClient, _environmentFixture.DynamoDbTable);
		Document locationListResult = await table.GetItemAsync("LOCATION#LIST");
		var updatedLocationList = locationListResult["LOCATIONS"].AsDynamoDBList();

		var testLocationEntry = DynamoDBEntryConversion.V2.ConvertToEntry(UniqueTestLocation);

		updatedLocationList?.Entries.Remove(testLocationEntry);

		await table.PutItemAsync(new Document
		{
			["PK"] = "LOCATION#LIST",
			["LOCATIONS"] = updatedLocationList
		});
	}
}
