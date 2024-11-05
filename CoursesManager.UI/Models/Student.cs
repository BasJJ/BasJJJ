﻿using CoursesManager.MVVM.Data;
using System.Collections.ObjectModel;

namespace CoursesManager.UI.Models;

public class Student : ViewModel
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
    public string City { get; set; }
    public DateTime DateCreated { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DateDeleted { get; set; }

    private ObservableCollection<Course> _courses = new ObservableCollection<Course>();
    public ObservableCollection<Course> Courses
    {
        get => _courses;
        set => SetProperty(ref _courses, value);
    }
}