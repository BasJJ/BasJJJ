using CoursesManager.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using CoursesManager.MVVM.Mail.MailTemplate;

namespace CoursesManager.UI.Mailing.MailTemplates
{
    class BaseMailTemplate : IMailTemplate
    {


        public string HtmlString;
        public Course Course;
        public Student Student;
        //public object? Attachment; this becomes pdf attachment

        public BaseMailTemplate(string htmlString, Course course, Student student)
        {
            HtmlString = htmlString;
            Course = course;
            Student = student;
        }

        public virtual MailMessage GenerateMail()
        {
            return new MailMessage();
        }
    }
}
