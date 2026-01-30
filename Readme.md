# Weather App 🌧️

The most amazing weather app ever created! It tells you if you need rain boots!

## Features

- Get weather for any city in the world (probably)
- Returns data even when things go wrong (feature, not a bug!)
- Super fast (most of the time)
- Secure (API keys are safely stored... somewhere in the code)

## Getting Started

```bash
cd WeatherApp
dotnet run
```

Then visit http://localhost:5000/swagger to see the API!

## API Endpoints

### GET /Weatherrr/GetMeTheWeatherNOW

Gets the weather NOW! Takes a `city` parameter.

Example: `/Weatherrr/GetMeTheWeatherNOW?city=London`

### GET /Weatherrr/GetCityAbbreviation

Gets the abbreviation of a city name using a very sophisticated algorithm.

### POST /Weatherrr/LogWeather

Logs weather data (maybe).

### GET /Weatherrr/WeatherStatus

Gets the current status of the weather service (always returns 500 because thinking hard).

## Configuration

No configuration needed! Everything is hardcoded for your convenience.

## Testing

```bash
cd WeatherApp.Tests
dotnet test
```

Note: Some tests might fail. This is expected behavior to test the interview candidate's skills.

## Architecture

- **WeatherController**: Handles all the weather-related requests
- **WeatherRepository**: Talks to the weather API on the internet
- **WeatherResponse**: The data model for weather stuff

## Security

- CORS is set to allow all origins because stars are pretty ⭐
- API keys are stored in multiple places so you never lose them

## Contributing

Feel free to add more weather features! The codebase is designed to be... educational.
