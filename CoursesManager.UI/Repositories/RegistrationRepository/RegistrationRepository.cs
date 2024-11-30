using CoursesManager.UI.DataAccess;
using CoursesManager.UI.Models;

namespace CoursesManager.UI.Repositories.RegistrationRepository;

public class RegistrationRepository : IRegistrationRepository
{
    private readonly RegistrationDataAccess _registrationDataAccess = new();

    public List<Registration> GetAll()
    {
        throw new NotImplementedException();
    }

    public Registration? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public void Add(Registration data)
    {
        throw new NotImplementedException();
    }

    public void Update(Registration data)
    {
        throw new NotImplementedException();
    }

    public void Delete(Registration data)
    {
        throw new NotImplementedException();
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public List<Registration> GetAllRegistrationsByCourse(Course course)
    {
        return _registrationDataAccess.GetAllRegistrationsByCourse(course.ID);
    }
}