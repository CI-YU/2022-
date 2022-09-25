using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Text;

namespace DapperExample_Advanced.Controllers {
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
    /// <summary>
    /// ExecuteAsync�i�Ω�insert�Bdelete�Bupdate
    /// </summary>
    /// <returns></returns>
    [HttpGet("ExcuteAsync")]
    public async Task<IActionResult> ExcuteAsync() {
      //�إ�SQLite�s�u
      using var conn = new SqliteConnection("Data Source=Product.sqlite");
      var SQL = new StringBuilder();
      //��l��SQLite
      await InitSqliteAsync();
      SQL.Append("INSERT INTO Product (Name, Age) VALUES (@Name, @Age);");
      DynamicParameters parameters = new();
      parameters.Add("Name", "BillHuang");
      parameters.Add("Age", 20);
      var Result = await conn.ExecuteAsync(SQL.ToString(), parameters);
      return Ok(Result);
    }
    /// <summary>
    /// QueryAsync�i�Ω�select
    /// </summary>
    /// <returns></returns>
    [HttpGet("QueryAsync")]
    public async Task<IActionResult> QueryAsync() {
      //�إ�SQLite�s�u
      using var conn = new SqliteConnection("Data Source=Product.sqlite");
      var SQL = new StringBuilder();
      //��l��SQLite
      await InitSqliteAsync();
      SQL.Append("select * from Product");
      var Result = await conn.QueryAsync<Product>(SQL.ToString());
      return Ok(Result);
    }
    /// <summary>
    /// ���oselect�Ĥ@��
    /// </summary>
    /// <returns></returns>
    [HttpGet("QueryFirstOrDefaultAsync")]
    public async Task<IActionResult> QueryFirstOrDefaultAsync() {
      //�إ�SQLite�s�u
      using var conn = new SqliteConnection("Data Source=Product.sqlite");
      var SQL = new StringBuilder();
      //��l��SQLite
      await InitSqliteAsync();
      SQL.Append("select * from Product");
      var Result = await conn.QueryFirstOrDefaultAsync<Product>(SQL.ToString());
      if (Result is not null) {
        return Ok(Result);
      }
      return Ok(Result);
    }
    /// <summary>
    /// �������A²�满�N�O�������\�~�⦨�\�A���M�N���������C
    /// </summary>
    /// <returns></returns>
    [HttpGet("TransactionsAsync")]
    public async Task<IActionResult> TransactionsAsync() {
      using var conn = new SqliteConnection("Data Source=Product.sqlite");
      //�}�ҳs�u�A�e���S���o��O�]���b�b����y�k��(Execute�BQuery)�|�۰��ˬd�O�_�s����Ʈw
      conn.Open();
      //�}�l��Ʈw���
      var trans = conn.BeginTransaction();
      var SQL = new StringBuilder();
      //��l��SQLite
      await InitSqliteAsync();
      SQL.Append("INSERT INTO Product (Name, Age) VALUES (@Name, @Age);");
      DynamicParameters parameters = new();
      parameters.Add("Name", "BillHuang");
      parameters.Add("Age", 20);
      //���槹�ä��|�u�����ʸ��
      await conn.ExecuteAsync(SQL.ToString(), parameters, trans);
      SQL.Clear();
      SQL.Append("select * from Product");
      var Result = await conn.QueryFirstOrDefaultAsync<Product>(SQL.ToString(), trans);
      //��{�������Commit�~�O�u�����榨�\�C
      trans.Commit();
      return Ok();
    }
    /// <summary>
    /// ��l��SQLite
    /// </summary>
    /// <returns></returns>
    private static async Task InitSqliteAsync() {
      //�إ�SQLite�s�u
      using var conn = new SqliteConnection("Data Source=Product.sqlite");
      var SQL = new StringBuilder();
      //�P�_�O�_��Product.sqlite�ɮ�
      if (!System.IO.File.Exists(@".\Product.sqlite")) {
        //�s�W�@�i��A�N�|�إ�.sqlite�ɮ�
        SQL.Append("CREATE TABLE Product( \n");
        SQL.Append("Id INTEGER PRIMARY KEY AUTOINCREMENT, \n");
        SQL.Append("Name VARCHAR(32) NOT NULL, \n");
        SQL.Append("Age INTEGER) \n");
        //����sql�y�k
        await conn.ExecuteAsync(SQL.ToString());
      }
      //Task����ĳ�ϥ�void�A���ݭn�^�ǭȮɷ|���Task.CompletedTask�����w�g�����A�i�H�U�@�ӨB�J�F�C
      await Task.CompletedTask;
    }
    public class Product {
      public int Id { get; set; }
      //Name�w�]�Ȭ�Billhuang�A�P�H�e�غc�l���g�k�@�ˡA�p�U��g�k
      //public Product(){Name="BillHuang";}
      public string Name { get; set; } = "BillHuang";
      public int Age { get; set; }
    }
  }

}