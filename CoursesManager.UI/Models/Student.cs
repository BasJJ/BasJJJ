using CoursesManager.MVVM.Data;

namespace CoursesManager.UI.Models;

public class Student : ViewModel
{
    private string _firstName;

    public string FirstName
    {
        get => _firstName;
        set => SetProperty(ref _firstName, value);
    }

    private string _insertion;

    public string Insertion
    {
        get => _insertion;
        set => SetProperty(ref _insertion, value);
    }

    private string _lastName;

    public string LastName
    {
        get => _lastName;
        set => SetProperty(ref _lastName, value);
    }

    private string _email;

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    private string _phoneNumber;

    public string PhoneNumber
    {
        get => _phoneNumber;
        set => SetProperty(ref _phoneNumber, value);
    }

    private string _postCode;

    public string PostCode
    {
        get => _postCode;
        set => SetProperty(ref _postCode, value);
    }

    private int _houseNumber;

    public int HouseNumber
    {
        get => _houseNumber;
        set => SetProperty(ref _houseNumber, value);
    }

    private string _houseNumberExtension;

    public string HouseNumberExtension
    {
        get => _houseNumberExtension;
        set => SetProperty(ref _houseNumberExtension, value);
    }

    private bool _awaitingPayement;

    public bool AwaitingPayement
    {
        get => _awaitingPayement;
        set => SetProperty(ref _awaitingPayement, value);
    }

    public string TableFilter()
    {
        return $"{FirstName}{Insertion}{LastName}{Email}".Replace(" ", "");
    }
}