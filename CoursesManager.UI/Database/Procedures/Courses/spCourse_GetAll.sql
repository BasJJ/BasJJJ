CREATE PROCEDURE spCourses_GetAll()
BEGIN
    SELECT 
        id,
        name,
        code,
        description,
        location_id,
        is_active,
        start_date,
        end_date,
        created_at,
        tile_image
    FROM courses;
END;
