using CoursesManager.UI.Models;

namespace CoursesManager.UI.DataAccess;

public class LocationDataAccess(string connectionString) : BaseDataAccess<Location>(connectionString)
{
    public void Add(Location courseLocation)
    {
        throw new NotImplementedException();
    }
}