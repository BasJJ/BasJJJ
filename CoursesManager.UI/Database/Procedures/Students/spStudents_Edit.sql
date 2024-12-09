DELIMITER $$
CREATE DEFINER=`courses_manager`@`%` PROCEDURE `spStudents_Edit`(
    IN p_id INT,
    IN p_first_name VARCHAR(255),
    IN p_last_name VARCHAR(255),
    IN p_email VARCHAR(255),
    IN p_phone VARCHAR(20),
    IN p_address_id INT,
    IN p_is_deleted BOOLEAN,
    IN p_deleted_at DATETIME,
    IN p_created_at DATETIME,
    IN p_updated_at DATETIME,
    IN p_insertion VARCHAR(255),
    IN p_date_of_birth DATE
)
BEGIN
    UPDATE students
    SET
        first_name = p_first_name,
        last_name = p_last_name,
        email = p_email,
        phone = p_phone,
        address_id = p_address_id,
        is_deleted = p_is_deleted,
        deleted_at = p_deleted_at,
        created_at = p_created_at,
        updated_at = p_updated_at,
        insertion = p_insertion,
        date_of_birth = p_date_of_birth
    WHERE id = p_id;
END$$
DELIMITER ;