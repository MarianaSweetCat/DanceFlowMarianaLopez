CREATE DATABASE DanceFlow;

CREATE TABLE Students
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Age INT NOT NULL CHECK (Age BETWEEN 3 AND 100),
    Phone VARCHAR(10) NOT NULL,
    Email VARCHAR(150) NOT NULL UNIQUE
);

CREATE TABLE DanceClasses
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Instructor VARCHAR(100) NOT NULL,
    Schedule VARCHAR(100) NOT NULL,
    Capacity INT NOT NULL CHECK (Capacity > 0)
);

CREATE TABLE Inscriptions
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StudentId INT NOT NULL,
    DanceClassId INT NOT NULL,
    InscriptionDate DATE NOT NULL,

    CONSTRAINT FK_Inscriptions_Students
        FOREIGN KEY (StudentId) REFERENCES Students(Id),

    CONSTRAINT FK_Inscriptions_DanceClasses
        FOREIGN KEY (DanceClassId) REFERENCES DanceClasses(Id),

    CONSTRAINT UQ_Student_Class
        UNIQUE (StudentId, DanceClassId)
);

INSERT INTO Students (Name, Age, Phone, Email)
VALUES
('Laura Martinez', 20, '8095551234', 'laura@gmail.com'),
('Daniel Rodriguez', 24, '8295556789', 'daniel@gmail.com'),
('Sofia Hernandez', 19, '8495554321', 'sofia@gmail.com'),
('Carlos Perez', 28, '8095559876', 'carlos@gmail.com');

INSERT INTO DanceClasses (Name, Instructor, Schedule, Capacity)
VALUES
('Ballet', 'Maria Torres', 'Monday and Wednesday - 9:00 AM', 15),
('Salsa', 'Pedro Ramirez', 'Tuesday and Thursday - 3:00 PM', 12),
('Hip Hop', 'Andrea Castillo', 'Friday - 4:00 PM', 20),
('Contemporary Dance', 'Luis Fernandez', 'Saturday - 10:00 AM', 10);

INSERT INTO Inscriptions (StudentId, DanceClassId, InscriptionDate)
VALUES
(1, 1, '2026-11-01'),
(2, 2, '2026-11-02'),
(3, 1, '2026-11-03'),
(4, 3, '2026-11-04'),
(1, 4, '2026-11-05');

SELECT * FROM Students;

SELECT * FROM DanceClasses;

SELECT * FROM Inscriptions;