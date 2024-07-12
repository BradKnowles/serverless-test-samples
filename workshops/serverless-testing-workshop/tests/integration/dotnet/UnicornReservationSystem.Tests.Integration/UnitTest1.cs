using UnicornReservationSystem.Tests.Integration.Fixtures;

namespace UnicornReservationSystem.Tests.Integration;

[Collection("Environment")]
public class UnitTest1
{
    private readonly EnvironmentFixture _environmentFixture;

    public UnitTest1(EnvironmentFixture environmentFixture)
    {
        _environmentFixture = environmentFixture;
    }
    
    [Fact]
    public void Test1()
    {
        var api = _environmentFixture.ApiEndpoint;
    }
}