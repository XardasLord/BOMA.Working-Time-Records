using System.Text.RegularExpressions;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BOMA.WTR.Infrastructure.Identity.Email;

// public class EmailSender : IEmailSender
// {
//     private readonly EmailServerConfiguration _configuration;
//
//     public EmailSender(IOptions<EmailServerConfiguration> configuration)
//     {
//         _configuration = configuration.Value;
//     }
//     
//     public async Task SendEmailAsync(string email, string subject, string htmlMessage)
//     {
//         using var client = new SmtpClient();
//         client.ServerCertificateValidationCallback = (s, c, h, e) => true;
//
//         await client.ConnectAsync(
//             _configuration.Server, 
//             _configuration.Port, 
//             _configuration.UseTls ? SecureSocketOptions.StartTls : SecureSocketOptions.None);
//
//         if (_configuration.UseAuthentication)
//             await client.AuthenticateAsync(_configuration.UserName, _configuration.Password);
//
//         var message = PrepareMessage(email, subject, htmlMessage);
//
//         await client.SendAsync(message);
//         await client.DisconnectAsync(true);
//     }
//     
//     private MimeMessage PrepareMessage(string email, string subject, string htmlMessage)
//     {
//         var message = new MimeMessage();
//
//         message.From.Add(new MailboxAddress(_configuration.FromEmail, _configuration.FromEmail));
//         message.To.Add(new MailboxAddress(email, email));
//         message.Subject = subject;
//
//         var bodyBuilder = new BodyBuilder
//         {
//             HtmlBody = htmlMessage,
//             TextBody = RemoveHtmlTags(htmlMessage)
//         };
//
//         message.Body = bodyBuilder.ToMessageBody();
//
//         return message;
//     }
//
//     private static string RemoveHtmlTags(string htmlCode) =>
//         Regex.Replace(htmlCode, "<[^>]*>", "");
// }