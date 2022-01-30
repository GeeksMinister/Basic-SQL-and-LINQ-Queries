using Labb3_Anropa_databasen.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

internal static partial class Database
{
    
    public static void PrintMenu()
        
    {
        while (true)
        {
            Clear();
            Write($"\n\t\t\t* (( Welcome to Happy Valley School )) * \n\n\n"+
                "  [1] Staff \n\n" +
                "  [2] All students\n\n" +
                "  [3] Students from a specific class\n\n" +
                "  [4] Submitted grades in the last (6) months\n\n" +
                "  [5] Submitted grades in the last month (View)\n\n" +
                "  [6] A list of courses with the average, lowest, and highest grades\n\n" +
                "  [7] Register a new student \n\n" +
                "  [8] Register a new staff member\n\n" +
                "  [9] Exit \n\n\n" +
                "   \t-Choose what you want to inquire:  ");
            int.TryParse(ReadLine(), out int option);
            switch (option)
            {
                case 1:
                    Clear();
                    ConnectionSwitcher();
                    StaffMenu();
                    ConnectionSwitcher();
                    Redirecting();
                    break;
                case 2:
                    Clear();
                    StudentsMenu();
                    Redirecting();
                    break;
                case 3:
                    Clear();
                    PrintClasses();
                    Redirecting();
                    break;
                case 4:
                    Clear();
                    ConnectionSwitcher();
                    ExecuteSqlAndPrint(LastSubmittedGrades());
                    ConnectionSwitcher();
                    Redirecting();
                    break;
                case 5:
                    Clear();
                    ConnectionSwitcher();
                    ViewRecentGrades();
                    ConnectionSwitcher();
                    Redirecting();
                    break;
                case 6:
                    Clear();
                    ConnectionSwitcher();
                    MaxMinAvgGrades();
                    ConnectionSwitcher();
                    Redirecting();
                    break;
                case 7:
                    Clear();
                    ConnectionSwitcher();
                    AddStudentInput();
                    ConnectionSwitcher();
                    Redirecting();
                    break;
                case 8:
                    Clear();
                    ConnectionSwitcher();
                    AddStaffInput();
                    ConnectionSwitcher();
                    Redirecting();
                    break;
                case 9:
                    Clear();
                    WriteLine("\n\n\n\n\t\tHave a wonderful day :-)");
                    Thread.Sleep(1800);
                    return;
                default:
                    WriteLine("\t\tInvalid input!   " +
                    "Please choose from 1 - 10\n");
                    ReadKey();
                    break;
            }
            // Save changes ??
        }
    }
    public static void Redirecting()
    {
        Write("\n\n\n\t\tPress 'Enter' to return to the main menu");
        ReadLine();
    }

    private static void StudentsMenu()
    {
        try
        {
            int option = 0;
            while (option > 4 || option < 1)
            {
                Clear();
                Write("\n\n  \t[1] Sorted by first name - Ascending\n\n" +
                  "  \t[2] Sorted by last name - Ascending \n\n" +
                  "\t[3] Sorted by first name - Descending \n\n" +
                  "\t[4] Sorted by last name - Descending\n\n\n" +
                            "   \t-Choose a sorting method:  ");
                int.TryParse(ReadLine(), out option);
            }

            PrintEntitiesEF(StudentQuery(option));
        }
        catch (Exception e)
        {
            WriteLine(e.Message);
        }
    }

    private static IOrderedQueryable<Student>? StudentQuery(int option)
    {
        HappyValleyContext context = new HappyValleyContext();
        IOrderedQueryable<Student> students;
        switch (option)
        {
            case 1:
                students = from student in context.Students
                           orderby student.FirstName
                           select student;
                return students;
            case 2:
                students = from student in context.Students
                           orderby student.LastName
                           select student;
                return students;
            case 3:
                students = from student in context.Students
                           orderby student.FirstName descending
                           select student;
                return students;
            case 4:
                students = from student in context.Students
                           orderby student.LastName descending
                           select student;
                return students;
        }
        return null;
    }

    private static void PrintEntitiesEF<T>(T table)
    {
        if (table is IOrderedQueryable<Student>)
        {
            IOrderedQueryable<Student>? student = table as IOrderedQueryable<Student>;
            StringBuilder sb = new StringBuilder();
            foreach (var item in student)
            {
                sb.Clear();
                    sb.Append($"{item.FirstName}      " +
                        $"{item.LastName}      {item.Ssn}      {item.Email}" +
                        $"     ({item.Class})");
                WriteLine($"\n\n\t[{item.Id}]      {sb} \n" +
                    $"     ________________________________________________________" +
                    $"______________________________________________________________" +
                    $"_________________");
            }
        }
        //For other table types in the futrure 
        else if (true)
        {

        }
    }

    private static void PrintClasses()
    {
        HappyValleyContext context = new HappyValleyContext();
        int classesCount = (from student in context.Students
                       select student.Class).Distinct().Count();

        List<string> list = (from student in context.Students
                       select student.Class).Distinct().ToList();
        for (int i = 0; i < classesCount; i++)
        {
            WriteLine($"\n\n\t[{i + 1}] {list[i]}");
        }
        SelectClass(list, out string className);
        InquireStudentInClass(className);
    }

    private static void SelectClass(List<string> list, out string className)
    {
        int choose = 0;
        while ((choose < 1) || (choose > list.Count))
        {
            Write("\n\n\tPlease choose a class: ");

            int.TryParse(ReadLine(), out choose);
        }
        className = list[choose - 1];
    }
    
    private static string SortedByColumn()
    {
        string sortBy;
        Clear();
        Write("\n\n\tHow would you like the rows to be sorted ?\n\n" +
            "\t[1] By First name\n\n" +
            "\t[2] By Last name\n\n\n");
        int choose = 0;
        while ((choose < 1) || (choose > 2))
        {
            Write("\n\n\tPlease choose from 1 - 2 : ");

            int.TryParse(ReadLine(), out choose);
        }

        return sortBy = (choose == 1) ? "First name" : "Last name";
    }

    private static void InquireStudentInClass(string className)
    {
        HappyValleyContext context = new HappyValleyContext();
        string orderBy = SortedByColumn();
        if (orderBy == "First name")
        {
            IOrderedQueryable<Student> students = from student in context.Students
                                                  where student.Class == className
                                                  orderby student.FirstName
                                                  select student;
            PrintEntitiesEF(students);
        }
        else if (orderBy == "Last name")
        {
            IOrderedQueryable<Student> students = from student in context.Students
                                                  where student.Class == className
                                                  orderby student.LastName
                                                  select student;
            PrintEntitiesEF(students);
        }
    }

    private static void AddStaffInput()
    {
        Write("\n\n\t\tAdd a new staff member\n");
        string firstName = string.Empty;
        string lastName = string.Empty;
        string position = string.Empty;
        string ssn = "1000000000";
        string phone = string.Empty;
        string email = string.Empty;

        DateOnly employmentDate = DateOnly.FromDateTime(DateTime.Now);
        while (firstName == "" || firstName.Length < 2 || firstName.Any(char.IsDigit))
        {
            Write("\n\n\tType in the first name: ");
            firstName = ReadLine().Trim();
        }
        while (lastName == "" || lastName.Length < 2 || lastName.Any(char.IsDigit))
        {
            Write("\n\n\tType in the last name: ");
            lastName = ReadLine().Trim();
        }
        while (position == "" || position.Length < 4 || position.Any(char.IsDigit))
        {
            Write("\n\n\tType in the postion: ");
            position = ReadLine().Trim();
        }
        while (ssn == "1000000000" || ssn.ToString().Length != 10)
        {
            Write("\n\n\tType in the SSN (10 digits):  ");
            ssn = ReadLine().Trim();
        }
        while (phone == "1000000000" || phone.ToString().Length != 10)
        {
            Write("\n\n\tType in the phone number (10 digits):  ");
            phone = ReadLine().Trim();
        }

        email = $"{firstName.ToLower()}.{lastName.ToLower()}@happyvalley.com";
        AddStaffEF(firstName, lastName, position, employmentDate, ssn, phone, email);
    }

    private static void AddStaffEF(string firstName, string lastName, string position,
        DateOnly employmentDate, string ssn, string phone, string email)
    {
        try
        {
            using HappyValleyContext context = new HappyValleyContext();
            Staff staff = new Staff()
            {
                FirstName = firstName,
                LastName = lastName,
                Position = position,
                EmploymentDate = employmentDate.ToString(),
                Ssn = ssn,
                Phone = phone,
                Email = email
            };
            context.Staff.Add(staff);
            context.SaveChanges();
        }
        catch (Exception e)
        {
            WriteLine(e.Message);
        }
    }
}

