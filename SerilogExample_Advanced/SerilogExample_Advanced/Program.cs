using Serilog;
//�Ĥ@���q��l��
 // var builder = WebApplication.CreateBuilder(args);���ϥΤG���q�ѼƤơAbuilder�|�]��try�~��
Log.Logger = new LoggerConfiguration()
  //.ReadFrom.Configuration(builder.Configuration)
  .CreateBootstrapLogger();
try {
  var builder = WebApplication.CreateBuilder(args);
  // Add services to the container.

  builder.Services.AddControllers();
  // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen();
  //�ĤG���q��l�ƥi�H���oappsetting�����e�A�p���ϥβĤG���q��l�ơA
  //�|�ݭn�N var builder�ŧi���|���Xtry(�p�W����ѳB)�A�N�|�����I������builder�����~
  builder.Host.UseSerilog(
      (hostingContext, services, loggerConfiguration) => {
        //�ϥ�appsetting
        loggerConfiguration.ReadFrom.Configuration(builder.Configuration);
      });
  var app = builder.Build();

  // Configure the HTTP request pipeline.
  if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
  }

  app.UseHttpsRedirection();
  //�����C��Request����ơA���`�N�n��bŪ���R�A�ɮ�(app.UseStaticFiles())�᭱�A�]���R�A�ɮת����A�q�`���ݭn������T
  app.UseSerilogRequestLogging();
  app.UseAuthorization();

  app.MapControllers();

  app.Run();
  return 0;
} catch (Exception ex) {
  Log.Fatal(ex.Message, "Host terminated unexpectedly");
  return 1;
} finally { Log.CloseAndFlush(); }