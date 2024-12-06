using CoursesManager.MVVM.Mail.MailTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesManager.MVVM.Mail.MailService
{
    public interface IMailService
    {
        Task<MailResult> SendMail(IMailTemplate mailTemplate);
        Task<List<MailResult>> SendMail(IEnumerable<IMailTemplate> mailTemplates);
    }
}
