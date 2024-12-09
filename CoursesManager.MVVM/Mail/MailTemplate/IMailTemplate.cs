using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CoursesManager.MVVM.Mail.MailTemplate
{
    public interface IMailTemplate
    {
        MailMessage GenerateMail();
    }
}
