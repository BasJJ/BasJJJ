using System;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using CoursesManager.MVVM.Env;
using CoursesManager.UI.Models;

public class MailService
{
    private readonly SmtpConfig _smtpConfig;

    public MailService()
    {
        // Haal de MailConnectionString op via EnvManager
        var mailConnectionString = EnvManager<EnvModel>.Values.MailConnectionString;

        if (string.IsNullOrWhiteSpace(mailConnectionString))
        {
            throw new Exception("MailConnectionString is niet ingesteld in .env");
        }

        _smtpConfig = ParseConnectionString(mailConnectionString);
    }

    public void SendEmail(string toEmail, string subject, string body)
    {
        if (_smtpConfig == null)
        {
            throw new Exception("SMTP-configuratie is niet geladen.");
        }

        try
        {
            using (var smtpClient = new SmtpClient(_smtpConfig.Server, _smtpConfig.Port))
            {
                smtpClient.Credentials = new NetworkCredential(_smtpConfig.User, _smtpConfig.Password);
                smtpClient.EnableSsl = _smtpConfig.EnableSsl;

                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(_smtpConfig.User),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true // Stel in op true als de body HTML bevat
                };

                mail.To.Add(toEmail);

                smtpClient.Send(mail);

                Console.WriteLine("E-mail succesvol verzonden.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"E-mail verzenden mislukt: {ex.Message}");
        }
    }

    private SmtpConfig ParseConnectionString(string connectionString)
    {
        var settings = ConnectionStringParser.Parse(connectionString);
        return new SmtpConfig
        {
            Server = settings["Server"],
            Port = int.Parse(settings["Port"]),
            User = settings["User"],
            Password = settings["Password"],
            EnableSsl = bool.Parse(settings["EnableSsl"])
        };
    }

    private static class ConnectionStringParser
    {
        public static Dictionary<string, string> Parse(string connectionString)
        {
            var parameters = new Dictionary<string, string>();
            var pairs = connectionString.Split(';', StringSplitOptions.RemoveEmptyEntries);

            foreach (var pair in pairs)
            {
                var keyValue = pair.Split('=', StringSplitOptions.RemoveEmptyEntries);
                if (keyValue.Length == 2)
                {
                    parameters[keyValue[0].Trim()] = keyValue[1].Trim();
                }
            }

            return parameters;
        }
    }
}
