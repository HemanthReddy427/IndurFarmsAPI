using IndurFarmsAPI.IServices;
using System.Net;
using System.Net.Mail;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public EmailService(
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public async Task SendWelcomeEmailAsync(string email, string userName)
    {
        string templatePath = Path.Combine(
            _environment.ContentRootPath,
            "Templates",
            "WelcomeEmail.html");

        string body = await File.ReadAllTextAsync(templatePath);

        body = body.Replace("{{UserName}}", userName);
        body = body.Replace("{{CurrentYear}}", DateTime.Now.Year.ToString());

        MailMessage mail = new MailMessage();

        mail.From = new MailAddress(_configuration["EmailSettings:SenderEmail"]!);

        mail.To.Add(email);

        mail.Subject = $"Welcome to Indur Farms! {userName}";

        mail.Body = body;

        mail.IsBodyHtml = true;

        SmtpClient smtp = new SmtpClient(_configuration["EmailSettings:SmtpServer"])
        {
            Port = int.Parse(_configuration["EmailSettings:Port"]!),
            Credentials = new NetworkCredential(
                _configuration["EmailSettings:SenderEmail"],
                _configuration["EmailSettings:Password"]),
            EnableSsl = true
        };

        await smtp.SendMailAsync(mail);
    }
}