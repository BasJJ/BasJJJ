using System.Collections.ObjectModel;

namespace CoursesManager.UI.Models.Repositories.RegistrationRepository
{
    public class RegistrationRepository : IRegistrationRepository
    {
        //private readonly List<Models.Registration> _registrations = new List<Models.Registration>();
        private ObservableCollection<Registration> _registrations;
        
        public RegistrationRepository()
        {
            _registrations = App.Registrations;
        }

        public IEnumerable<Registration> GetAll()
        {
            return _registrations.ToList();
        }

        public Registration GetById(int id)
        {
            return _registrations.FirstOrDefault(r => r.ID == id);
        }

        public void Add(Registration registration)
        {
            registration.DateCreated = DateTime.Now;
            _registrations.Add(registration);
        }

        public void Update(Registration registration)
        {
            var existingRegistration = GetById(registration.ID);
            if (existingRegistration != null)
            {
                existingRegistration.StudentID = registration.StudentID;
                existingRegistration.CourseID = registration.CourseID;
                existingRegistration.RegistrationDate = registration.RegistrationDate;
                existingRegistration.PaymentStatus = registration.PaymentStatus;
                existingRegistration.IsActive = registration.IsActive;
            }
        }

        public void Delete(int id)
        {
            var registration = GetById(id);
            if (registration != null)
            {
                _registrations.Remove(registration);
            }
        }
    }
}
