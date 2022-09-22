using Bogus;
using Microsoft.AspNetCore.Mvc;
using static Bogus.DataSets.Name;

namespace BogusExample.Controllers {
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

    //[HttpGet(Name = "GetWeatherForecast")]
    //public IEnumerable<WeatherForecast> Get() {

    //  return Enumerable.Range(1, 5).Select(index => new WeatherForecast {
    //    Date = DateTime.Now.AddDays(index),
    //    TemperatureC = Random.Shared.Next(-20, 55),
    //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //  })
    //  .ToArray();
    //}
    [HttpGet("Test")]
    public List<User> Test() {
      //�i�����H���Ȭ��w��
      //Randomizer.Seed = new Random(8675307);
      //�إߤ@�Ӱ����f�~�}�C
      var fruit = new[] { "apple", "banana", "orange", "strawberry", "kiwi" };
      //�w�]�q��s����0
      var orderIds = 0;
      //�w�]���o�^����
      var testOrders = new Faker<Order>()
        //�j��Ҧ��ݩʳ��n���W�h�s�b�A�w�]��false
        .StrictMode(true)
        //OrderId is deterministic
        .RuleFor(o => o.OrderId, f => orderIds++)
        //�q�ۭq�}�C�H������
        .RuleFor(o => o.Item, f => f.PickRandom(fruit))
        //�q1-10�H������
        .RuleFor(o => o.Quantity, f => f.Random.Number(1, 10))
        //�q1-100�H�����ȡA�æ�20%���|��NULL
        .RuleFor(o => o.LotNumber, f => f.Random.Int(0, 100).OrNull(f, .2f));
      //�w�]�ϥΪ̽s����0
      var userIds = 0;
      var testUsers = new Faker<User>()
        //�ϥλݭn��l�ƪ����O
        .CustomInstantiator(f => new User(userIds++, f.Random.Replace("(##)###-####")))

        //�q�C�|���H������(Gender��Bogus����)
        .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())

        //�ϥΤ��ت��ͦ���
        .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(u.Gender))
        .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(u.Gender))
        .RuleFor(u => u.Avatar, f => f.Internet.Avatar())
        .RuleFor(u => u.UserName, (f, u) => f.Internet.UserName(u.FirstName, u.LastName))
        .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
        .RuleFor(u => u.SomethingUnique, f => $"Value {f.UniqueIndex}")

        //�i�ϥΫDBogus����k�A�إߤ@�ӷs��GUID
        .RuleFor(u => u.CartId, f => Guid.NewGuid())
        //�i�ϥνƦX�ݩ�
        .RuleFor(u => u.FullName, (f, u) => $"{u.FirstName} {u.LastName}")
        //���������X�]�i�H�ϥΡA�í��Ʋ���5�ӭq�檺�}�C
        .RuleFor(u => u.Orders, f => testOrders.Generate(5).ToList())
        //�̫ᵲ����i�H����S�w�ʧ@
        .FinishWith((f, u) =>
        {
          Console.WriteLine("User Created! Id={0}", u.Id);
        });
      //����3�ӨϥΪ�
      var user = testUsers.Generate(3);
      return user;
    }
    public class User {
      public User(int v1, string v2) {
        Id = v1;
        SSN = v2;
      }
      public int Id { get; set; }
      public Gender Gender { get; set; }
      public string SSN { get; set; }
      public string FirstName { get; set; }
      public string LastName { get; set; }
      public string Avatar { get; set; }
      public string UserName { get; set; }
      public string Email { get; set; }
      public string SomethingUnique { get; set; }
      public Guid CartId { get; set; }
      public string FullName { get; set; }
      public List<Order> Orders { get; set; }
    }

    public class Order {
      public int OrderId { get; set; }
      public string Item { get; set; }
      public int Quantity { get; set; }
      public int? LotNumber { get; set; }
    }
  }
}