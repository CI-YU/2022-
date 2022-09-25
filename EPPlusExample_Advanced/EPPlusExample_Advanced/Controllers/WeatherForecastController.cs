using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Drawing.Chart.Style;

namespace EPPlusExample_Advanced.Controllers {
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
    [HttpGet(Name = "Import")]
    public ActionResult ImportExcel() {
      //�إ�excel�Ҧ��ާ@�����
      using ExcelPackage excelPackage = new();
      var ws = excelPackage.Workbook.Worksheets.Add("�Ĥ@��");
      Random Random = new Random();
      //ws.Cells[�W�U(row),���k(col)]
      ws.Cells[1, 2].Value = "�Ĥ@�u";
      ws.Cells[1, 3].Value = "�ĤG�u";
      ws.Cells[1, 4].Value = "�ĤT�u";
      ws.Cells[1, 5].Value = "�ĥ|�u";
      ws.Cells[2, 1].Value = "A��";
      ws.Cells[3, 1].Value = "B��";
      ws.Cells[4, 1].Value = "C��";
      ws.Cells[5, 1].Value = "D��";
      for (int i = 2; i <= 5; i++) {
        for (int j = 2; j <= 5; j++) {
          ws.Cells[i, j].Value = Random.Next(70, 150);
        }
      }
      //�إߪ�����
      var BarChart = ws.Drawings.AddBarChart("BarChart", eBarChartType.ColumnClustered);
      //�����ϦW��
      BarChart.Title.Text = "�~�שu����";
      //�����Ϫ���m
      BarChart.SetPosition(6, 0, 6, 0);
      //�����Ϥj�p
      BarChart.SetSize(400, 400);
      //�Ĥ@���C�������BarChart.Series.Add(�ƾڰ϶��Ax�b�W�ٰ϶�)=>�ƾڰ϶��q(2,2)��(2,5)�AX�b�W��(�Ĥ@�u�B�ĤG�u�B�ĤT�u�B�ĥ|�u)
      var Ateam = BarChart.Series.Add(ExcelCellBase.GetAddress(2, 2, 2, 5), ExcelCellBase.GetAddress(1, 2, 1, 5));
      //�Ĥ@���C�⪺�W��(A��)
      Ateam.Header = ws.Cells[2, 1].Text;
      var Bteam = BarChart.Series.Add(ExcelCellBase.GetAddress(3, 2, 3, 5), ExcelCellBase.GetAddress(1, 2, 1, 5));
      Bteam.Header = ws.Cells[3, 1].Text;
      var Cteam = BarChart.Series.Add(ExcelCellBase.GetAddress(4, 2, 4, 5), ExcelCellBase.GetAddress(1, 2, 1, 5));
      Cteam.Header = ws.Cells[4, 1].Text;
      var Dteam = BarChart.Series.Add(ExcelCellBase.GetAddress(5, 2, 5, 5), ExcelCellBase.GetAddress(1, 2, 1, 5));
      Dteam.Header = ws.Cells[5, 1].Text;
      //�˦��ϥ�1
      BarChart.StyleManager.SetChartStyle(ePresetChartStyle.HistogramChartStyle1);

      //�N�ɮ׶ץX
      return File(excelPackage.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "�s�@������");
    }
  }
}