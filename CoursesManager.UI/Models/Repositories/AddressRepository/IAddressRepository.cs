namespace CoursesManager.UI.Models.Repositories.AddressRepository;

public interface IAddressRepository
{
    IEnumerable<Address> GetAll();

    Address GetById(int id);

    void Add(Address course);

    void Update(Address course);

    void Delete(int id);
}