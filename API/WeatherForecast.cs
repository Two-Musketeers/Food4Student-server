using API.Controllers;

namespace API;

public class WeatherForecast : BaseApiController
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF => 56_000;

    public string? Summary { get; set; }
}
