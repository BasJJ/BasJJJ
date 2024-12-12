using System;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using CoursesManager.MVVM.Env;
using CoursesManager.UI.Models;
using CoursesManager.MVVM.Mail.MailService;
using CoursesManager.MVVM.Mail;
using CoursesManager.MVVM.Mail.MailTemplate;
using ZstdSharp.Unsafe;
using System.ComponentModel.DataAnnotations;

public class MailService : IMailService
{
    private readonly SmtpConfig _smtpConfig;
    //Uncomment wanneer Encryption Service klaar is
    //private readonly EncryptionService _encryptionService;

    public MailService()
    {
        var mailConnectionString = EnvManager<EnvModel>.Values.MailConnectionString;

        if (string.IsNullOrWhiteSpace(mailConnectionString))
        {
            throw new Exception("MailConnectionString is niet ingesteld in .env");
        }

        _smtpConfig = ParseConnectionString(mailConnectionString);
    }
    public async Task<MailResult> SendMail(MailMessage mailTemplate)
    {
        try
        {
            var emailMessage = mailTemplate.GenerateMail();
            using var smtpClient = new SmtpClient(_smtpConfig.Server, _smtpConfig.Port)
            {
                Credentials = new NetworkCredential(_smtpConfig.User, _smtpConfig.Password), // _smtpConfig.Password aanpassen naar _encryptionService.Decrypt(_smtpConfig.Password) wanneer EncryptionService klaar is
                EnableSsl = _smtpConfig.EnableSsl
            };
            await smtpClient.SendMailAsync(emailMessage);
            return new MailResult
            {
                Outcome = MailOutcome.Success,
                MailMessage = emailMessage
            };
        }
        catch (Exception ex)
        {
            return new MailResult
            {
                Outcome = MailOutcome.Failure,
                MailMessage = mailTemplate.GenerateMail()
            };
        }
    }

    public async Task<List<MailResult>> SendMail(List<MailMessage> mailTemplates)
    {
        var mailResults = new List<MailResult>();

        var tasks = mailTemplates.Select(async template =>
        {
            try
            {
                var emailMessage = template.GenerateMail();

                using var smtpClient = new SmtpClient(_smtpConfig.Server, _smtpConfig.Port)
                {
                    Credentials = new NetworkCredential(_smtpConfig.User, _smtpConfig.Password), // _smtpConfig.Password aanpassen naar _encryptionService.Decrypt(_smtpConfig.Password) wanneer EncryptionService klaar is
                    EnableSsl = _smtpConfig.EnableSsl
                };

                await smtpClient.SendMailAsync(emailMessage);

                mailResults.Add(new MailResult
                {
                    Outcome = MailOutcome.Success,
                    MailMessage = emailMessage
                });
            }
            catch (Exception ex)
            {
                mailResults.Add(new MailResult
                {
                    Outcome = MailOutcome.Failure,
                    MailMessage = template.GenerateMail()
                });

                Console.WriteLine($"Fout bij verzenden: {ex.Message}");
            }
        });
        await Task.WhenAll(tasks);

        return mailResults;
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
