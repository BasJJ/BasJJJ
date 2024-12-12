using CoursesManager.MVVM.Exceptions;
using CoursesManager.MVVM.Mail;
using CoursesManager.UI.Models;
using CoursesManager.UI.Repositories.RegistrationRepository;
using CoursesManager.UI.Repositories.TemplateRepository;
using System.Net.Mail;


namespace CoursesManager.UI.Mailing
{
    public class MailProvider : IMailProvider
    {
        MailService mailService = new MailService();
        RegistrationRepository registrationRepository = new RegistrationRepository();
        TemplateRepository templateRepository = new TemplateRepository();
        List<Registration> courseRegistrations = new List<Registration>();
        List<MailMessage> messages = new List<MailMessage>();
        List<MailResult> mailResults = new List<MailResult>();

        public byte GeneratePDF(Course course)
        {
            throw new NotImplementedException();
        }

        public Task<List<MailResult>> SendCertificates(Course course)
        {
            Template template = templateRepository.GetTemplateByName("CertificateMail");

            try
            {
                foreach (Student student in course.Students)
                {
                    template.HtmlString = FillNameAndCourseTemplate(template.HtmlString, $"{student.FirstName} {student.LastName}", course.Name);
                    MailMessage message = new MailMessage();
                    message.To.Add(student.Email);
                    message.Subject = template.SubjectString;
                    message.Body = template.HtmlString;
                    //message.Attachments = Certificate;
                    messages.Add(message);
                }
                if (messages.Any())
                {
                    await mailService.SendMail(messages);
                }
            }
            catch (Exception ex)
            {
            }
            return mailResults;
        }

        public async Task<List<MailResult>> SendCourseStartNotifications(Course course)
        {
            Template template = templateRepository.GetTemplateByName("CourseStartMail");

            try
            {
                foreach (Student student in course.Students)
                {
                    template.HtmlString = FillNameAndCourseTemplate(template.HtmlString, $"{student.FirstName} {student.LastName}", course.Name);
                    MailMessage message = new MailMessage();
                    message.To.Add(student.Email);
                    message.Subject = template.SubjectString;
                    message.Body = template.HtmlString;
                    messages.Add(message);
                }
                if (messages.Any())
                {
                    await mailService.SendMail(messages);
                }
            }
            catch (Exception ex)
            {
            }
            return mailResults;
        }

        public async Task<List<MailResult>> SendPaymentNotifications(Course course)
        {
            courseRegistrations = registrationRepository.GetAllRegistrationsByCourse(course);
            Template template = templateRepository.GetTemplateByName("PaymentMail");
            try
            {
                foreach (Registration registration in courseRegistrations)
                {

                    if (!registration.PaymentStatus)
                    {
                        Student student = course.Students.FirstOrDefault(s => s.Id == registration.Student.Id);
                        template.HtmlString = FillPaymentTemplate(template.HtmlString, $"{student.FirstName} {student.LastName}", course.Name, "iets.nl");
                        MailMessage message = new MailMessage();
                        message.To.Add(student.Email);
                        message.Subject = template.SubjectString;
                        message.Body = template.HtmlString;
                        messages.Add(message);
                    }
                }

                if (messages.Any())
                {
                    await mailService.SendMail(messages);
                }
                return mailResults;
            }
            catch (Exception ex)
            {
                throw new SomethingWentAllToShiiiiitException();
            }
        }

        private string FillNameAndCourseTemplate(string template, string name, string courseName)
        {
            template = template.Replace("[Naam]", name);
            template = template.Replace("[Cursusnaam]", courseName);

            return template;
        }

        private string FillPaymentTemplate(string template, string name, string courseName, string URL)
        {
            template = template.Replace("[Naam]", name);
            template = template.Replace("[Cursusnaam]", courseName);
            template = template.Replace("[Betaal Link]", URL);

            return template;
        }
    }
}
