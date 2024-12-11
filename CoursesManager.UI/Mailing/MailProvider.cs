using CoursesManager.MVVM.Mail;
using CoursesManager.UI.Models;
using CoursesManager.UI.Repositories.RegistrationRepository;
using CoursesManager.UI.Repositories.TemplateRepository;


namespace CoursesManager.UI.Mailing
{
    public class MailProvider : IMailProvider
    {
        RegistrationRepository registrationRepository = new RegistrationRepository();
        TemplateRepository templateRepository = new TemplateRepository();
        List<Registration> courseRegistrations = new List<Registration>();

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
            courseRegistrations = registrationRepository.GetAllRegistrationsByCourse(course);
            Template template = templateRepository.GetTemplateByName("PaymentMail");
            foreach (Registration registration in courseRegistrations)
            {
                if (!registration.PaymentStatus)
                {

                }

}


            }
        }
    }