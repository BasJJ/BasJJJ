using CoursesManager.UI.DataAccess;
using CoursesManager.UI.Models;

namespace CoursesManager.UI.Repositories.RegistrationRepository;

public class RegistrationRepository : BaseRepository, IRegistrationRepository
{
    private readonly RegistrationDataAccess _registrationDataAccess;

    private readonly List<Registration> _allRegistrations;

    public RegistrationRepository()
    {
        _registrationDataAccess = new RegistrationDataAccess();
        _allRegistrations = new();
    }

    public List<Registration> GetAll()
    {
        if (_allRegistrations.Count == 0 || ShouldRefresh)
        {
            return RefreshAll();
        }

        return _allRegistrations;
    }

    public List<Registration> RefreshAll()
    {
        lock (_allRegistrations)
        {
            _lastUpdated = DateTime.Now;

            _allRegistrations.Clear();

            _allRegistrations.AddRange(_registrationDataAccess.GetAll());

            return _allRegistrations;
        }
    }

    public Registration? GetById(int id)
    {
        lock (_allRegistrations)
        {
            return _allRegistrations.FirstOrDefault(reg => reg.Id == id);
        }
    }

    public void Add(Registration data)
    {
        lock (_allRegistrations)
        {
            ArgumentNullException.ThrowIfNull(data);

            _registrationDataAccess.Add(data);
            _allRegistrations.Add(data);
        }
    }

    public void Update(Registration data)
    {
        lock (_allRegistrations)
        {
            ArgumentNullException.ThrowIfNull(data);

            _registrationDataAccess.Update(data);

            if (!_allRegistrations.Contains(data))
            {
                var existing = _allRegistrations.FirstOrDefault(reg => reg.Id == data.Id);
                if (existing is not null)
                {
                    existing.CourseId = data.CourseId;
                    existing.Course = data.Course;
                    existing.StudentId = data.StudentId;
                    existing.Student = data.Student;
                    existing.RegistrationDate = data.RegistrationDate;
                    existing.PaymentStatus = data.PaymentStatus;
                    existing.IsActive = data.IsActive;
                    existing.IsAchieved = data.IsAchieved;
                }
                else
                {
                    _allRegistrations.Add(data);
                }
            }
        }
    }

    public void Delete(Registration data)
    {
        ArgumentNullException.ThrowIfNull(data);

        Delete(data.Id);
    }

    public void Delete(int id)
    {
        lock (_allRegistrations)
        {
            _registrationDataAccess.Delete(id);

            _allRegistrations.RemoveAll(reg => reg.Id == id);
        }
    }

    public List<Registration> GetAllRegistrationsByCourse(Course course)
    {
        ArgumentNullException.ThrowIfNull(course);

        return _registrationDataAccess.GetAllRegistrationsByCourse(course.Id);
    }
}
