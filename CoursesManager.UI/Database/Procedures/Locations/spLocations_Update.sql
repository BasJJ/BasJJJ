CREATE PROCEDURE spLocations_Update(
    IN p_location_id INT,
    IN p_new_name varchar(255),
    IN p_new_address_id INT
)
BEGIN
    DECLARE txn_started_by_me BOOLEAN DEFAULT FALSE;
    DECLARE set_clause TEXT;
    DECLARE sql_query TEXT;

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

    SET set_clause = '';

    IF p_new_name IS NOT NULL THEN
        SET set_clause = CONCAT(set_clause, 'name = "', p_new_name, '", ');
    END IF;

    IF p_new_address_id IS NOT NULL THEN
        SET set_clause = CONCAT(set_clause, 'address_id = ', p_new_address_id, ', ');
    END IF;

    SET set_clause = LEFT(set_clause, LENGTH(set_clause) - 2);

    IF LENGTH(set_clause) = 0 THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'No fields to update.';
    END IF;

    SET sql_query = CONCAT('UPDATE locations SET ', set_clause, ', updated_at = NOW() WHERE id = ', p_location_id);

    PREPARE stmt FROM sql_query;
    EXECUTE stmt;
    DEALLOCATE PREPARE stmt;

    IF txn_started_by_me THEN
        COMMIT;
    END IF;
END;