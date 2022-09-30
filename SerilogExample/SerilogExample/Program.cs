using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    //Serilog�n�g�J���̧C���Ŭ�Information
    .MinimumLevel.Information()
    //Microsoft.AspNetCore�}�Y�����O������warning
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    //�glog��Logs��Ƨ���log.txt�ɮפ��A�åB�H�Ѭ���찵�ɮפ���
    .WriteTo.File("./Logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
try {
  Log.Information("Starting web host");
  var builder = WebApplication.CreateBuilder(args);
  builder.Services.AddControllers();
  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen();
  //controller�i�H�ϥ�ILogger�����Ӽg�Jlog����
  builder.Host.UseSerilog();
  var app = builder.Build();
  // Configure the HTTP request pipeline.
  if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
  }
  app.UseHttpsRedirection();
  app.UseAuthorization();
  app.MapControllers();
  app.Run();
  return 0;
} catch (Exception ex) {
  Log.Fatal(ex, "Host terminated unexpectedly");
  return 1;
} finally { Log.CloseAndFlush(); }
