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
}