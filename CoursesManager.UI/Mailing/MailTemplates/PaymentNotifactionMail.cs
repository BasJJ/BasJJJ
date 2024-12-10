using CoursesManager.MVVM.Mail.MailTemplate;
using CoursesManager.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesManager.UI.Mailing.MailTemplates
{
    internal class PaymentNotifactionMail : BaseMailTemplate
    {
        public PaymentNotifactionMail(string htmlString, Course course, Student student) : base(htmlString, course, student)
        {
        }
    }
}
