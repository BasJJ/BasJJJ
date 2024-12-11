DELIMITER $$
CREATE DEFINER=`courses_manager`@`%` PROCEDURE `spStudents_Delete`()
BEGIN
    DELETE FROM students WHERE id = p_id;
END$$
DELIMITER ;