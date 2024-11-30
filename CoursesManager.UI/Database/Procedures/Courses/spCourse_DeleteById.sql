CREATE PROCEDURE spCourse_DeleteById(IN p_id INT)
BEGIN
    DECLARE txn_started_by_me BOOLEAN DEFAULT FALSE;

    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        IF txn_started_by_me THEN
            ROLLBACK;
        END IF;

        RESIGNAL;
    END;

    -- Check if a transaction is already active
    IF @@autocommit = 1 THEN
        SET txn_started_by_me = TRUE;
        START TRANSACTION;
    END IF;

    DELETE FROM courses WHERE id = p_id;

    IF ROW_COUNT() = 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'No course was found for the specified ID.';
    END IF;

    IF txn_started_by_me THEN
        COMMIT;
    END IF;
END