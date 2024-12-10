using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Media.Imaging;

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
                    Id = i,
                    FirstName = GetRandomFirstName(),
                    Insertion = GetRandomInsertion(),
                    LastName = GetRandomLastName(),
                    Email = $"{GetRandomFirstName()}.{GetRandomLastName()}@example.com".ToLower(),
                    Phone = GenerateRandomPhoneNumber(),
                    Address = GenerateRandomAddress(),
                    CreatedAt = DateTime.Now,
                    IsDeleted = _random.Next(0, 2) == 1,
                    DateOfBirth = DateTime.Now.AddYears(-_random.Next(18, 30)),
                    Registrations = new ObservableCollection<Registration>()
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
                    Id = i,
                    Name = GenerateCourseName(i),
                    Code = GetCourseCode(GenerateCourseName(i)),
                    Description = GetCourseDescription(GenerateCourseName(i)),
                    Participants = 0,
                    IsActive = _random.Next(0, 2) == 1,
                    IsPayed = false,
                    Category = $"Category{i % 3}",
                    StartDate = DateTime.Now.AddDays(_random.Next(15, 60)),
                    EndDate = DateTime.Now.AddDays(_random.Next(91, 180)),
                    Location = GenerateRandomLocation(),
                    DateCreated = DateTime.Now,
                    Students = new ObservableCollection<Student>(),
                    Image = GetImage()
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
                    Address = GenerateRandomAddress()
                };

                locations.Add(location);
            }

            return locations;
        }

        public static ObservableCollection<Registration> GenerateRegistrations(ObservableCollection<Student> students, ObservableCollection<Course> courses)
        {
            ObservableCollection<Registration> registrations = new ObservableCollection<Registration>();

            for (int i = 0; i < 5; i++) {
                foreach (var student in students)
                {
                    int courseId = _random.Next(0, courses.Count);
                    Course course = courses[courseId];

                    Registration registration = new Registration
                    {
                        Id = registrations.Count,
                        StudentId = student.Id,
                        Student = student,
                        CourseId = course.Id,
                        Course = course,
                        RegistrationDate = DateTime.Now.AddDays(-_random.Next(1, 30)),
                        PaymentStatus = _random.Next(0, 2) == 1,
                        IsActive = _random.Next(0, 2) == 1,
                        IsAchieved = _random.Next(0, 2) == 1
                    };

                    student.Registrations.Add(registration);
                    course.Students.Add(student);
                    registrations.Add(registration);
                }
            }

            return registrations;
        }

        private static Address GenerateRandomAddress()
        {
            return new Address
            {
                Id = _random.Next(1, 1000),
                Country = "Nederland",
                ZipCode = GenerateRandomPostCode(),
                City = $"City{_random.Next(1, 100)}",
                Street = $"Street{_random.Next(1, 100)}",
                HouseNumber = _random.Next(1, 300).ToString(),
                HouseNumberExtension = GenerateRandomHouseNumberExtension(),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }

        private static Location GenerateRandomLocation()
        {
            return new Location
            {
                Id = _random.Next(1, 1000),
                Name = $"Location{_random.Next(1, 100)}",
                Address = GenerateRandomAddress()
            };
        }

        private static string GetRandomFirstName()
        {
            string[] firstNames = { "Noah", "Emma", "Olivia", "Julia", "Chris", "Lucas", "David", "Sarah", "Robert", "Daan" };
            return firstNames[_random.Next(firstNames.Length)];
        }

        private static string GetRandomInsertion()
        {
            string[] insertions = { "", "", "", "van", "de", "van der" };
            return insertions[_random.Next(insertions.Length)];
        }

        private static string GetRandomLastName()
        {
            string[] lastNames = { "Jong", "Jansen", "Vries", "Berg", "Dijk", "Bakker", "Janssen", "Visser", "Smit", "Meijer" };
            return lastNames[_random.Next(lastNames.Length)];
        }

        private static string GenerateRandomPhoneNumber()
        {
            return $"+1-{_random.Next(100, 999)}-{_random.Next(100, 999)}-{_random.Next(1000, 9999)}";
        }

        private static string GenerateRandomPostCode()
        {
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return $"{_random.Next(1000, 9999)} {letters[_random.Next(letters.Length)]}{letters[_random.Next(letters.Length)]}";
        }

        private static string GenerateRandomHouseNumberExtension()
        {
            string[] extensions = { "", "A", "B", "C", "D", "E" };
            return extensions[_random.Next(extensions.Length)];
        }

        private static string GenerateCourseName(int i)
        {
            string[] courseNames = {
                "Interculturele Communicatie", "Toegepaste Statistiek", "Bedrijfsethiek en Duurzaamheid",
                "Onderzoeksmethoden en Tools", "Strategische Planning", "Kennisbeheer en Innovatie",
                "Professionele Vaardigheden", "Data-analyse en Visualisatie", "Wereldwijde Trends en Ontwikkelingen",
                "Digitale Transformatie", "Organisatie en Leiderschap", "Creatief Denken en Probleemoplossing",
                "Ontwikkeling van Projectvoorstellen", "Wet- en Regelgeving", "Multidisciplinaire Samenwerking",
                "Klantgerichtheid en Strategie", "Risicoanalyse en Management", "Cultureel Bewustzijn",
                "Efficiëntie in Teamverband", "Ethische Besluitvorming", "Visie en Toekomststrategie",
                "Digitalisering en IT-toepassingen", "Human Resources Management", "Marketing en Branding",
                "Duurzame Innovatie", "Professionele Netwerken en Relaties", "Procesoptimalisatie",
                "Effectieve Communicatietechnieken", "Analyseren en Rapporteren", "Onderzoek naar Nieuwe Technologieën"
            };
            return courseNames[i % courseNames.Length];
        }

        private static readonly Dictionary<string, string> CourseNameToCodeMap = new Dictionary<string, string>
        {
            { "Interculturele Communicatie", "ICT.IO" },
            { "Toegepaste Statistiek", "ICT.TS" },
            { "Bedrijfsethiek en Duurzaamheid", "ICT.BD" },
            { "Onderzoeksmethoden en Tools", "ICT.OT" },
            { "Strategische Planning", "ICT.SP" },
            { "Kennisbeheer en Innovatie", "ICT.KI" },
            { "Professionele Vaardigheden", "ICT.PV" },
            { "Data-analyse en Visualisatie", "ICT.DV" },
            { "Wereldwijde Trends en Ontwikkelingen", "ICT.WT" },
            { "Digitale Transformatie", "ICT.DT" },
            { "Organisatie en Leiderschap", "ICT.OL" },
            { "Creatief Denken en Probleemoplossing", "ICT.CD" },
            { "Ontwikkeling van Projectvoorstellen", "ICT.OP" },
            { "Wet- en Regelgeving", "ICT.WR" },
            { "Multidisciplinaire Samenwerking", "ICT.MS" },
            { "Klantgerichtheid en Strategie", "ICT.KS" },
            { "Risicoanalyse en Management", "ICT.RM" },
            { "Cultureel Bewustzijn", "ICT.CB" },
            { "Efficiëntie in Teamverband", "ICT.ET" },
            { "Ethische Besluitvorming", "ICT.EB" },
            { "Visie en Toekomststrategie", "ICT.VT" },
            { "Digitalisering en IT-toepassingen", "ICT.DI" },
            { "Human Resources Management", "ICT.HR" },
            { "Marketing en Branding", "ICT.MB" },
            { "Duurzame Innovatie", "ICT.DI" },
            { "Professionele Netwerken en Relaties", "ICT.PN" },
            { "Procesoptimalisatie", "ICT.PO" },
            { "Effectieve Communicatietechnieken", "ICT.EC" },
            { "Analyseren en Rapporteren", "ICT.AR" },
            { "Onderzoek naar Nieuwe Technologieën", "ICT.ON" }
        };

        private static string GetCourseCode(string courseName)
        {
            if (CourseNameToCodeMap.TryGetValue(courseName, out string code))
            {
                return code;
            }
            return "Code not found";
        }

        private static readonly Dictionary<string, string> CourseNameToDescriptionMap = new Dictionary<string, string>
        {
            { "Interculturele Communicatie", "Description for Interculturele Communicatie" },
            { "Toegepaste Statistiek", "Description for Toegepaste Statistiek" },
            { "Bedrijfsethiek en Duurzaamheid", "Description for Bedrijfsethiek en Duurzaamheid" },
            { "Onderzoeksmethoden en Tools", "Description for Onderzoeksmethoden en Tools" },
            { "Strategische Planning", "Description for Strategische Planning" },
            { "Kennisbeheer en Innovatie", "Description for Kennisbeheer en Innovatie" },
            { "Professionele Vaardigheden", "Description for Professionele Vaardigheden" },
            { "Data-analyse en Visualisatie", "Description for Data-analyse en Visualisatie" },
            { "Wereldwijde Trends en Ontwikkelingen", "Description for Wereldwijde Trends en Ontwikkelingen" },
            { "Digitale Transformatie", "Description for Digitale Transformatie" },
            { "Organisatie en Leiderschap", "Description for Organisatie en Leiderschap" },
            { "Creatief Denken en Probleemoplossing", "Description for Creatief Denken en Probleemoplossing" },
            { "Ontwikkeling van Projectvoorstellen", "Description for Ontwikkeling van Projectvoorstellen" },
            { "Wet- en Regelgeving", "Description for Wet- en Regelgeving" },
            { "Multidisciplinaire Samenwerking", "Description for Multidisciplinaire Samenwerking" },
            { "Klantgerichtheid en Strategie", "Description for Klantgerichtheid en Strategie" },
            { "Risicoanalyse en Management", "Description for Risicoanalyse en Management" },
            { "Cultureel Bewustzijn", "Description for Cultureel Bewustzijn" },
            { "Efficiëntie in Teamverband", "Description for Efficiëntie in Teamverband" },
            { "Ethische Besluitvorming", "Description for Ethische Besluitvorming" },
            { "Visie en Toekomststrategie", "Description for Visie en Toekomststrategie" },
            { "Digitalisering en IT-toepassingen", "Description for Digitalisering en IT-toepassingen" },
            { "Human Resources Management", "Description for Human Resources Management" },
            { "Marketing en Branding", "Description for Marketing en Branding" },
            { "Duurzame Innovatie", "Description for Duurzame Innovatie" },
            { "Professionele Netwerken en Relaties", "Description for Professionele Netwerken en Relaties" },
            { "Procesoptimalisatie", "Description for Procesoptimalisatie" },
            { "Effectieve Communicatietechnieken", "Description for Effectieve Communicatietechnieken" },
            { "Analyseren en Rapporteren", "Description for Analyseren en Rapporteren" },
            { "Onderzoek naar Nieuwe Technologieën", "Description for Onderzoek naar Nieuwe Technologieën" }
        };

        private static string GetCourseDescription(string courseName)
        {
            if (CourseNameToDescriptionMap.TryGetValue(courseName, out string description))
            {
                return description;
            }
            return "Description not found";
        }

        private static BitmapImage GetImage()
        {
            BitmapImage image = new BitmapImage();
            try
            {
                image = LoadImage($"Resources/Images/picture{_random.Next(0, 9)}.jpg");
            }
            catch (Exception)
            {
                return null;
            }

            return image;
        }

        private static BitmapImage LoadImage(string relativePath)
        {
            var uri = new Uri($"pack://application:,,,/{relativePath}", UriKind.Absolute);
            return new BitmapImage(uri);
        }
    }
}