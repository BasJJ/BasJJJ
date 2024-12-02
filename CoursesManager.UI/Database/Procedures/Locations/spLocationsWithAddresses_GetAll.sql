CREATE PROCEDURE spLocationsWithAddresses_GetAll()
BEGIN
    SELECT
        locations.id AS location_id,
        addresses.id AS adress_id,
        locations.name AS name,
        addresses.country,
        addresses.city,
        addresses.street,
        addresses.house_number,
        addresses.zipcode
    FROM locations
    INNER JOIN addresses ON locations.address_id = addresses.id;
END;