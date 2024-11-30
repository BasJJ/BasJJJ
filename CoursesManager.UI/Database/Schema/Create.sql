CREATE TABLE adresses (
    id INT AUTO_INCREMENT
        PRIMARY KEY,
    country VARCHAR(255) NOT NULL,
    city VARCHAR(255) NOT NULL,
    street VARCHAR(255) NOT NULL,
    house_number VARCHAR(10) NOT NULL,
    zipcode VARCHAR(10) NOT NULL,
    created_at DATETIME NOT NULL,
    updated_at DATETIME NOT NULL,
    CONSTRAINT id_UNIQUE UNIQUE (id)
);

CREATE TABLE locations (
    id INT AUTO_INCREMENT
        PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    adress_id INT NOT NULL,
    created_at DATETIME NOT NULL,
    updated_at DATETIME NOT NULL,
    CONSTRAINT id_UNIQUE UNIQUE (id),
    CONSTRAINT adress_id_fk FOREIGN KEY (adress_id) REFERENCES adresses (id)
);

CREATE TABLE courses (
    id INT AUTO_INCREMENT
        PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    code VARCHAR(255) NOT NULL,
    description TEXT NOT NULL,
    location_id INT NOT NULL,
    is_active TINYINT DEFAULT 1 NOT NULL,
    tile_image BLOB NULL,
    start_date DATE NOT NULL,
    end_date DATE NOT NULL,
    created_at DATETIME NOT NULL,
    updated_at DATETIME NOT NULL,
    CONSTRAINT id_UNIQUE UNIQUE (id),
    CONSTRAINT course_code_UNIQUE UNIQUE (code),
    CONSTRAINT location_id_fk FOREIGN KEY (location_id) REFERENCES locations (id)
);

CREATE INDEX location_id_fk_idx
    ON courses (location_id);

CREATE INDEX adress_id_fk_idx
    ON locations (adress_id);

CREATE TABLE students (
    id INT AUTO_INCREMENT
        PRIMARY KEY,
    firstname VARCHAR(255) NOT NULL,
    lastname VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL,
    phone VARCHAR(255) NULL,
    adress_id INT NOT NULL,
    is_deleted TINYINT DEFAULT 0 NOT NULL,
    deleted_at DATETIME NULL,
    created_at DATETIME NOT NULL,
    updated_at DATETIME NOT NULL,
    insertion VARCHAR(45) NULL,
    CONSTRAINT email_UNIQUE UNIQUE (email),
    CONSTRAINT id_UNIQUE UNIQUE (id),
    CONSTRAINT adress_id_fk2 FOREIGN KEY (adress_id) REFERENCES adresses (id)
);

CREATE TABLE registrations (
    id INT AUTO_INCREMENT
        PRIMARY KEY,
    course_id INT NOT NULL,
    student_id INT NOT NULL,
    registration_date DATE NOT NULL,
    payment_status TINYINT DEFAULT 0 NOT NULL,
    is_active TINYINT DEFAULT 1 NOT NULL,
    is_achieved TINYINT DEFAULT 0 NOT NULL,
    created_at DATETIME NOT NULL,
    updated_at DATETIME NOT NULL,
    CONSTRAINT id_UNIQUE UNIQUE (id),
    CONSTRAINT registraton_UNIQUE UNIQUE (student_id, course_id),
    CONSTRAINT course_id_fk FOREIGN KEY (course_id) REFERENCES courses (id),
    CONSTRAINT student_id_fk FOREIGN KEY (student_id) REFERENCES students (id)
);

CREATE INDEX course_id_fk_idx
    ON registrations (course_id);

CREATE INDEX student_id_fk_idx
    ON registrations (student_id);

CREATE INDEX adress_id_fk_idx
    ON students (adress_id);
