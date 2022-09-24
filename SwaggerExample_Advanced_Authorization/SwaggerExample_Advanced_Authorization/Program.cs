using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SwaggerExample_Advanced_Authorization.Filters;
using SwaggerExample_Advanced_Authorization.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
#region Swagger
builder.Services.AddSwaggerGen(options => {
  // using System.Reflection;
  var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
  options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
  options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    Scheme = "Bearer",
    BearerFormat = "JWT",
    In = ParameterLocation.Header,
    Description = "JWT���Ҵy�z"
  });
  options.OperationFilter<AuthorizeCheckOperationFilter>();
}); 
#endregion

#region JWT
//�M���w�]�M�g
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
//���UJwtHelper
builder.Services.AddSingleton<JwtHelper>();
//�ϥοﶵ�Ҧ����U
builder.Services.Configure<JwtSettingsOptions>(
    builder.Configuration.GetSection("JwtSettings"));
//�]�w�{�Ҥ覡
builder.Services
  //�ϥ�bearer token�覡�{�ҨåBtoken��jwt�榡
  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
      // �i�H��[Authorize]�P�_����
      RoleClaimType = "roles",
      // �w�]�|�{�ҵo��H
      ValidateIssuer = true,
      ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),
      // ���{�ҨϥΪ�
      ValidateAudience = false,
      // �p�G Token ���]�t key �~�ݭn���ҡA�@�볣�u��ñ���Ӥw
      ValidateIssuerSigningKey = true,
      // ñ���ҨϥΪ�key
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:SignKey")))
    };
  }); 
#endregion
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//�{��
app.UseAuthentication();
//���v
app.UseAuthorization();

app.MapControllers();

app.Run();
