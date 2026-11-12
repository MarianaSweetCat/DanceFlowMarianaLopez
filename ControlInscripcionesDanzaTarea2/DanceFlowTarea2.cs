// Mariana Alejandra López Rodríguez 2026-0911
try
{
    // Student ID list and dictionaries
    List<int> studentIds = new List<int>();
    Dictionary<int, string> studentNames = new Dictionary<int, string>();
    Dictionary<int, int> studentAges = new Dictionary<int, int>();
    Dictionary<int, string> studentPhones = new Dictionary<int, string>();
    Dictionary<int, string> studentEmails = new Dictionary<int, string>();

    // Dance class ID list and dictionaries
    List<int> classIds = new List<int>();
    Dictionary<int, string> classNames = new Dictionary<int, string>();
    Dictionary<int, string> instructors = new Dictionary<int, string>();
    Dictionary<int, string> schedules = new Dictionary<int, string>();
    Dictionary<int, int> classCapacities = new Dictionary<int, int>();

    // Inscription ID and relationship lists, plus date dictionary
    List<int> inscriptionIds = new List<int>();
    List<int> inscribedStudentIds = new List<int>();
    List<int> inscribedClassIds = new List<int>();
    Dictionary<int, string> inscriptionDates = new Dictionary<int, string>();

    int nextStudentId = 1;
    int nextClassId = 1;
    int nextInscriptionId = 1;

    // Read text information
    string ReadText(string message)
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

    // Read and validate numbers
    int ReadNumber(string message)
    {
        int number;

        while (true)
        {
            Console.Write(message);
            string input = Console.ReadLine() ?? "";

            if (int.TryParse(input, out number))
            {
                return number;
            }

            Console.WriteLine("Invalid input. Enter a valid number.");
        }
    }

    // Read and validate a phone number
    string ReadPhone()
    {
        while (true)
        {
            Console.Write("Enter phone number (10 digits): ");
            string input = Console.ReadLine() ?? "";
            string digits = "";
            bool validCharacters = true;

            foreach (char character in input)
            {
                if (char.IsDigit(character))
                {
                    digits += character;
                }
                else if (!char.IsWhiteSpace(character) &&
                         character != '+' && character != '-' &&
                         character != '(' && character != ')')
                {
                    validCharacters = false;
                }
            }

            if (validCharacters && digits.Length == 10)
            {
                return digits;
            }

            Console.WriteLine("Invalid phone number. Enter exactly 10 digits.");
        }
    }

    // Read and validate email
    string ReadEmail()
    {
        while (true)
        {
            string email = ReadText("Enter email: ");
            int atPosition = email.IndexOf('@');
            int lastAtPosition = email.LastIndexOf('@');
            int dotPosition = email.LastIndexOf('.');

            if (atPosition > 0 &&
                atPosition == lastAtPosition &&
                dotPosition > atPosition + 1 &&
                dotPosition < email.Length - 1 &&
                !email.Contains(" "))
            {
                return email;
            }

            Console.WriteLine("Invalid email address. Try again.");
        }
    }

    // Check whether an email is already registered
    bool EmailExists(string email, int excludedStudentId = -1)
    {
        for (int i = 0; i < studentIds.Count; i++)
        {
            int currentStudentId = studentIds[i];

            if (currentStudentId != excludedStudentId &&
                studentEmails[currentStudentId].Equals(
                    email, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    // This part is used to read and validate an inscription date
    string ReadDate(string message)
    {
        DateTime date;

        while (true)
        {
            Console.Write(message);
            string input = Console.ReadLine() ?? "";

            string[] acceptedFormats = { "d/M/yyyy", "dd/MM/yyyy", "yyyy-MM-dd" };

            if (DateTime.TryParseExact(
                input,
                acceptedFormats,
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out date))
            {
                return date.ToString("dd/MM/yyyy");
            }

            Console.WriteLine("Invalid date. Use DD/MM/YYYY or YYYY-MM-DD, for example 11/11/2026.");
        }
    }

    // Pause the program
    void Pause()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    // STUDENTS MODULE

    void AddStudent()
    {
        Console.Clear();
        Console.WriteLine("----- ADD STUDENT -----");

        string name = ReadText("Enter full name: ");

        int age;

        do
        {
            age = ReadNumber("Enter age: ");

            if (age <= 0 || age > 100)
            {
                Console.WriteLine("Enter an age between 1 and 100.");
            }

        } while (age <= 0 || age > 100);

        string phone = ReadPhone();
        string email = ReadEmail();

        if (EmailExists(email))
        {
            Console.WriteLine("This email is already registered.");
            Pause();
            return;
        }

        studentIds.Add(nextStudentId);
        studentNames.Add(nextStudentId, name);
        studentAges.Add(nextStudentId, age);
        studentPhones.Add(nextStudentId, phone);
        studentEmails.Add(nextStudentId, email);

        Console.WriteLine("\nStudent registered successfully!");
        Console.WriteLine("Student ID: " + nextStudentId);

        nextStudentId++;

        Pause();
    }

    void SearchStudentById()
    {
        Console.Clear();
        Console.WriteLine("----- SEARCH STUDENT BY ID -----");

        int id = ReadNumber("Enter student ID: ");
        int position = studentIds.IndexOf(id);

        if (position != -1)
        {
            Console.WriteLine("\nID: " + studentIds[position]);
            Console.WriteLine("Name: " + studentNames[id]);
            Console.WriteLine("Age: " + studentAges[id]);
            Console.WriteLine("Phone: " + studentPhones[id]);
            Console.WriteLine("Email: " + studentEmails[id]);
        }
        else
        {
            Console.WriteLine("Student not found.");
        }

        Pause();
    }

    void SearchStudentByName()
    {
        Console.Clear();
        Console.WriteLine("----- SEARCH STUDENT BY NAME -----");

        string name = ReadText("Enter student name: ");
        bool found = false;

        for (int i = 0; i < studentIds.Count; i++)
        {
            if (studentNames[studentIds[i]].Contains(name,
                StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("\nID: " + studentIds[i]);
                Console.WriteLine("Name: " + studentNames[studentIds[i]]);
                Console.WriteLine("Age: " + studentAges[studentIds[i]]);
                Console.WriteLine("Phone: " + studentPhones[studentIds[i]]);
                Console.WriteLine("Email: " + studentEmails[studentIds[i]]);

                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("No students found.");
        }

        Pause();
    }

    void ModifyStudent()
    {
        Console.Clear();
        Console.WriteLine("----- MODIFY STUDENT -----");

        int id = ReadNumber("Enter student ID: ");
        int position = studentIds.IndexOf(id);

        if (position != -1)
        {
            Console.WriteLine("\nCurrent information:");
            Console.WriteLine("Name: " + studentNames[id]);
            Console.WriteLine("Age: " + studentAges[id]);
            Console.WriteLine("Phone: " + studentPhones[id]);
            Console.WriteLine("Email: " + studentEmails[id]);

            Console.WriteLine("\nEnter new information:");

            studentNames[id] = ReadText("New name: ");

            int age;

            do
            {
                age = ReadNumber("New age: ");

                if (age <= 0 || age > 100)
                {
                    Console.WriteLine("Enter an age between 1 and 100.");
                }

            } while (age <= 0 || age > 100);

            studentAges[id] = age;
            studentPhones[id] = ReadPhone();

            string email;

            do
            {
                email = ReadEmail();

                if (EmailExists(email, id))
                {
                    Console.WriteLine("This email belongs to another student.");
                }

            } while (EmailExists(email, id));

            studentEmails[id] = email;

            Console.WriteLine("\nStudent updated successfully!");
        }
        else
        {
            Console.WriteLine("Student not found.");
        }

        Pause();
    }

    void DeleteStudent()
    {
        Console.Clear();
        Console.WriteLine("----- DELETE STUDENT -----");

        int id = ReadNumber("Enter student ID: ");
        int position = studentIds.IndexOf(id);

        if (position != -1)
        {
            bool hasInscription = inscribedStudentIds.Contains(id);

            if (hasInscription)
            {
                Console.WriteLine("Cannot delete this student because they have active inscriptions.");
            }
            else
            {
                Console.Write("Confirm deletion? (Y/N): ");
                string confirmation = Console.ReadLine() ?? "";

                if ("Y".Equals(confirmation,
                    StringComparison.OrdinalIgnoreCase))
                {
                    studentIds.RemoveAt(position);
                    studentNames.Remove(id);
                    studentAges.Remove(id);
                    studentPhones.Remove(id);
                    studentEmails.Remove(id);

                    Console.WriteLine("Student deleted successfully!");
                }
                else
                {
                    Console.WriteLine("Deletion cancelled.");
                }
            }
        }
        else
        {
            Console.WriteLine("Student not found.");
        }
        Pause();
    }

    void ListStudents()
    {
        Console.Clear();
        Console.WriteLine("----- STUDENT LIST -----");

        if (studentIds.Count == 0)
        {
            Console.WriteLine("No students registered.");
        }
        else
        {
            for (int i = 0; i < studentIds.Count; i++)
            {
                Console.WriteLine("\nStudent ID: " + studentIds[i]);
                Console.WriteLine("Name: " + studentNames[studentIds[i]]);
                Console.WriteLine("Age: " + studentAges[studentIds[i]]);
                Console.WriteLine("Phone: " + studentPhones[studentIds[i]]);
                Console.WriteLine("Email: " + studentEmails[studentIds[i]]);
                Console.WriteLine("------------------------------");
            }

            Console.WriteLine("Total students: " + studentIds.Count);
        }

        Pause();
    }

    void StudentsMenu()
    {
        bool menuRunning = true;

        while (menuRunning)
        {
            Console.Clear();

            Console.WriteLine("----- STUDENTS MENU -----");
            Console.WriteLine("1. Add student");
            Console.WriteLine("2. Search by ID");
            Console.WriteLine("3. Search by name");
            Console.WriteLine("4. Modify student");
            Console.WriteLine("5. Delete student");
            Console.WriteLine("6. List students");
            Console.WriteLine("7. Return to main menu");

            int option = ReadNumber("\nSelect an option: ");

            switch (option)
            {
                case 1: AddStudent(); break;
                case 2: SearchStudentById(); break;
                case 3: SearchStudentByName(); break;
                case 4: ModifyStudent(); break;
                case 5: DeleteStudent(); break;
                case 6: ListStudents(); break;
                case 7: menuRunning = false; break;

                default:
                    Console.WriteLine("Invalid option.");
                    Pause();
                    break;
            }
        }
    }

    // DANCE CLASSES MODULE

    void AddClass()
    {
        Console.Clear();
        Console.WriteLine("----- ADD DANCE CLASS -----");

        string name = ReadText("Enter class name: ");
        string instructor = ReadText("Enter instructor name: ");
        string schedule = ReadText("Enter class schedule: ");

        int capacity;

        do
        {
            capacity = ReadNumber("Enter maximum capacity: ");

            if (capacity <= 0)
            {
                Console.WriteLine("Capacity must be greater than zero.");
            }

        } while (capacity <= 0);

        classIds.Add(nextClassId);
        classNames.Add(nextClassId, name);
        instructors.Add(nextClassId, instructor);
        schedules.Add(nextClassId, schedule);
        classCapacities.Add(nextClassId, capacity);

        Console.WriteLine("\nDance class registered successfully!");
        Console.WriteLine("Class ID: " + nextClassId);

        nextClassId++;

        Pause();
    }

    void SearchClassById()
    {
        Console.Clear();
        Console.WriteLine("----- SEARCH CLASS BY ID -----");

        int id = ReadNumber("Enter class ID: ");
        int position = classIds.IndexOf(id);

        if (position != -1)
        {
            Console.WriteLine("\nID: " + classIds[position]);
            Console.WriteLine("Class: " + classNames[id]);
            Console.WriteLine("Instructor: " + instructors[id]);
            Console.WriteLine("Schedule: " + schedules[id]);
            Console.WriteLine("Capacity: " + classCapacities[id]);
        }
        else
        {
            Console.WriteLine("Class not found.");
        }

        Pause();
    }

    void SearchClassByName()
    {
        Console.Clear();
        Console.WriteLine("----- SEARCH CLASS BY NAME -----");

        string name = ReadText("Enter class name: ");
        bool found = false;

        for (int i = 0; i < classIds.Count; i++)
        {
            if (classNames[classIds[i]].Contains(name,
                StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("\nID: " + classIds[i]);
                Console.WriteLine("Class: " + classNames[classIds[i]]);
                Console.WriteLine("Instructor: " + instructors[classIds[i]]);
                Console.WriteLine("Schedule: " + schedules[classIds[i]]);
                Console.WriteLine("Capacity: " + classCapacities[classIds[i]]);

                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("No classes found.");
        }

        Pause();
    }

    void SearchClassByInstructor()
    {
        Console.Clear();
        Console.WriteLine("----- SEARCH CLASS BY INSTRUCTOR -----");

        string instructor = ReadText("Enter instructor name: ");
        bool found = false;

        for (int i = 0; i < classIds.Count; i++)
        {
            if (instructors[classIds[i]].Contains(instructor,
                StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("\nID: " + classIds[i]);
                Console.WriteLine("Class: " + classNames[classIds[i]]);
                Console.WriteLine("Instructor: " + instructors[classIds[i]]);
                Console.WriteLine("Schedule: " + schedules[classIds[i]]);
                Console.WriteLine("Capacity: " + classCapacities[classIds[i]]);

                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("No classes found for this instructor.");
        }

        Pause();
    }

    void ModifyClass()
    {
        Console.Clear();
        Console.WriteLine("----- MODIFY DANCE CLASS -----");

        int id = ReadNumber("Enter class ID: ");
        int position = classIds.IndexOf(id);

        if (position != -1)
        {
            Console.WriteLine("\nEnter new information:");

            int capacity;
            int currentInscriptions = 0;

            for (int i = 0; i < inscribedClassIds.Count; i++)
            {
                if (inscribedClassIds[i] == id)
                {
                    currentInscriptions++;
                }
            }

            do
            {
                capacity = ReadNumber("New maximum capacity: ");

                if (capacity <= 0 || capacity < currentInscriptions)
                {
                    Console.WriteLine("Capacity cannot be less than current inscriptions.");
                }

            } while (capacity <= 0 || capacity < currentInscriptions);

            classNames[id] = ReadText("New class name: ");
            instructors[id] = ReadText("New instructor: ");
            schedules[id] = ReadText("New schedule: ");
            classCapacities[id] = capacity;

            Console.WriteLine("\nClass updated successfully!");
        }
        else
        {
            Console.WriteLine("Class not found.");
        }

        Pause();
    }

    void DeleteClass()
    {
        Console.Clear();
        Console.WriteLine("----- DELETE DANCE CLASS -----");

        int id = ReadNumber("Enter class ID: ");
        int position = classIds.IndexOf(id);

        if (position != -1)
        {
            if (inscribedClassIds.Contains(id))
            {
                Console.WriteLine("Cannot delete this class because it has active inscriptions.");
            }
            else
            {
                Console.Write("Confirm deletion? (Y/N): ");
                string confirmation = Console.ReadLine() ?? "";

                if ("Y".Equals(confirmation,
                    StringComparison.OrdinalIgnoreCase))
                {
                    classIds.RemoveAt(position);
                    classNames.Remove(id);
                    instructors.Remove(id);
                    schedules.Remove(id);
                    classCapacities.Remove(id);

                    Console.WriteLine("Class deleted successfully!");
                }
                else
                {
                    Console.WriteLine("Deletion cancelled.");
                }
            }
        }
        else
        {
            Console.WriteLine("Class not found.");
        }

        Pause();
    }

    void ListClasses()
    {
        Console.Clear();
        Console.WriteLine("----- DANCE CLASS LIST -----");

        if (classIds.Count == 0)
        {
            Console.WriteLine("No classes registered.");
        }
        else
        {
            for (int i = 0; i < classIds.Count; i++)
            {
                int inscribed = 0;

                for (int j = 0; j < inscribedClassIds.Count; j++)
                {
                    if (inscribedClassIds[j] == classIds[i])
                    {
                        inscribed++;
                    }
                }

                Console.WriteLine("\nClass ID: " + classIds[i]);
                Console.WriteLine("Name: " + classNames[classIds[i]]);
                Console.WriteLine("Instructor: " + instructors[classIds[i]]);
                Console.WriteLine("Schedule: " + schedules[classIds[i]]);
                Console.WriteLine("Capacity: " + classCapacities[classIds[i]]);
                Console.WriteLine("Inscriptions: " + inscribed);
                Console.WriteLine("------------------------------");
            }

            Console.WriteLine("Total classes: " + classIds.Count);
        }

        Pause();
    }

    void ClassesMenu()
    {
        bool menuRunning = true;

        while (menuRunning)
        {
            Console.Clear();

            Console.WriteLine("----- DANCE CLASSES MENU -----");
            Console.WriteLine("1. Add class");
            Console.WriteLine("2. Search by ID");
            Console.WriteLine("3. Search by name");
            Console.WriteLine("4. Search by instructor");
            Console.WriteLine("5. Modify class");
            Console.WriteLine("6. Delete class");
            Console.WriteLine("7. List classes");
            Console.WriteLine("8. Return to main menu");

            int option = ReadNumber("\nSelect an option: ");

            switch (option)
            {
                case 1: AddClass(); break;
                case 2: SearchClassById(); break;
                case 3: SearchClassByName(); break;
                case 4: SearchClassByInstructor(); break;
                case 5: ModifyClass(); break;
                case 6: DeleteClass(); break;
                case 7: ListClasses(); break;
                case 8: menuRunning = false; break;

                default:
                    Console.WriteLine("Invalid option.");
                    Pause();
                    break;
            }
        }
    }

    // INSCRIPTIONS MODULE

    void AddInscription()
    {
        Console.Clear();
        Console.WriteLine("----- ADD INSCRIPTION -----");

        if (studentIds.Count == 0 || classIds.Count == 0)
        {
            Console.WriteLine("You must register students and classes first.");
            Pause();
            return;
        }

        Console.WriteLine("\nAvailable students:");

        for (int i = 0; i < studentIds.Count; i++)
        {
            Console.WriteLine(studentIds[i] + " - " + studentNames[studentIds[i]]);
        }

        int studentId = ReadNumber("\nEnter student ID: ");
        int studentPosition = studentIds.IndexOf(studentId);

        if (studentPosition == -1)
        {
            Console.WriteLine("Student not found.");
            Pause();
            return;
        }

        Console.WriteLine("\nAvailable classes:");

        for (int i = 0; i < classIds.Count; i++)
        {
            int inscribed = 0;

            for (int j = 0; j < inscribedClassIds.Count; j++)
            {
                if (inscribedClassIds[j] == classIds[i])
                {
                    inscribed++;
                }
            }

            Console.WriteLine(
                classIds[i] + " - " + classNames[classIds[i]] +
                " | Available spaces: " +
                (classCapacities[classIds[i]] - inscribed));
        }

        int classId = ReadNumber("\nEnter class ID: ");
        int classPosition = classIds.IndexOf(classId);

        if (classPosition == -1)
        {
            Console.WriteLine("Class not found.");
            Pause();
            return;
        }

        bool alreadyInscribed = false;

        for (int i = 0; i < inscribedStudentIds.Count; i++)
        {
            if (inscribedStudentIds[i] == studentId &&
                inscribedClassIds[i] == classId)
            {
                alreadyInscribed = true;
                break;
            }
        }

        if (alreadyInscribed)
        {
            Console.WriteLine(
                "This student is already inscribed in this class.");
            Pause();
            return;
        }

        int currentInscriptions = 0;

        for (int i = 0; i < inscribedClassIds.Count; i++)
        {
            if (inscribedClassIds[i] == classId)
            {
                currentInscriptions++;
            }
        }

        if (currentInscriptions >= classCapacities[classIds[classPosition]])
        {
            Console.WriteLine("This class has reached its maximum capacity.");
            Pause();
            return;
        }

        string date = ReadDate("Enter inscription date (DD/MM/YYYY): ");

        inscriptionIds.Add(nextInscriptionId);
        inscribedStudentIds.Add(studentId);
        inscribedClassIds.Add(classId);
        inscriptionDates.Add(nextInscriptionId, date);

        Console.WriteLine("\nInscription registered successfully!");
        Console.WriteLine("Inscription ID: " + nextInscriptionId);

        nextInscriptionId++;

        Pause();
    }

    void SearchInscriptionById()
    {
        Console.Clear();
        Console.WriteLine("----- SEARCH INSCRIPTION BY ID -----");

        int id = ReadNumber("Enter inscription ID: ");
        int position = inscriptionIds.IndexOf(id);

        if (position != -1)
        {
            int studentPosition = studentIds.IndexOf(
                inscribedStudentIds[position]);

            int classPosition = classIds.IndexOf(
                inscribedClassIds[position]);

            Console.WriteLine("\nInscription ID: " + inscriptionIds[position]);
            Console.WriteLine("Student: " + studentNames[studentIds[studentPosition]]);
            Console.WriteLine("Class: " + classNames[classIds[classPosition]]);
            Console.WriteLine("Date: " + inscriptionDates[id]);
        }
        else
        {
            Console.WriteLine("Inscription not found.");
        }

        Pause();
    }

    void SearchInscriptionByStudent()
    {
        Console.Clear();
        Console.WriteLine("----- SEARCH INSCRIPTIONS BY STUDENT -----");

        int studentId = ReadNumber("Enter student ID: ");

        if (!studentIds.Contains(studentId))
        {
            Console.WriteLine("Student not found.");
            Pause();
            return;
        }

        bool found = false;

        for (int i = 0; i < inscriptionIds.Count; i++)
        {
            if (inscribedStudentIds[i] == studentId)
            {
                int classPosition = classIds.IndexOf(
                    inscribedClassIds[i]);

                Console.WriteLine("\nInscription ID: " + inscriptionIds[i]);
                Console.WriteLine("Student: " +
                    studentNames[studentId]);
                Console.WriteLine("Class: " + classNames[classIds[classPosition]]);
                Console.WriteLine("Date: " + inscriptionDates[inscriptionIds[i]]);

                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("This student has no inscriptions.");
        }

        Pause();
    }

    void SearchInscriptionByClass()
    {
        Console.Clear();
        Console.WriteLine("----- SEARCH INSCRIPTIONS BY CLASS -----");

        int classId = ReadNumber("Enter class ID: ");

        if (!classIds.Contains(classId))
        {
            Console.WriteLine("Class not found.");
            Pause();
            return;
        }

        bool found = false;

        for (int i = 0; i < inscriptionIds.Count; i++)
        {
            if (inscribedClassIds[i] == classId)
            {
                int studentPosition = studentIds.IndexOf(
                    inscribedStudentIds[i]);

                Console.WriteLine("\nInscription ID: " + inscriptionIds[i]);
                Console.WriteLine("Student: " + studentNames[studentIds[studentPosition]]);
                Console.WriteLine("Class: " +
                    classNames[classId]);
                Console.WriteLine("Date: " + inscriptionDates[inscriptionIds[i]]);

                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("This class has no inscriptions.");
        }

        Pause();
    }

    void ModifyInscription()
    {
        Console.Clear();
        Console.WriteLine("----- MODIFY INSCRIPTION -----");

        int id = ReadNumber("Enter inscription ID: ");
        int position = inscriptionIds.IndexOf(id);

        if (position == -1)
        {
            Console.WriteLine("Inscription not found.");
            Pause();
            return;
        }

        Console.WriteLine("\nEnter the new student ID:");
        int studentId = ReadNumber("Student ID: ");

        if (!studentIds.Contains(studentId))
        {
            Console.WriteLine("Student not found.");
            Pause();
            return;
        }

        Console.WriteLine("\nEnter the new class ID:");
        int classId = ReadNumber("Class ID: ");
        int classPosition = classIds.IndexOf(classId);

        if (classPosition == -1)
        {
            Console.WriteLine("Class not found.");
            Pause();
            return;
        }

        bool duplicate = false;

        for (int i = 0; i < inscriptionIds.Count; i++)
        {
            if (i != position &&
                inscribedStudentIds[i] == studentId &&
                inscribedClassIds[i] == classId)
            {
                duplicate = true;
                break;
            }
        }

        if (duplicate)
        {
            Console.WriteLine("This student is already inscribed in this class.");
            Pause();
            return;
        }

        int inscribed = 0;

        for (int i = 0; i < inscribedClassIds.Count; i++)
        {
            if (i != position && inscribedClassIds[i] == classId)
            {
                inscribed++;
            }
        }

        if (inscribed >= classCapacities[classIds[classPosition]])
        {
            Console.WriteLine("This class has reached its maximum capacity.");
            Pause();
            return;
        }

        inscribedStudentIds[position] = studentId;
        inscribedClassIds[position] = classId;
        inscriptionDates[id] = ReadDate("New inscription date (DD/MM/YYYY): ");

        Console.WriteLine("\nInscription updated successfully!");

        Pause();
    }

    void DeleteInscription()
    {
        Console.Clear();
        Console.WriteLine("----- DELETE INSCRIPTION -----");

        int id = ReadNumber("Enter inscription ID: ");
        int position = inscriptionIds.IndexOf(id);

        if (position != -1)
        {
            Console.Write("Confirm deletion? (Y/N): ");
            string confirmation = Console.ReadLine() ?? "";

            if ("Y".Equals(confirmation,
                StringComparison.OrdinalIgnoreCase))
            {
                inscriptionIds.RemoveAt(position);
                inscribedStudentIds.RemoveAt(position);
                inscribedClassIds.RemoveAt(position);
                inscriptionDates.Remove(id);

                Console.WriteLine("Inscription deleted successfully!");
            }
            else
            {
                Console.WriteLine("Deletion cancelled.");
            }
        }
        else
        {
            Console.WriteLine("Inscription not found.");
        }

        Pause();
    }

    void ListInscriptions()
    {
        Console.Clear();
        Console.WriteLine("----- INSCRIPTION LIST -----");

        if (inscriptionIds.Count == 0)
        {
            Console.WriteLine("No inscriptions registered.");
        }
        else
        {
            for (int i = 0; i < inscriptionIds.Count; i++)
            {
                int studentPosition = studentIds.IndexOf(
                    inscribedStudentIds[i]);

                int classPosition = classIds.IndexOf(
                    inscribedClassIds[i]);

                Console.WriteLine("\nInscription ID: " + inscriptionIds[i]);
                Console.WriteLine("Student: " + studentNames[studentIds[studentPosition]]);
                Console.WriteLine("Class: " + classNames[classIds[classPosition]]);
                Console.WriteLine("Date: " + inscriptionDates[inscriptionIds[i]]);
                Console.WriteLine("------------------------------");
            }

            Console.WriteLine(
                "Total inscriptions: " + inscriptionIds.Count);
        }

        Pause();
    }

    void InscriptionsMenu()
    {
        bool menuRunning = true;

        while (menuRunning)
        {
            Console.Clear();

            Console.WriteLine("----- INSCRIPTIONS MENU -----");
            Console.WriteLine("1. Add inscription");
            Console.WriteLine("2. Search by ID");
            Console.WriteLine("3. Search by student");
            Console.WriteLine("4. Search by class");
            Console.WriteLine("5. Modify inscription");
            Console.WriteLine("6. Delete inscription");
            Console.WriteLine("7. List inscriptions");
            Console.WriteLine("8. Return to main menu");

            int option = ReadNumber("\nSelect an option: ");

            switch (option)
            {
                case 1: AddInscription(); break;
                case 2: SearchInscriptionById(); break;
                case 3: SearchInscriptionByStudent(); break;
                case 4: SearchInscriptionByClass(); break;
                case 5: ModifyInscription(); break;
                case 6: DeleteInscription(); break;
                case 7: ListInscriptions(); break;
                case 8: menuRunning = false; break;

                default:
                    Console.WriteLine("Invalid option.");
                    Pause();
                    break;
            }
        }
    }

    // MAIN MENU

    bool running = true;

    while (running)
    {
        Console.Clear();

        Console.WriteLine("--------------------------------------");
        Console.WriteLine("             DANCEFLOW");
        Console.WriteLine("     DANCE CLASS MANAGEMENT SYSTEM");
        Console.WriteLine("--------------------------------------");
        Console.WriteLine("1. Students");
        Console.WriteLine("2. Dance Classes");
        Console.WriteLine("3. Inscriptions");
        Console.WriteLine("4. Exit");
        Console.WriteLine("--------------------------------------");

        int option = ReadNumber("\nSelect an option: ");

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

            default:
                Console.WriteLine("Invalid option. Try again.");
                Pause();
                break;
        }
    }
}
catch (Exception error)
{
    Console.WriteLine("\nAn error occurred: " + error.Message);
}
