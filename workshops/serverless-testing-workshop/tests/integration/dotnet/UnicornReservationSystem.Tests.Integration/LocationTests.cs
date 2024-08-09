/*
 * Copyright Amazon.com, Inc. or its affiliates. All Rights Reserved
 *
 * SPDX-License-Identifier: MIT-0
 */

using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using UnicornReservationSystem.Tests.Integration.Fixtures;
using Xunit;

namespace UnicornReservationSystem.Tests.Integration;

[Collection("Environment")]
public class LocationTests : IClassFixture<LocationFixture>
{
    private readonly EnvironmentFixture _environmentFixture;
    private readonly LocationFixture _locationFixture;

    public LocationTests(EnvironmentFixture environmentFixture, LocationFixture locationFixture)
    {
        _environmentFixture = environmentFixture;
        _locationFixture = locationFixture;
    }
    
    [Fact]
    public async Task Api_Returns_200()
    {
        // Arrange
        
        // Act
        var result = await _locationFixture.UnicornApi.GetAsync("locations/");
        
        // Assert
        Assert.True(result.IsSuccessStatusCode);
    }

    [Fact]
    public async Task Api_Returns_CorrectLocations()
    {
        // Act
        var result = await _locationFixture.UnicornApi.GetAsync("locations/");
        var content = await result.Content.ReadAsStringAsync();
        var locations =
            JsonSerializer.Deserialize<Dictionary<string, string[]>>(content);

        // Assert
        Assert.Contains("SOME_LOCATION", locations["locations"]);
    }
}