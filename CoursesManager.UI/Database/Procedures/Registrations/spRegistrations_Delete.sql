DELIMITER $$
CREATE DEFINER=`courses_manager`@`%` PROCEDURE `spRegistrations_Delete`()
BEGIN
    DELETE FROM registrations WHERE id = p_id;
END$$
DELIMITER ;