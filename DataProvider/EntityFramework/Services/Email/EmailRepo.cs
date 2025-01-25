using System.Net;
using System.Net.Mail;

public interface IEmailRepo
{
    Task SendEmailAsync(string toEmail, string subject, string body);
}
public class EmailRepo : IEmailRepo
{
    private readonly string _smtpServer;
    private readonly int _port;
    private readonly string _senderEmail;
    private readonly string _senderPassword;

    public EmailRepo()
    {
        _smtpServer = "smtp.mail.ir";
        _port = 587;
        _senderEmail = "bamdadtabari@mail.ir";
        _senderPassword = "QAZqaz!@#123";
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        using var smtpClient = new SmtpClient(_smtpServer, _port)
        {
            Credentials = new NetworkCredential(_senderEmail, _senderPassword),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_senderEmail, "PunckCoders"),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        await smtpClient.SendMailAsync(mailMessage);
    }
}
