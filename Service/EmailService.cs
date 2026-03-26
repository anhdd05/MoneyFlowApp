using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration; 
using MimeKit;

namespace Services;

public class EmailService
{
    public EmailService() { }

    public async Task<bool> SendEmail(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("MoneyFlow", "mocbx12345@gmail.com"));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var client = new SmtpClient();
        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

        try
        {
            // Kết nối đến server Gmail
            await client.ConnectAsync("smtp.gmail.com", 465, true);

            // Xác thực tài khoản bằng App Password
            await client.AuthenticateAsync("mocbx12345@gmail.com", "rrmv mscm owsd yrxh");

            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            return true; // Gửi thành công
        }
        catch (MailKit.Security.AuthenticationException)
        {
            Console.WriteLine("LỖI: Sai mật khẩu App Password hoặc tài khoản Mail.");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine("LỖI HỆ THỐNG: " + ex.Message);
            return false;
        }
    }
}