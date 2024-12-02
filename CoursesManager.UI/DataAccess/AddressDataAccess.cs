using CoursesManager.UI.Models;
using MySql.Data.MySqlClient;

namespace CoursesManager.UI.DataAccess;

public class AddressDataAccess : BaseDataAccess<Address>
{
    public void Add(Address address)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (MySqlException ex)
        {
            throw new InvalidOperationException(ex.Message, ex);
        }
    }

    public void Update(Address address)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (MySqlException ex)
        {
            throw new InvalidOperationException(ex.Message, ex);
        }
    }

    public void Delete(int id)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch (MySqlException ex)
        {
            throw new InvalidOperationException(ex.Message, ex);
        }
    }
}