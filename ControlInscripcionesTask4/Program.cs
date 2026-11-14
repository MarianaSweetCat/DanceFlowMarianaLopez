// Mariana Alejandra López Rodríguez 2026-0911
// DANCE CLASS MANAGEMENT SYSTEM TAREA 4

DanceFlowApp app = new DanceFlowApp();
app.TestConnection();
app.Run();

// STUDENT CLASS
class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }

    public Student(int id, string name, int age, string phone, string email)
    {
        Id = id;
        Name = name;
        Age = age;
        Phone = phone;
        Email = email;
    }
}

// DANCE CLASS
class DanceClass
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Instructor { get; set; }
    public string Schedule { get; set; }
    public int Capacity { get; set; }

    public DanceClass(int id, string name, string instructor, string schedule, int capacity)
    {
        Id = id;
        Name = name;
        Instructor = instructor;
        Schedule = schedule;
        Capacity = capacity;
    }
}

// INSCRIPTION CLASS
class Inscription
{
    public int Id { get; set; }
    public Student Student { get; set; }
    public DanceClass DanceClass { get; set; }
    public DateTime InscriptionDate { get; set; }

    public Inscription(int id, Student student, DanceClass danceClass, DateTime inscriptionDate)
    {
        Id = id;
        Student = student;
        DanceClass = danceClass;
        InscriptionDate = inscriptionDate;
    }
}

// MAIN APPLICATION CLASS
class DanceFlowApp
{
    private string connectionString =
        "Server=DESKTOP-PA2RE64;Database=DanceFlow;Integrated Security=True;TrustServerCertificate=True;";

    // DATABASE CONNECTION
    private Microsoft.Data.SqlClient.SqlConnection GetConnection()
    {
        return new Microsoft.Data.SqlClient.SqlConnection(connectionString);
    }

    public void TestConnection()
    {
        try
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                Console.WriteLine("Database connection successful!");
            }
        }
        catch (Exception error)
        {
            Console.WriteLine("Database connection failed: " + error.Message);
            Pause();
        }
    }

    private string ReadText(string message)
    {
        string value;

        do
        {
            Console.Write(message);
            value = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(value))
            {
                Console.WriteLine("This field cannot be empty.");
            }

        } while (string.IsNullOrWhiteSpace(value));

        return value.Trim();
    }
    private int ReadNumber(string message, int minimum, int maximum)
    {
        int number;
        bool valid;

        do
        {
            Console.Write(message);
            valid = int.TryParse(Console.ReadLine(), out number);

            if (!valid || number < minimum || number > maximum)
            {
                Console.WriteLine($"Enter a valid number between {minimum} and {maximum}.");
            }

        } while (!valid || number < minimum || number > maximum);

        return number;
    }

    private string ReadPhone()
    {
        string phone;
        string digits;

        do
        {
            Console.Write("Enter phone number: ");
            phone = Console.ReadLine() ?? "";

            digits = phone.Replace(" ", "")
                          .Replace("-", "")
                          .Replace("(", "")
                          .Replace(")", "")
                          .Replace("+", "");

            if (digits.Length != 10 || !digits.All(char.IsDigit))
            {
                Console.WriteLine("The phone number must contain exactly 10 digits.");
            }

        } while (digits.Length != 10 || !digits.All(char.IsDigit));

        return digits;
    }

    private string ReadEmail(int currentStudentId = 0)
    {
        string email;
        bool valid;

        do
        {
            Console.Write("Enter email: ");
            email = (Console.ReadLine() ?? "").Trim();

            valid = !string.IsNullOrWhiteSpace(email)
                    && email.Contains("@")
                    && email.LastIndexOf('.') > email.IndexOf('@')
                    && email.LastIndexOf('.') < email.Length - 1
                    && !email.Contains(" ");

            if (!valid)
            {
                Console.WriteLine("Enter a valid email address.");
                continue;
            }

            if (EmailExists(email, currentStudentId))
            {
                Console.WriteLine("This email is already registered.");
                valid = false;
            }

        } while (!valid);

        return email;
    }

    private bool EmailExists(string email, int currentStudentId = 0)
    {
        using (var connection = GetConnection())
        {
            connection.Open();

            string query = @"SELECT COUNT(*)
                             FROM Students
                             WHERE LOWER(Email) = LOWER(@Email)
                             AND Id <> @Id";

            using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Id", currentStudentId);

                return (int)command.ExecuteScalar() > 0;
            }
        }
    }

    private DateTime ReadDate()
    {
        string date;
        DateTime parsedDate;
        bool valid;

        do
        {
            Console.Write("Enter inscription date (DD/MM/YYYY): ");
            date = Console.ReadLine() ?? "";

            valid = DateTime.TryParseExact(
                date,
                new[] { "d/M/yyyy", "dd/MM/yyyy", "yyyy-MM-dd" },
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out parsedDate
            );

            if (!valid)
            {
                Console.WriteLine("Enter a valid date.");
            }

        } while (!valid);

        return parsedDate;
    }

    private void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    // MAIN MENU
    public void Run()
    {
        bool running = true;

        while (running)
        {
            try
            {
                Console.Clear();

                Console.WriteLine("----------------------------------");
                Console.WriteLine("       DANCEFLOW SYSTEM");
                Console.WriteLine("----------------------------------");
                Console.WriteLine("1. Students");
                Console.WriteLine("2. Dance Classes");
                Console.WriteLine("3. Inscriptions");
                Console.WriteLine("4. Exit");
                Console.WriteLine("----------------------------------");

                int option = ReadNumber("Choose an option: ", 1, 4);

                switch (option)
                {
                    case 1:
                        StudentsMenu();
                        break;

                    case 2:
                        ClassesMenu();
                        break;

                    case 3:
                        InscriptionsMenu();
                        break;

                    case 4:
                        running = false;
                        Console.WriteLine("\nThank you for using DanceFlow!");
                        break;
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("\nAn error occurred: " + error.Message);
                Pause();
            }
        }
    }

    // STUDENTS MENU

    private void StudentsMenu()
    {
        bool back = false;

        while (!back)
        {
            Console.Clear();

            Console.WriteLine("---------- STUDENTS ----------");
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. Search Student by ID");
            Console.WriteLine("3. Search Student by Name");
            Console.WriteLine("4. Modify Student");
            Console.WriteLine("5. Delete Student");
            Console.WriteLine("6. List Students");
            Console.WriteLine("7. Return to Main Menu");

            int option = ReadNumber("Choose an option: ", 1, 7);

            switch (option)
            {
                case 1:
                    AddStudent();
                    break;

                case 2:
                    SearchStudentById();
                    break;

                case 3:
                    SearchStudentByName();
                    break;

                case 4:
                    ModifyStudent();
                    break;

                case 5:
                    DeleteStudent();
                    break;

                case 6:
                    ListStudents();
                    break;

                case 7:
                    back = true;
                    break;
            }

            if (!back)
            {
                Pause();
            }
        }
    }

    // STUDENT CRUD METHODS

    private void AddStudent()
    {
        Console.WriteLine("\n---------- ADD STUDENT ----------");

        string name = ReadText("Enter student name: ");
        int age = ReadNumber("Enter age: ", 3, 100);
        string phone = ReadPhone();
        string email = ReadEmail();

        using (var connection = GetConnection())
        {
            connection.Open();

            string query = @"INSERT INTO Students (Name, Age, Phone, Email)
                             OUTPUT INSERTED.Id
                             VALUES (@Name, @Age, @Phone, @Email)";

            using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Age", age);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@Email", email);

                int newId = (int)command.ExecuteScalar();

                Console.WriteLine("\nStudent added successfully!");
                Console.WriteLine("Student ID: " + newId);
            }
        }
    }

    private Student FindStudentById(int id)
    {
        using (var connection = GetConnection())
        {
            connection.Open();

            string query = @"SELECT Id, Name, Age, Phone, Email
                             FROM Students
                             WHERE Id = @Id";

            using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Student(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetInt32(2),
                            reader.GetString(3),
                            reader.GetString(4)
                        );
                    }
                }
            }
        }

        return null;
    }

    private void SearchStudentById()
    {
        Console.WriteLine("\n---------- SEARCH STUDENT ----------");

        int id = ReadNumber("Enter student ID: ", 1, int.MaxValue);
        Student student = FindStudentById(id);

        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        DisplayStudent(student);
    }

    private void SearchStudentByName()
    {
        Console.WriteLine("\n---------- SEARCH STUDENT BY NAME ----------");

        string name = ReadText("Enter student name: ");

        using (var connection = GetConnection())
        {
            connection.Open();

            string query = @"SELECT Id, Name, Age, Phone, Email
                             FROM Students
                             WHERE Name LIKE @Name";

            using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", "%" + name + "%");

                using (var reader = command.ExecuteReader())
                {
                    bool found = false;

                    while (reader.Read())
                    {
                        found = true;

                        Student student = new Student(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetInt32(2),
                            reader.GetString(3),
                            reader.GetString(4)
                        );

                        DisplayStudent(student);
                    }

                    if (!found)
                    {
                        Console.WriteLine("No students found.");
                    }
                }
            }
        }
    }

    private void ModifyStudent()
    {
        Console.WriteLine("\n---------- MODIFY STUDENT ----------");

        ListStudents();

        int id = ReadNumber("Enter the student ID to modify: ", 1, int.MaxValue);
        Student student = FindStudentById(id);

        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        Console.WriteLine("\nCurrent information:");
        DisplayStudent(student);

        Console.WriteLine("\nEnter the new information:");

        string name = ReadText("Enter new name: ");
        int age = ReadNumber("Enter new age: ", 3, 100);
        string phone = ReadPhone();
        string email = ReadEmail(student.Id);

        using (var connection = GetConnection())
        {
            connection.Open();

            string query = @"UPDATE Students
                             SET Name = @Name,
                                 Age = @Age,
                                 Phone = @Phone,
                                 Email = @Email
                             WHERE Id = @Id";

            using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Age", age);
                command.Parameters.AddWithValue("@Phone", phone);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }

        Console.WriteLine("\nStudent modified successfully!");
    }

    private void DeleteStudent()
    {
        Console.WriteLine("\n---------- DELETE STUDENT ----------");

        ListStudents();

        int id = ReadNumber("Enter the student ID to delete: ", 1, int.MaxValue);
        Student student = FindStudentById(id);

        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        using (var connection = GetConnection())
        {
            connection.Open();

            string checkQuery = "SELECT COUNT(*) FROM Inscriptions WHERE StudentId = @Id";

            using (var command = new Microsoft.Data.SqlClient.SqlCommand(checkQuery, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                if ((int)command.ExecuteScalar() > 0)
                {
                    Console.WriteLine("This student has active inscriptions and cannot be deleted.");
                    return;
                }
            }
        }

        Console.WriteLine($"Are you sure you want to delete {student.Name}?");
        Console.WriteLine("1. Yes");
        Console.WriteLine("2. No");

        int confirmation = ReadNumber("Choose an option: ", 1, 2);

        if (confirmation == 1)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM Students WHERE Id = @Id";

                using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Student deleted successfully.");
        }
        else
        {
            Console.WriteLine("Operation cancelled.");
        }
    }

    private void ListStudents()
    {
        Console.WriteLine("\n---------- STUDENT LIST ----------");

        using (var connection = GetConnection())
        {
            connection.Open();

            string query = "SELECT Id, Name, Age, Phone, Email FROM Students ORDER BY Id";

            using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                bool found = false;

                while (reader.Read())
                {
                    found = true;

                    Student student = new Student(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetInt32(2),
                        reader.GetString(3),
                        reader.GetString(4)
                    );

                    DisplayStudent(student);
                }

                if (!found)
                {
                    Console.WriteLine("There are no students registered.");
                }
            }
        }
    }

    private void DisplayStudent(Student student)
    {
        Console.WriteLine("----------------------------------");
        Console.WriteLine("ID: " + student.Id);
        Console.WriteLine("Name: " + student.Name);
        Console.WriteLine("Age: " + student.Age);
        Console.WriteLine("Phone: " + student.Phone);
        Console.WriteLine("Email: " + student.Email);
        Console.WriteLine("----------------------------------");
    }

    // DANCE CLASSES MENU

    private void ClassesMenu()
    {
        bool back = false;

        while (!back)
        {
            Console.Clear();

            Console.WriteLine("---------- DANCE CLASSES ----------");
            Console.WriteLine("1. Add Dance Class");
            Console.WriteLine("2. Search Class by ID");
            Console.WriteLine("3. Search Class by Name");
            Console.WriteLine("4. Search Class by Instructor");
            Console.WriteLine("5. Modify Dance Class");
            Console.WriteLine("6. Delete Dance Class");
            Console.WriteLine("7. List Dance Classes");
            Console.WriteLine("8. Return to Main Menu");

            int option = ReadNumber("Choose an option: ", 1, 8);

            switch (option)
            {
                case 1:
                    AddClass();
                    break;

                case 2:
                    SearchClassById();
                    break;

                case 3:
                    SearchClassByName();
                    break;

                case 4:
                    SearchClassByInstructor();
                    break;

                case 5:
                    ModifyClass();
                    break;

                case 6:
                    DeleteClass();
                    break;

                case 7:
                    ListClasses();
                    break;

                case 8:
                    back = true;
                    break;
            }

            if (!back)
            {
                Pause();
            }
        }
    }

    // DANCE CLASS CRUD METHODS

    private void AddClass()
    {
        Console.WriteLine("\n---------- ADD DANCE CLASS ----------");

        string name = ReadText("Enter class name: ");
        string instructor = ReadText("Enter instructor name: ");
        string schedule = ReadText("Enter class schedule: ");
        int capacity = ReadNumber("Enter class capacity: ", 1, int.MaxValue);

        using (var connection = GetConnection())
        {
            connection.Open();

            string query = @"INSERT INTO DanceClasses (Name, Instructor, Schedule, Capacity)
                             OUTPUT INSERTED.Id
                             VALUES (@Name, @Instructor, @Schedule, @Capacity)";

            using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Instructor", instructor);
                command.Parameters.AddWithValue("@Schedule", schedule);
                command.Parameters.AddWithValue("@Capacity", capacity);

                int newId = (int)command.ExecuteScalar();

                Console.WriteLine("\nDance class added successfully!");
                Console.WriteLine("Class ID: " + newId);
            }
        }
    }

    private DanceClass FindClassById(int id)
    {
        using (var connection = GetConnection())
        {
            connection.Open();

            string query = @"SELECT Id, Name, Instructor, Schedule, Capacity
                             FROM DanceClasses
                             WHERE Id = @Id";

            using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new DanceClass(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetInt32(4)
                        );
                    }
                }
            }
        }

        return null;
    }

    private void SearchClassById()
    {
        Console.WriteLine("\n---------- SEARCH CLASS ----------");

        int id = ReadNumber("Enter class ID: ", 1, int.MaxValue);
        DanceClass danceClass = FindClassById(id);

        if (danceClass == null)
        {
            Console.WriteLine("Dance class not found.");
            return;
        }

        DisplayClass(danceClass);
    }

    private void SearchClassByName()
    {
        Console.WriteLine("\n---------- SEARCH CLASS BY NAME ----------");

        string name = ReadText("Enter class name: ");

        SearchClasses("Name", name);
    }

    private void SearchClassByInstructor()
    {
        Console.WriteLine("\n---------- SEARCH CLASS BY INSTRUCTOR ----------");

        string instructor = ReadText("Enter instructor name: ");

        SearchClasses("Instructor", instructor);
    }

    private void SearchClasses(string field, string value)
    {
        string query = field == "Name"
            ? "SELECT Id, Name, Instructor, Schedule, Capacity FROM DanceClasses WHERE Name LIKE @Value"
            : "SELECT Id, Name, Instructor, Schedule, Capacity FROM DanceClasses WHERE Instructor LIKE @Value";

        using (var connection = GetConnection())
        {
            connection.Open();

            using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Value", "%" + value + "%");

                using (var reader = command.ExecuteReader())
                {
                    bool found = false;

                    while (reader.Read())
                    {
                        found = true;

                        DanceClass danceClass = new DanceClass(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetInt32(4)
                        );

                        DisplayClass(danceClass);
                    }

                    if (!found)
                    {
                        Console.WriteLine("No dance classes found.");
                    }
                }
            }
        }
    }

    private int CountClassInscriptions(int classId)
    {
        using (var connection = GetConnection())
        {
            connection.Open();

            string query = "SELECT COUNT(*) FROM Inscriptions WHERE DanceClassId = @Id";

            using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", classId);

                return (int)command.ExecuteScalar();
            }
        }
    }

    private void ModifyClass()
    {
        Console.WriteLine("\n---------- MODIFY DANCE CLASS ----------");

        ListClasses();

        int id = ReadNumber("Enter the class ID to modify: ", 1, int.MaxValue);
        DanceClass danceClass = FindClassById(id);

        if (danceClass == null)
        {
            Console.WriteLine("Dance class not found.");
            return;
        }

        Console.WriteLine("\nCurrent information:");
        DisplayClass(danceClass);

        Console.WriteLine("\nEnter the new information:");

        string name = ReadText("Enter new class name: ");
        string instructor = ReadText("Enter new instructor: ");
        string schedule = ReadText("Enter new schedule: ");

        int currentInscriptions = CountClassInscriptions(id);

        int capacity = ReadNumber(
            $"Enter new capacity (minimum {currentInscriptions}): ",
            Math.Max(1, currentInscriptions),
            int.MaxValue
        );

        using (var connection = GetConnection())
        {
            connection.Open();

            string query = @"UPDATE DanceClasses
                             SET Name = @Name,
                                 Instructor = @Instructor,
                                 Schedule = @Schedule,
                                 Capacity = @Capacity
                             WHERE Id = @Id";

            using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Instructor", instructor);
                command.Parameters.AddWithValue("@Schedule", schedule);
                command.Parameters.AddWithValue("@Capacity", capacity);
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }

        Console.WriteLine("\nDance class modified successfully!");
    }

    private void DeleteClass()
    {
        Console.WriteLine("\n---------- DELETE DANCE CLASS ----------");

        ListClasses();

        int id = ReadNumber("Enter the class ID to delete: ", 1, int.MaxValue);
        DanceClass danceClass = FindClassById(id);

        if (danceClass == null)
        {
            Console.WriteLine("Dance class not found.");
            return;
        }

        if (CountClassInscriptions(id) > 0)
        {
            Console.WriteLine("This class has active inscriptions and cannot be deleted.");
            return;
        }

        Console.WriteLine($"Are you sure you want to delete {danceClass.Name}?");
        Console.WriteLine("1. Yes");
        Console.WriteLine("2. No");

        int confirmation = ReadNumber("Choose an option: ", 1, 2);

        if (confirmation == 1)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM DanceClasses WHERE Id = @Id";

                using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Dance class deleted successfully.");
        }
        else
        {
            Console.WriteLine("Operation cancelled.");
        }
    }

    private void ListClasses()
    {
        Console.WriteLine("\n---------- DANCE CLASS LIST ----------");

        using (var connection = GetConnection())
        {
            connection.Open();

            string query = "SELECT Id, Name, Instructor, Schedule, Capacity FROM DanceClasses ORDER BY Id";

            using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                bool found = false;

                while (reader.Read())
                {
                    found = true;

                    DanceClass danceClass = new DanceClass(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetInt32(4)
                    );

                    DisplayClass(danceClass);
                }

                if (!found)
                {
                    Console.WriteLine("There are no dance classes registered.");
                }
            }
        }
    }

    private void DisplayClass(DanceClass danceClass)
    {
        int currentInscriptions = CountClassInscriptions(danceClass.Id);

        Console.WriteLine("----------------------------------");
        Console.WriteLine("ID: " + danceClass.Id);
        Console.WriteLine("Name: " + danceClass.Name);
        Console.WriteLine("Instructor: " + danceClass.Instructor);
        Console.WriteLine("Schedule: " + danceClass.Schedule);
        Console.WriteLine("Capacity: " + danceClass.Capacity);
        Console.WriteLine("Registered students: " + currentInscriptions);
        Console.WriteLine("----------------------------------");
    }

    // INSCRIPTIONS MENU

    private void InscriptionsMenu()
    {
        bool back = false;

        while (!back)
        {
            Console.Clear();

            Console.WriteLine("---------- INSCRIPTIONS ----------");
            Console.WriteLine("1. Add Inscription");
            Console.WriteLine("2. Search Inscription by ID");
            Console.WriteLine("3. Search Inscriptions by Student");
            Console.WriteLine("4. Search Inscriptions by Class");
            Console.WriteLine("5. Modify Inscription");
            Console.WriteLine("6. Delete Inscription");
            Console.WriteLine("7. List Inscriptions");
            Console.WriteLine("8. Return to Main Menu");

            int option = ReadNumber("Choose an option: ", 1, 8);

            switch (option)
            {
                case 1:
                    AddInscription();
                    break;

                case 2:
                    SearchInscriptionById();
                    break;

                case 3:
                    SearchInscriptionsByStudent();
                    break;

                case 4:
                    SearchInscriptionsByClass();
                    break;

                case 5:
                    ModifyInscription();
                    break;

                case 6:
                    DeleteInscription();
                    break;

                case 7:
                    ListInscriptions();
                    break;

                case 8:
                    back = true;
                    break;
            }

            if (!back)
            {
                Pause();
            }
        }
    }

    private Inscription FindInscriptionById(int id)
    {
        using (var connection = GetConnection())
        {
            connection.Open();

            string query = @"SELECT I.Id, I.StudentId, I.DanceClassId, I.InscriptionDate,
                                    S.Name, S.Age, S.Phone, S.Email,
                                    D.Name, D.Instructor, D.Schedule, D.Capacity
                             FROM Inscriptions I
                             INNER JOIN Students S ON I.StudentId = S.Id
                             INNER JOIN DanceClasses D ON I.DanceClassId = D.Id
                             WHERE I.Id = @Id";

            using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Student student = new Student(
                            reader.GetInt32(1),
                            reader.GetString(4),
                            reader.GetInt32(5),
                            reader.GetString(6),
                            reader.GetString(7)
                        );

                        DanceClass danceClass = new DanceClass(
                            reader.GetInt32(2),
                            reader.GetString(8),
                            reader.GetString(9),
                            reader.GetString(10),
                            reader.GetInt32(11)
                        );

                        return new Inscription(
                            reader.GetInt32(0),
                            student,
                            danceClass,
                            reader.GetDateTime(3)
                        );
                    }
                }
            }
        }

        return null;
    }
    private bool InscriptionExists(int studentId, int classId, int currentId = 0)
    {
        using (var connection = GetConnection())
        {
            connection.Open();

            string query = @"SELECT COUNT(*)
                             FROM Inscriptions
                             WHERE StudentId = @StudentId
                             AND DanceClassId = @ClassId
                             AND Id <> @Id";

            using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StudentId", studentId);
                command.Parameters.AddWithValue("@ClassId", classId);
                command.Parameters.AddWithValue("@Id", currentId);

                return (int)command.ExecuteScalar() > 0;
            }
        }
    }

    private void AddInscription()
    {
        Console.WriteLine("\n---------- ADD INSCRIPTION ----------");

        ListStudents();

        int studentId = ReadNumber("Enter student ID: ", 1, int.MaxValue);
        Student student = FindStudentById(studentId);

        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        ListClasses();

        int classId = ReadNumber("Enter dance class ID: ", 1, int.MaxValue);
        DanceClass danceClass = FindClassById(classId);

        if (danceClass == null)
        {
            Console.WriteLine("Dance class not found.");
            return;
        }

        if (InscriptionExists(studentId, classId))
        {
            Console.WriteLine("This student is already registered in this class.");
            return;
        }

        if (CountClassInscriptions(classId) >= danceClass.Capacity)
        {
            Console.WriteLine("This dance class has reached its maximum capacity.");
            return;
        }

        DateTime date = ReadDate();

        using (var connection = GetConnection())
        {
            connection.Open();

            string query = @"INSERT INTO Inscriptions (StudentId, DanceClassId, InscriptionDate)
                             OUTPUT INSERTED.Id
                             VALUES (@StudentId, @ClassId, @Date)";

            using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StudentId", studentId);
                command.Parameters.AddWithValue("@ClassId", classId);
                command.Parameters.Add("@Date", System.Data.SqlDbType.Date).Value = date.Date;

                int newId = (int)command.ExecuteScalar();

                Console.WriteLine("\nInscription added successfully!");
                Console.WriteLine("Inscription ID: " + newId);
            }
        }
    }

    private void SearchInscriptionById()
    {
        Console.WriteLine("\n---------- SEARCH INSCRIPTION ----------");

        int id = ReadNumber("Enter inscription ID: ", 1, int.MaxValue);
        Inscription inscription = FindInscriptionById(id);

        if (inscription == null)
        {
            Console.WriteLine("Inscription not found.");
            return;
        }

        DisplayInscription(inscription);
    }
    private void SearchInscriptionsByStudent()
    {
        Console.WriteLine("\n---------- SEARCH INSCRIPTIONS BY STUDENT ----------");

        ListStudents();

        int studentId = ReadNumber("Enter student ID: ", 1, int.MaxValue);
        Student student = FindStudentById(studentId);

        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        ListInscriptionsByCriteria("StudentId", studentId);
    }
    private void SearchInscriptionsByClass()
    {
        Console.WriteLine("\n---------- SEARCH INSCRIPTIONS BY CLASS ----------");

        ListClasses();

        int classId = ReadNumber("Enter class ID: ", 1, int.MaxValue);
        DanceClass danceClass = FindClassById(classId);

        if (danceClass == null)
        {
            Console.WriteLine("Dance class not found.");
            return;
        }

        ListInscriptionsByCriteria("DanceClassId", classId);
    }
    private void ListInscriptionsByCriteria(string field, int value)
    {
        string query = field == "StudentId"
            ? @"SELECT I.Id, I.StudentId, I.DanceClassId, I.InscriptionDate,
                       S.Name, S.Age, S.Phone, S.Email,
                       D.Name, D.Instructor, D.Schedule, D.Capacity
                FROM Inscriptions I
                INNER JOIN Students S ON I.StudentId = S.Id
                INNER JOIN DanceClasses D ON I.DanceClassId = D.Id
                WHERE I.StudentId = @Value"
            : @"SELECT I.Id, I.StudentId, I.DanceClassId, I.InscriptionDate,
                       S.Name, S.Age, S.Phone, S.Email,
                       D.Name, D.Instructor, D.Schedule, D.Capacity
                FROM Inscriptions I
                INNER JOIN Students S ON I.StudentId = S.Id
                INNER JOIN DanceClasses D ON I.DanceClassId = D.Id
                WHERE I.DanceClassId = @Value";

        using (var connection = GetConnection())
        {
            connection.Open();

            using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Value", value);

                using (var reader = command.ExecuteReader())
                {
                    bool found = false;

                    while (reader.Read())
                    {
                        found = true;
                        DisplayInscriptionFromReader(reader);
                    }

                    if (!found)
                    {
                        Console.WriteLine("No inscriptions found.");
                    }
                }
            }
        }
    }

    private void ModifyInscription()
    {
        Console.WriteLine("\n---------- MODIFY INSCRIPTION ----------");

        ListInscriptions();

        int id = ReadNumber("Enter the inscription ID to modify: ", 1, int.MaxValue);
        Inscription inscription = FindInscriptionById(id);

        if (inscription == null)
        {
            Console.WriteLine("Inscription not found.");
            return;
        }

        Console.WriteLine("\nCurrent information:");
        DisplayInscription(inscription);

        Console.WriteLine("\nSelect the new student:");

        ListStudents();

        int studentId = ReadNumber("Enter new student ID: ", 1, int.MaxValue);
        Student newStudent = FindStudentById(studentId);

        if (newStudent == null)
        {
            Console.WriteLine("Student not found. Operation cancelled.");
            return;
        }

        Console.WriteLine("\nSelect the new dance class:");

        ListClasses();

        int classId = ReadNumber("Enter new class ID: ", 1, int.MaxValue);
        DanceClass newClass = FindClassById(classId);

        if (newClass == null)
        {
            Console.WriteLine("Dance class not found. Operation cancelled.");
            return;
        }

        if (InscriptionExists(studentId, classId, id))
        {
            Console.WriteLine("This student is already registered in this class.");
            return;
        }

        int currentInscriptions = CountClassInscriptions(classId);

        if (inscription.DanceClass.Id == classId)
        {
            currentInscriptions--;
        }

        if (currentInscriptions >= newClass.Capacity)
        {
            Console.WriteLine("This dance class has reached its maximum capacity.");
            return;
        }

        DateTime date = ReadDate();

        using (var connection = GetConnection())
        {
            connection.Open();

            string query = @"UPDATE Inscriptions
                             SET StudentId = @StudentId,
                                 DanceClassId = @ClassId,
                                 InscriptionDate = @Date
                             WHERE Id = @Id";

            using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StudentId", studentId);
                command.Parameters.AddWithValue("@ClassId", classId);
                command.Parameters.Add("@Date", System.Data.SqlDbType.Date).Value = date.Date;
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }

        Console.WriteLine("\nInscription modified successfully!");
    }

    private void DeleteInscription()
    {
        Console.WriteLine("\n---------- DELETE INSCRIPTION ----------");

        ListInscriptions();

        int id = ReadNumber("Enter the inscription ID to delete: ", 1, int.MaxValue);
        Inscription inscription = FindInscriptionById(id);

        if (inscription == null)
        {
            Console.WriteLine("Inscription not found.");
            return;
        }

        Console.WriteLine("Are you sure you want to delete this inscription?");
        Console.WriteLine("1. Yes");
        Console.WriteLine("2. No");

        int confirmation = ReadNumber("Choose an option: ", 1, 2);

        if (confirmation == 1)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                string query = "DELETE FROM Inscriptions WHERE Id = @Id";

                using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Inscription deleted successfully.");
        }
        else
        {
            Console.WriteLine("Operation cancelled.");
        }
    }
    private void ListInscriptions()
    {
        Console.WriteLine("\n---------- INSCRIPTION LIST ----------");

        using (var connection = GetConnection())
        {
            connection.Open();

            string query = @"SELECT I.Id, I.StudentId, I.DanceClassId, I.InscriptionDate,
                                    S.Name, S.Age, S.Phone, S.Email,
                                    D.Name, D.Instructor, D.Schedule, D.Capacity
                             FROM Inscriptions I
                             INNER JOIN Students S ON I.StudentId = S.Id
                             INNER JOIN DanceClasses D ON I.DanceClassId = D.Id
                             ORDER BY I.Id";

            using (var command = new Microsoft.Data.SqlClient.SqlCommand(query, connection))
            using (var reader = command.ExecuteReader())
            {
                bool found = false;

                while (reader.Read())
                {
                    found = true;
                    DisplayInscriptionFromReader(reader);
                }

                if (!found)
                {
                    Console.WriteLine("There are no inscriptions registered.");
                }
            }
        }
    }

    private void DisplayInscriptionFromReader(Microsoft.Data.SqlClient.SqlDataReader reader)
    {
        Console.WriteLine("----------------------------------");
        Console.WriteLine("Inscription ID: " + reader.GetInt32(0));
        Console.WriteLine("Student: " + reader.GetString(4));
        Console.WriteLine("Student ID: " + reader.GetInt32(1));
        Console.WriteLine("Dance Class: " + reader.GetString(8));
        Console.WriteLine("Class ID: " + reader.GetInt32(2));
        Console.WriteLine("Inscription Date: " + reader.GetDateTime(3).ToString("dd/MM/yyyy"));
        Console.WriteLine("----------------------------------");
    }

    private void DisplayInscription(Inscription inscription)
    {
        Console.WriteLine("----------------------------------");
        Console.WriteLine("Inscription ID: " + inscription.Id);
        Console.WriteLine("Student: " + inscription.Student.Name);
        Console.WriteLine("Student ID: " + inscription.Student.Id);
        Console.WriteLine("Dance Class: " + inscription.DanceClass.Name);
        Console.WriteLine("Class ID: " + inscription.DanceClass.Id);
        Console.WriteLine("Inscription Date: " + inscription.InscriptionDate.ToString("dd/MM/yyyy"));
        Console.WriteLine("----------------------------------");
    }
}
