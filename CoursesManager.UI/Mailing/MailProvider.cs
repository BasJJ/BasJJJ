using CoursesManager.MVVM.Mail;
using CoursesManager.UI.Models;


namespace CoursesManager.UI.Mailing
{
    public class MailProvider : IMailProvider
    {
        public byte GeneratePDF(Course course)
        {
            throw new NotImplementedException();
        }

        public Task<List<MailResult>> SendCertificates(Course course)
        {
            throw new NotImplementedException();
        }

        public Task<List<MailResult>> SendCourseStartNotifications(Course course)
        {
            throw new NotImplementedException();
        }

        public Task<List<MailResult>> SendPaymentNotifications(Course course)
        {
            throw new NotImplementedException();
        }
    }
}
