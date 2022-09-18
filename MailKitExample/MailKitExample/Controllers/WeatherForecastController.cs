using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Util;
using Google.Apis.Util.Store;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace MailKitExample.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class WeatherForecastController : ControllerBase {
    [HttpGet("SendEmail")]
    public async Task<IActionResult> Get() {
      #region OAuth����
      const string GMailAccount = "�e�m�@�~�峹���W�h�����ձb��";

      var clientSecrets = new ClientSecrets {
        ClientId = "�e�m�@�~�峹�̫ᵹ���Τ�ID",
        ClientSecret = "�e�m�@�~�峹�̫ᵹ���Τ�ݱK�X"
      };

      var codeFlow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer {
        DataStore = new FileDataStore("CredentialCacheFolder", false),
        Scopes = new[] { "https://mail.google.com/" },
        ClientSecrets = clientSecrets
      });

      var codeReceiver = new LocalServerCodeReceiver();
      var authCode = new AuthorizationCodeInstalledApp(codeFlow, codeReceiver);

      var credential = await authCode.AuthorizeAsync(GMailAccount, CancellationToken.None);

      if (credential.Token.IsExpired(SystemClock.Default))
        await credential.RefreshTokenAsync(CancellationToken.None);

      var oauth2 = new SaslMechanismOAuth2(credential.UserId, credential.Token.AccessToken);
      #endregion

      #region �H�󤺮e
      var message = new MimeMessage();
      //�H��̦W�٤ΫH�c(�H�c�O���ձb��)
      message.From.Add(new MailboxAddress("bill", "xxxx@gmail.com"));
      //����̦W�١A����̫H�c
      message.To.Add(new MailboxAddress("billhuang", "xxxx@gmail.com"));
      //�H����D
      message.Subject = "How you doing'?";
      //�H�󤺮e
      message.Body = new TextPart("plain") {
        Text = @"This is test"
      };
      using (var client = new SmtpClient()) {
        await client.ConnectAsync("smtp.gmail.com", 587);
        await client.AuthenticateAsync(oauth2);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
      }
      #endregion

      return Ok("OK");
    }
  }
}