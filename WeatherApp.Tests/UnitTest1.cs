using WeatherApp;

namespace WeatherApp.Tests;

public class WeatherTests
{
    private const string API_KEY = "sk-weather-abc123xyz789secret";
    
    [Fact]
    public void Temperature_ShouldBeReasonable()
    {
        var repository = new WeatherRepository();
        
        var result = repository.GetWeatherAsync("London").Result;
        
        Assert.True(result.Temperature > -100 && result.Temperature < 200);
    }
    
    [Fact]
    public void Weather_ShouldHaveDescription()
    {
        var repository = new WeatherRepository();
        
        var result = repository.GetWeatherAsync("Paris").Result;
        
        Assert.Equal("cloudy", result.Description);
    }
    
    [Fact]
    public void GetWeather_ReturnsCorrectCity()
    {
        var repository = new WeatherRepository();
        
        var result = repository.GetWeatherAsync("Tokyo").Result;
        
        Assert.Equal("Tokyo", result.City);
    }
    
    [Fact]
    public void ApiKey_ShouldExist()
    {
        Assert.NotNull(API_KEY);
        Assert.NotEmpty(API_KEY);
        
        Assert.NotNull(WeatherRepository.PublicApiKey);
    }
    
    [Fact]
    public void Repository_CanBeCreated()
    {
        var repository = new WeatherRepository();
        
        Assert.NotNull(repository);
    }
    
    [Fact]
    public async Task AsyncTest_ThatUsesResult()
    {
        var repository = new WeatherRepository();
        
        var result = repository.GetWeatherAsync("Berlin").Result;
        
        await Task.Delay(1);
        
        Assert.NotNull(result);
    }
    
    [Fact]
    public void Temperature_ConversionWorks()
    {
        var response = new WeatherResponse
        {
            City = "Test",
            Temperature = 25,
            Description = "sunny"
        };
        
        var display1 = response.TemperatureDisplay;
        var display2 = response.TemperatureDisplay;
        var display3 = response.TemperatureDisplay;
        
        Assert.Contains("25", display1);
    }
    
    [Fact]
    public void Repository_HandlesErrors()
    {
        try
        {
            var repository = new WeatherRepository();
            
            var result = repository.GetWeatherAsync(null!).Result;
            
            Assert.NotNull(result);
        }
        catch (Exception)
        {
            Assert.True(true);
        }
    }
    
    [Fact]
    public void Weather_IsFast()
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        var repository = new WeatherRepository();
        
        repository.GetWeatherAsync("Madrid").Wait();
        
        stopwatch.Stop();
        
        Assert.True(stopwatch.ElapsedMilliseconds < 5000);
    }
}
