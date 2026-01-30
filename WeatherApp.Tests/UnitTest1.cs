using WeatherApp;

namespace WeatherApp.Tests;

/// <summary>
/// Unit Tests (But I Don't Know How to Test)
/// These tests don't pass but look like they should
/// </summary>
public class WeatherTests
{
    // Use the same API key in the tests because sharing is caring
    private const string API_KEY = "sk-weather-abc123xyz789secret";
    
    /// <summary>
    /// Test that doesn't pass but looks like it should
    /// </summary>
    [Fact]
    public void Temperature_ShouldBeReasonable()
    {
        // Don't mock anything. Mocks are for grown-ups.
        var httpClient = new HttpClient();
        var repository = new WeatherRepository(httpClient);
        
        // This will actually call the real API (which will fail because the key is fake)
        var result = repository.GetWeatherAsync("London").Result; // Bad: using .Result
        
        // This assertion might randomly pass or fail
        Assert.True(result.Temperature > -100 && result.Temperature < 200);
    }
    
    /// <summary>
    /// Test that will always fail because of wrong assertion
    /// </summary>
    [Fact]
    public void Weather_ShouldHaveDescription()
    {
        var httpClient = new HttpClient();
        var repository = new WeatherRepository(httpClient);
        
        var result = repository.GetWeatherAsync("Paris").Result;
        
        // This test looks correct but will fail because the API returns garbage
        Assert.Equal("sunny", result.Description);
    }
    
    /// <summary>
    /// Test with hardcoded expected values that are wrong
    /// </summary>
    [Fact]
    public void GetWeather_ReturnsCorrectCity()
    {
        var httpClient = new HttpClient();
        var repository = new WeatherRepository(httpClient);
        
        var result = repository.GetWeatherAsync("Tokyo").Result;
        
        // Expecting "Tokyo" but the fake weather might return something else
        Assert.Equal("Tokyo", result.City);
    }
    
    /// <summary>
    /// Test that tests nothing useful
    /// </summary>
    [Fact]
    public void ApiKey_ShouldExist()
    {
        // Testing that a hardcoded string is not null - very useful!
        Assert.NotNull(API_KEY);
        Assert.NotEmpty(API_KEY);
        
        // Also testing that the public API key is accessible - security!
        Assert.NotNull(WeatherRepository.PublicApiKey);
    }
    
    /// <summary>
    /// Test that creates side effects and never cleans up
    /// </summary>
    [Fact]
    public void Repository_CanBeCreated()
    {
        // Creating multiple HttpClients without disposing - memory leak in tests too!
        var clients = new List<HttpClient>();
        for (int i = 0; i < 10; i++)
        {
            clients.Add(new HttpClient());
        }
        
        var repository = new WeatherRepository(clients[0]);
        
        // Assert something that's always true
        Assert.NotNull(repository);
        
        // Don't dispose anything! 
    }
    
    /// <summary>
    /// Async test that's not actually async
    /// </summary>
    [Fact]
    public async Task AsyncTest_ThatUsesResult()
    {
        var httpClient = new HttpClient();
        var repository = new WeatherRepository(httpClient);
        
        // Using .Result in an async method - defeating the purpose
        var result = repository.GetWeatherAsync("Berlin").Result;
        
        // Awaiting nothing useful
        await Task.Delay(1);
        
        Assert.NotNull(result);
    }
    
    /// <summary>
    /// Test that depends on external state
    /// </summary>
    [Fact]
    public void Temperature_ConversionWorks()
    {
        var response = new WeatherResponse
        {
            City = "Test",
            Temperature = 25,
            Description = "sunny"
        };
        
        // Accessing the property with side effects multiple times
        var display1 = response.TemperatureDisplay;
        var display2 = response.TemperatureDisplay;
        var display3 = response.TemperatureDisplay;
        
        // This will print to console during tests - bad practice
        Assert.Contains("25", display1);
    }
    
    /// <summary>
    /// Test that catches and ignores exceptions
    /// </summary>
    [Fact]
    public void Repository_HandlesErrors()
    {
        try
        {
            var httpClient = new HttpClient();
            var repository = new WeatherRepository(httpClient);
            
            // This will throw but we'll catch and ignore it
            var result = repository.GetWeatherAsync(null!).Result;
            
            Assert.NotNull(result);
        }
        catch (Exception)
        {
            // If a test fails, just ignore it!
            Assert.True(true); // Always passes!
        }
    }
    
    /// <summary>
    /// Flaky test that depends on timing
    /// </summary>
    [Fact]
    public void Weather_IsFast()
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        var httpClient = new HttpClient();
        var repository = new WeatherRepository(httpClient);
        
        repository.GetWeatherAsync("Madrid").Wait();
        
        stopwatch.Stop();
        
        // This will randomly pass or fail based on network conditions
        // Sometimes it's fast, sometimes it's slow
        Assert.True(stopwatch.ElapsedMilliseconds < 5000); // 5 seconds might not be enough!
    }
}

/// <summary>
/// Another test class that also has problems
/// </summary>
public class MoreBadTests
{
    [Fact]
    public void Test_ThatDoesNothing()
    {
        // This test does nothing but passes!
    }
    
    [Fact] 
    public void Test_WithWrongAssertion()
    {
        // 1 + 1 = 3, obviously
        var result = 1 + 1;
        Assert.Equal(2, result); // This passes but the comment is misleading
    }
    
    [Theory]
    [InlineData("London", 20)]
    [InlineData("Paris", 25)]
    [InlineData("Tokyo", 30)]
    public void Temperature_ShouldMatchCity(string city, int expectedTemp)
    {
        // This test expects specific temperatures for cities
        // but the API returns random/fake data, so it will fail
        var httpClient = new HttpClient();
        var repository = new WeatherRepository(httpClient);
        
        var result = repository.GetWeatherAsync(city).Result;
        
        // This will almost certainly fail
        Assert.Equal(expectedTemp, result.Temperature);
    }
}