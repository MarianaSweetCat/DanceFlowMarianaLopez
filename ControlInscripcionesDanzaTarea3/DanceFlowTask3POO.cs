// Mariana Alejandra López Rodríguez 2026-0911
// DANCE CLASS MANAGEMENT SYSTEM TAREA 3

DanceFlowApp app = new DanceFlowApp();
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
    public string InscriptionDate { get; set; }
    public Inscription(int id, Student student, DanceClass danceClass, string inscriptionDate)
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
    private List<Student> students = new List<Student>();
    private List<DanceClass> classes = new List<DanceClass>();
    private List<Inscription> inscriptions = new List<Inscription>();
    private int nextStudentId = 1;
    private int nextClassId = 1;
    private int nextInscriptionId = 1;

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


    // INPUT VALIDATION METHODS
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
    private string ReadEmail(Student currentStudent = null)
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

            bool exists = students.Any(student =>
                student.Email.Equals(email, StringComparison.OrdinalIgnoreCase)
                && (currentStudent == null || student.Id != currentStudent.Id));

            if (exists)
            {
                Console.WriteLine("This email is already registered.");
                valid = false;
            }

        } while (!valid);

        return email;
    }
    private string ReadDate()
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

        return parsedDate.ToString("dd/MM/yyyy");
    }
    private void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
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
        int age = ReadNumber("Enter age: ", 1, 100);
        string phone = ReadPhone();
        string email = ReadEmail();

        Student newStudent = new Student(
            nextStudentId,
            name,
            age,
            phone,
            email
        );

        students.Add(newStudent);
        nextStudentId++;

        Console.WriteLine("\nStudent added successfully!");
        Console.WriteLine("Student ID: " + newStudent.Id);
    }
    private Student FindStudentById(int id)
    {
        return students.FirstOrDefault(student => student.Id == id);
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

        List<Student> results = students
            .Where(student => student.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("No students found.");
            return;
        }

        foreach (Student student in results)
        {
            DisplayStudent(student);
        }
    }
    private void ModifyStudent()
    {
        Console.WriteLine("\n---------- MODIFY STUDENT ----------");

        if (students.Count == 0)
        {
            Console.WriteLine("There are no students registered.");
            return;
        }

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

        student.Name = ReadText("Enter new name: ");
        student.Age = ReadNumber("Enter new age: ", 1, 100);
        student.Phone = ReadPhone();
        student.Email = ReadEmail(student);

        Console.WriteLine("\nStudent modified successfully!");
    }
    private void DeleteStudent()
    {
        Console.WriteLine("\n---------- DELETE STUDENT ----------");

        if (students.Count == 0)
        {
            Console.WriteLine("There are no students registered.");
            return;
        }

        ListStudents();

        int id = ReadNumber("Enter the student ID to delete: ", 1, int.MaxValue);
        Student student = FindStudentById(id);

        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        bool hasInscriptions = inscriptions.Any(
            inscription => inscription.Student.Id == id
        );

        if (hasInscriptions)
        {
            Console.WriteLine("This student has active inscriptions and cannot be deleted.");
            return;
        }

        Console.WriteLine($"Are you sure you want to delete {student.Name}?");
        Console.WriteLine("1. Yes");
        Console.WriteLine("2. No");

        int confirmation = ReadNumber("Choose an option: ", 1, 2);

        if (confirmation == 1)
        {
            students.Remove(student);
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

        if (students.Count == 0)
        {
            Console.WriteLine("There are no students registered.");
            return;
        }

        foreach (Student student in students)
        {
            DisplayStudent(student);
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

        DanceClass newClass = new DanceClass(
            nextClassId,
            name,
            instructor,
            schedule,
            capacity
        );

        classes.Add(newClass);
        nextClassId++;

        Console.WriteLine("\nDance class added successfully!");
        Console.WriteLine("Class ID: " + newClass.Id);
    }
    private DanceClass FindClassById(int id)
    {
        return classes.FirstOrDefault(danceClass => danceClass.Id == id);
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

        List<DanceClass> results = classes
            .Where(danceClass => danceClass.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("No dance classes found.");
            return;
        }

        foreach (DanceClass danceClass in results)
        {
            DisplayClass(danceClass);
        }
    }
    private void SearchClassByInstructor()
    {
        Console.WriteLine("\n---------- SEARCH CLASS BY INSTRUCTOR ----------");

        string instructor = ReadText("Enter instructor name: ");

        List<DanceClass> results = classes
            .Where(danceClass => danceClass.Instructor.Contains(instructor, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("No classes found for this instructor.");
            return;
        }

        foreach (DanceClass danceClass in results)
        {
            DisplayClass(danceClass);
        }
    }
    private void ModifyClass()
    {
        Console.WriteLine("\n---------- MODIFY DANCE CLASS ----------");

        if (classes.Count == 0)
        {
            Console.WriteLine("There are no dance classes registered.");
            return;
        }

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

        danceClass.Name = ReadText("Enter new class name: ");
        danceClass.Instructor = ReadText("Enter new instructor: ");
        danceClass.Schedule = ReadText("Enter new schedule: ");

        int currentInscriptions = inscriptions.Count(
            inscription => inscription.DanceClass.Id == danceClass.Id
        );

        int newCapacity = ReadNumber(
            $"Enter new capacity (minimum {currentInscriptions}): ",
            Math.Max(1, currentInscriptions),
            int.MaxValue
        );

        danceClass.Capacity = newCapacity;

        Console.WriteLine("\nDance class modified successfully!");
    }
    private void DeleteClass()
    {
        Console.WriteLine("\n---------- DELETE DANCE CLASS ----------");

        if (classes.Count == 0)
        {
            Console.WriteLine("There are no dance classes registered.");
            return;
        }

        ListClasses();

        int id = ReadNumber("Enter the class ID to delete: ", 1, int.MaxValue);
        DanceClass danceClass = FindClassById(id);

        if (danceClass == null)
        {
            Console.WriteLine("Dance class not found.");
            return;
        }

        bool hasInscriptions = inscriptions.Any(
            inscription => inscription.DanceClass.Id == id
        );

        if (hasInscriptions)
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
            classes.Remove(danceClass);
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

        if (classes.Count == 0)
        {
            Console.WriteLine("There are no dance classes registered.");
            return;
        }

        foreach (DanceClass danceClass in classes)
        {
            DisplayClass(danceClass);
        }
    }
    private void DisplayClass(DanceClass danceClass)
    {
        int currentInscriptions = inscriptions.Count(
            inscription => inscription.DanceClass.Id == danceClass.Id
        );

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
                    SearchInscriptionByStudent();
                    break;

                case 4:
                    SearchInscriptionByClass();
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

    // INSCRIPTION CRUD METHODS
    private void AddInscription()
    {
        Console.WriteLine("\n---------- ADD INSCRIPTION ----------");

        if (students.Count == 0)
        {
            Console.WriteLine("Register a student before creating an inscription.");
            return;
        }

        if (classes.Count == 0)
        {
            Console.WriteLine("Register a dance class before creating an inscription.");
            return;
        }

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

        bool alreadyRegistered = inscriptions.Any(
            inscription => inscription.Student.Id == studentId
            && inscription.DanceClass.Id == classId
        );

        if (alreadyRegistered)
        {
            Console.WriteLine("This student is already registered in this class.");
            return;
        }

        int currentInscriptions = inscriptions.Count(
            inscription => inscription.DanceClass.Id == classId
        );

        if (currentInscriptions >= danceClass.Capacity)
        {
            Console.WriteLine("This dance class has reached its maximum capacity.");
            return;
        }

        string date = ReadDate();

        Inscription newInscription = new Inscription(
            nextInscriptionId,
            student,
            danceClass,
            date
        );

        inscriptions.Add(newInscription);
        nextInscriptionId++;

        Console.WriteLine("\nInscription added successfully!");
        Console.WriteLine("Inscription ID: " + newInscription.Id);
    }
    private Inscription FindInscriptionById(int id)
    {
        return inscriptions.FirstOrDefault(inscription => inscription.Id == id);
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
    private void SearchInscriptionByStudent()
    {
        Console.WriteLine("\n---------- SEARCH INSCRIPTIONS BY STUDENT ----------");

        if (students.Count == 0)
        {
            Console.WriteLine("There are no students registered.");
            return;
        }

        ListStudents();

        int studentId = ReadNumber("Enter student ID: ", 1, int.MaxValue);
        Student student = FindStudentById(studentId);

        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        List<Inscription> results = inscriptions
            .Where(inscription => inscription.Student.Id == studentId)
            .ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("This student has no inscriptions.");
            return;
        }

        foreach (Inscription inscription in results)
        {
            DisplayInscription(inscription);
        }
    }
    private void SearchInscriptionByClass()
    {
        Console.WriteLine("\n---------- SEARCH INSCRIPTIONS BY CLASS ----------");

        if (classes.Count == 0)
        {
            Console.WriteLine("There are no dance classes registered.");
            return;
        }

        ListClasses();

        int classId = ReadNumber("Enter class ID: ", 1, int.MaxValue);
        DanceClass danceClass = FindClassById(classId);

        if (danceClass == null)
        {
            Console.WriteLine("Dance class not found.");
            return;
        }

        List<Inscription> results = inscriptions
            .Where(inscription => inscription.DanceClass.Id == classId)
            .ToList();

        if (results.Count == 0)
        {
            Console.WriteLine("This class has no inscriptions.");
            return;
        }

        foreach (Inscription inscription in results)
        {
            DisplayInscription(inscription);
        }
    }
    private void ModifyInscription()
    {
        Console.WriteLine("\n---------- MODIFY INSCRIPTION ----------");

        if (inscriptions.Count == 0)
        {
            Console.WriteLine("There are no inscriptions registered.");
            return;
        }

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

        bool duplicate = inscriptions.Any(item =>
            item.Id != inscription.Id
            && item.Student.Id == studentId
            && item.DanceClass.Id == classId
        );

        if (duplicate)
        {
            Console.WriteLine("This student is already registered in this class.");
            return;
        }

        int currentInscriptions = inscriptions.Count(item =>
            item.DanceClass.Id == classId
            && item.Id != inscription.Id
        );

        if (currentInscriptions >= newClass.Capacity)
        {
            Console.WriteLine("This dance class has reached its maximum capacity.");
            return;
        }

        inscription.Student = newStudent;
        inscription.DanceClass = newClass;
        inscription.InscriptionDate = ReadDate();

        Console.WriteLine("\nInscription modified successfully!");
    }
    private void DeleteInscription()
    {
        Console.WriteLine("\n---------- DELETE INSCRIPTION ----------");

        if (inscriptions.Count == 0)
        {
            Console.WriteLine("There are no inscriptions registered.");
            return;
        }

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
            inscriptions.Remove(inscription);
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

        if (inscriptions.Count == 0)
        {
            Console.WriteLine("There are no inscriptions registered.");
            return;
        }

        foreach (Inscription inscription in inscriptions)
        {
            DisplayInscription(inscription);
        }
    }
    private void DisplayInscription(Inscription inscription)
    {
        Console.WriteLine("----------------------------------");
        Console.WriteLine("Inscription ID: " + inscription.Id);
        Console.WriteLine("Student: " + inscription.Student.Name);
        Console.WriteLine("Student ID: " + inscription.Student.Id);
        Console.WriteLine("Dance Class: " + inscription.DanceClass.Name);
        Console.WriteLine("Class ID: " + inscription.DanceClass.Id);
        Console.WriteLine("Inscription Date: " + inscription.InscriptionDate);
        Console.WriteLine("----------------------------------");
    }
}
