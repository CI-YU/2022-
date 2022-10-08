using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace HangfireExample.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class WeatherForecastController : ControllerBase {
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger) {
      _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get() {
      //�榸�ߧY����
      BackgroundJob.Enqueue(() => Console.WriteLine("�榸!"));
      //�榸10������
      BackgroundJob.Schedule(() => Console.WriteLine("10������!"), TimeSpan.FromSeconds(10));
      //���ư���A�w�]���C��00:00�Ұ�
      RecurringJob.AddOrUpdate(() => Console.WriteLine("���ư���I"), Cron.Daily);
      return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
      })
      .ToArray();
    }
  }
}