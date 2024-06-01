using System;
using System.Net;
using System.Net.Mail;

public class EmailService
{
    private readonly string _smtpServer = "smtp.gmail.com";
    private readonly int _smtpPort = 587;
    private readonly string _smtpUsername = "agricontwebb@gmail.com";
    private readonly string _smtpPassword = "ggcwxwokkfbkjlbq";

    public void SendEmail(string toEmail, string subject, string body)
    {
        try
        {
            using (var message = new MailMessage(_smtpUsername, toEmail))
            {
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                using (var client = new SmtpClient(_smtpServer, _smtpPort))
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
                    client.EnableSsl = true;

                    client.Send(message);
                }
            }
            Console.WriteLine("Email enviado com sucesso para " + toEmail);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao enviar email: " + ex.Message);
            throw;
        }
    }
}
