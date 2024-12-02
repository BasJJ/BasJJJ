CREATE PROCEDURE spRegistrations_GetByCourseId(IN p_courseId INT)
BEGIN
    SELECT id, course_id, student_id, registration_date, payment_status, is_active, is_achieved
    FROM registrations
    WHERE course_id = p_courseId;
END;