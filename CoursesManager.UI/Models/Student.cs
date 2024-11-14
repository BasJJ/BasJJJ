using CoursesManager.MVVM.Data;
using System.Collections.ObjectModel;


namespace CoursesManager.UI.Models;

public class Student : ViewModel, ICopyable<Student>
{
    public int Id { get; set; }

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

    public string PhoneNumber { get; set; }

    private string _postCode;

    public string PostCode
    {
        get => _postCode;
        set => SetProperty(ref _postCode, value);
    }

    public string _country;

    public string Country
    {
        get => _country;
        set => SetProperty(ref _country, value);
    }

    public string _city;

    public string City
    {
        get => _city;
        set => SetProperty(ref _city, value);
    }

    public string _streetname;

    public string StreetName
    {
        get => _streetname;
        set => SetProperty(ref _streetname, value);
    }

    private int _houseNumber;

    public int HouseNumber
    {
        get => _houseNumber;
        set => SetProperty(ref _houseNumber, value);
    }

    private bool _awaitingpayement;

    public bool AwaitingPayement
    {
        get => _awaitingpayement;
        set => SetProperty(ref _awaitingpayement, value);
    }

    private string _houseNumberextension;

    public string HouseNumberExtension
    {
        get => _houseNumberextension;
        set => SetProperty(ref _houseNumberextension, value);
    }

    public string TableFilter()
    {
        return $"{FirstName}{Insertion}{LastName}{Email}".Replace(" ", "");
    }

    public Student Copy()
    {
        return new Student
        {
            FirstName = this.FirstName,
            Insertion = this.Insertion,
            LastName = this.LastName,
            Email = this.Email,
            PhoneNumber = this.PhoneNumber,
            PostCode = this.PostCode,
            Country = this.Country,
            City = this.City,
            StreetName = this.StreetName,
            HouseNumber = this.HouseNumber,
        };
    }

    public DateTime DateCreated { get; set; }
    public bool Is_deleted { get; set; }
    public DateTime? date_deleted { get; set; }
}