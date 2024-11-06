using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursesManager.UI.Models
{
    // This class is used to generate dummy data for the students.
    // We can use this one until we have a database to store the data.
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    namespace CoursesManager.UI.Models
    {
        public static class DummyDataGenerator
        {
            private static Random _random = new Random();

            public static ObservableCollection<Student> GenerateStudents(int count)
            {
                ObservableCollection<Student> students = new ObservableCollection<Student>();

                for (int i = 0; i < count; i++)
                {
                    Student student = new Student
                    {
                        Id = i + 1,
                        FirstName = GetRandomFirstName(),
                        Insertion = GetRandomInsertion(),
                        LastName = GetRandomLastName(),
                        Email = $"{GetRandomFirstName()}.{GetRandomLastName()}@example.com".ToLower(),
                        PhoneNumber = GenerateRandomPhoneNumber(),
                        PostCode = GenerateRandomPostCode(),
                        HouseNumber = _random.Next(1, 300),
                        HouseNumberExtension = GenerateRandomHouseNumberExtension(),
                        AwaitingPayement = _random.Next(0, 2) == 1, // Randomly true or false
                        DateCreated = DateTime.Now
                    };

                    students.Add(student);
                }

                return students;
            }

            public static ObservableCollection<Course> GenerateCourses(int count)
            {
                ObservableCollection<Course> courses = new ObservableCollection<Course>();

                for (int i = 0; i < count; i++)
                {
                    Course course = new Course
                    {
                        ID = i + 1,
                        Name = $"Course{i + 1}",
                        Code = GenerateRandomCourseCode(),
                        Description = $"Description for Course{i + 1}",
                        Participants = _random.Next(0, 100),
                        IsActive = _random.Next(0, 2) == 1, // Randomly true or false
                        IsPayed = _random.Next(0, 2) == 1, // Randomly true or false
                        Category = $"Category{i % 3 + 1}",
                        StartDate = DateTime.Now.AddDays(_random.Next(1, 30)),
                        EndDate = DateTime.Now.AddDays(_random.Next(31, 60)),
                        LocationId = _random.Next(1, count + 1),
                        DateCreated = DateTime.Now
                    };

                    courses.Add(course);
                }

                return courses;
            }

            public static ObservableCollection<Location> GenerateLocations(int count)
            {
                ObservableCollection<Location> locations = new ObservableCollection<Location>();

                for (int i = 0; i < count; i++)
                {
                    Location location = new Location
                    {
                        Id = i + 1,
                        Name = $"Location{i + 1}",
                        Address = $"Address for Location{i + 1}",
                        Capacity = $"{_random.Next(10, 100)}",
                        DateCreated = DateTime.Now
                    };

                    locations.Add(location);
                }

                return locations;
            }

            public static ObservableCollection<Registration> GenerateRegistrations(int studentCount, int courseCount)
            {
                ObservableCollection<Registration> registrations = new ObservableCollection<Registration>();

                for (int i = 0; i < studentCount; i++)
                {
                    Registration registration = new Registration
                    {
                        ID = i + 1,
                        StudentID = _random.Next(1, studentCount + 1),
                        CourseID = _random.Next(1, courseCount + 1),
                        RegistrationDate = DateTime.Now.AddDays(-_random.Next(1, 30)),
                        PaymentStatus = _random.Next(0, 2) == 1,
                        IsActive = _random.Next(0, 2) == 1,
                        DateCreated = DateTime.Now
                    };

                    registrations.Add(registration);
                }

                return registrations;
            }

            private static string GetRandomFirstName()
            {
                string[] firstNames =
                    { "John", "Jane", "Michael", "Emily", "Chris", "Anna", "David", "Sarah", "Robert", "Emma" };
                return firstNames[_random.Next(firstNames.Length)];
            }

            private static string GetRandomInsertion()
            {
                string[] insertions = { "", "van", "de", "der", "von", "van der" };
                return insertions[_random.Next(insertions.Length)];
            }

            private static string GetRandomLastName()
            {
                string[] lastNames =
                {
                    "Smith", "Johnson", "Brown", "Williams", "Jones", "Garcia", "Miller", "Davis", "Rodriguez",
                    "Martinez"
                };
                return lastNames[_random.Next(lastNames.Length)];
            }

            private static string GenerateRandomPhoneNumber()
            {
                return $"+1-{_random.Next(100, 999)}-{_random.Next(100, 999)}-{_random.Next(1000, 9999)}";
            }

            private static string GenerateRandomPostCode()
            {
                string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                return
                    $"{_random.Next(1000, 9999)} {letters[_random.Next(letters.Length)]}{letters[_random.Next(letters.Length)]}";
            }

            private static string GenerateRandomHouseNumberExtension()
            {
                string[] extensions = { "", "A", "B", "C", "D", "E" };
                return extensions[_random.Next(extensions.Length)];
            }

            private static string GenerateRandomCourseCode()
            {
                string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                return
                    $"{letters[_random.Next(letters.Length)]}{letters[_random.Next(letters.Length)]}{letters[_random.Next(letters.Length)]}.{letters[_random.Next(letters.Length)]}{letters[_random.Next(letters.Length)]}";
            }
        }
    }
}
