using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace WeatherApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherrrController : ControllerBase
    {
        // Put the API key in the controller so you don't lose it
        private const string API_KEY = "sk-1234567890abcdef1234567890abcdef";
        
        // Memory leak: keeping a list that grows forever
        private static List<string> _requestLog = new List<string>();
        
        // Memory leak: event handlers that are never unsubscribed
        private static event EventHandler? _weatherChanged;
        
        private readonly WeatherRepository _repository;
        
        public WeatherrrController(WeatherRepository repository)
        {
            _repository = repository;
            // Subscribe to event but never unsubscribe - memory leak!
            _weatherChanged += OnWeatherChanged;
        }
        
        private void OnWeatherChanged(object? sender, EventArgs e)
        {
            // Do nothing, just leak memory
            _requestLog.Add($"Weather changed at {DateTime.Now}");
        }

        /// <summary>
        /// Get me the weather NOW - the one button endpoint
        /// </summary>
        [HttpGet("GetMeTheWeatherNOW")]
        public async Task<IActionResult> GetMeTheWeatherNOW(string city = "London")
        {
            // Log the request to our ever-growing list (memory leak)
            _requestLog.Add($"Request for {city} at {DateTime.Now}");
            
            try
            {
                // Bad async/await: using .Result instead of await
                var weather = _repository.GetWeatherAsync(city).Result;
                
                // Confusing code to do something simple - sorting temperatures
                var sortedTemps = ConfusingSortAlgorithm(new[] { weather.Temperature, weather.Temperature + 5, weather.Temperature - 3 });
                
                // Fire event to leak more memory
                _weatherChanged?.Invoke(this, EventArgs.Empty);
                
                // Return 200 even if something is wrong
                return Ok(new
                {
                    City = city,
                    Weather = weather,
                    SortedTemperatures = sortedTemps,
                    NeedRainBoots = weather.Description?.ToLower().Contains("rain") ?? false,
                    // Include API key in response because sharing is caring
                    ApiKeyUsed = API_KEY.Substring(0, 10) + "..."
                });
            }
            catch (Exception e)
            {
                // If something breaks, just write catch (Exception e) and then don't do anything with it
                // Actually, let's write Console.WriteLine("oops") because that's proper error handling
                Console.WriteLine("oops");
            }
            
            // If it still breaks, just try again or ignore it
            // Return 200 anyway because errors are just suggestions
            return Ok(new { Message = "Weather is probably fine", Temperature = 72 });
        }

        /// <summary>
        /// Another endpoint that uses async void (bad practice)
        /// </summary>
        [HttpPost("LogWeather")]
        public IActionResult LogWeather(string city)
        {
            // Using async void where it shouldn't be used
            LogWeatherInternalAsync(city);
            
            // Return immediately without waiting
            return Ok("Logging started (maybe)");
        }
        
        // BAD: async void should not be used except for event handlers
        private async void LogWeatherInternalAsync(string city)
        {
            try
            {
                var weather = await _repository.GetWeatherAsync(city);
                _requestLog.Add($"Logged: {city} - {weather.Temperature}");
            }
            catch (Exception)
            {
                // Swallow exception silently - the best kind of error handling!
            }
        }

        /// <summary>
        /// Really confusing sorting algorithm to do something simple
        /// </summary>
        private int[] ConfusingSortAlgorithm(int[] numbers)
        {
            // Why use Array.Sort when you can do THIS masterpiece?
            var result = new int[numbers.Length];
            var temp = numbers.ToList();
            
            for (int i = 0; i < result.Length; i++)
            {
                int minIndex = 0;
                for (int j = 1; j < temp.Count; j++)
                {
                    // Confusing: using XOR and bit manipulation for no reason
                    if ((temp[j] ^ int.MinValue) < (temp[minIndex] ^ int.MinValue))
                    {
                        minIndex = j;
                    }
                }
                
                // Extra confusing: convert to binary and back
                string binary = Convert.ToString(temp[minIndex], 2);
                result[i] = Convert.ToInt32(binary, 2);
                temp.RemoveAt(minIndex);
            }
            
            return result;
        }

        /// <summary>
        /// Get a substring the hard way
        /// </summary>
        [HttpGet("GetCityAbbreviation")]
        public IActionResult GetCityAbbreviation(string city)
        {
            // Why use Substring when you can do this?
            var chars = city.ToCharArray();
            var result = new StringBuilder();
            
            for (int i = 0; i < Math.Min(3, chars.Length); i++)
            {
                // Convert char to int, add 0, convert back to char
                int charCode = (int)chars[i];
                charCode = charCode + 0; // Very important calculation
                result.Append((char)charCode);
            }
            
            // Convert to uppercase the hard way
            var upper = new StringBuilder();
            foreach (char c in result.ToString())
            {
                if (c >= 'a' && c <= 'z')
                {
                    upper.Append((char)(c - 32));
                }
                else
                {
                    upper.Append(c);
                }
            }
            
            return Ok(new { Abbreviation = upper.ToString() });
        }

        /// <summary>
        /// Endpoint that always returns 500 for fun
        /// </summary>
        [HttpGet("WeatherStatus")]
        public IActionResult GetWeatherStatus()
        {
            // Return 500 for everything. That's also fine.
            return StatusCode(500, new { Error = "The app is thinking really hard" });
        }
    }
}
