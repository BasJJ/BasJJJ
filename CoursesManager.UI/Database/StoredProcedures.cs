namespace CoursesManager.UI.Database;

public static class StoredProcedures
{
    #region Courses

    public const string CoursesDeleteById = "spCourses_DeleteById";

    #endregion

    #region Locations

    public const string LocationsWithAddressesGetAll = "spLocationsWithAddresses_GetAll";
    public const string LocationsInsert = "spLocations_Insert";
    public const string LocationsDeleteById = "spLocations_DeleteById";

    #endregion

    #region Registrations

    public const string RegistrationsGetByCourseId = "spRegistrations_GetByCourseId";

    #endregion

    #region Addresses

    public const string AddAddress = "spAddresses_Add";
    public const string GetAddressById = "spAddresses_GetById";
    public const string GetAllAddresses = "spAddresses_GetAll";
    public const string UpdateAddress = "spAddresses_Update";
    public const string DeleteAddress = "spAddresses_Delete";

    #endregion

    #region Students

    public const string AddStudent = "spStudents_Add";
    public const string GetStudentById = "spStudents_GetById";
    public const string GetAllStudents = "spStudents_GetAll";
    public const string GetNotDeletedStudents = "spStudents_GetNotDeleted";
    public const string GetDeletedStudents = "spStudents_GetDeleted";

    #endregion
}