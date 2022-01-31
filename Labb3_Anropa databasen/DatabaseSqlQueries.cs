using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Text;

internal static partial class Database
{
    private static readonly string ConnectionString =
        "Server=(LocalDB)\\MSSQLLocalDB; Database=HappyValleyTest;" +
        " Trusted_Connection=True;" + " MultipleActiveResultSets=True";
    private static readonly SqlConnection Connection =
        new SqlConnection(ConnectionString);

    private static async void ConnectionSwitcher()
    {
        try
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
            else if (Connection.State == ConnectionState.Open)
            {
                await Connection.CloseAsync();
            }
        }
        catch (Exception e)
        {
            WriteLine(e.Message);
        }
    }

    private static void ExecuteSqlAndPrint(string query)
    {
        try
        {
            SqlCommand cmd = new SqlCommand(query, Connection);
            SqlDataReader reader = cmd.ExecuteReader();
            StringBuilder sb = new StringBuilder();
            int id;
            while (reader.Read())
            {
                sb.Clear();
                id = reader.GetInt32(0);
                for (int i = 1; i < reader.FieldCount; i++)
                {
                    if (reader.GetFieldType(i).ToString() == "System.DateTime")
                    {
                        sb.Append(reader.GetDateTime(i) + "      ");
                    }
                    else
                    {
                        sb.Append(reader.GetString(i) + "      ");
                    }
                }
                WriteLine($"\n\n\t[{id}]      {sb} \n" +
                    $"     ________________________________________________________" +
                    $"______________________________________________________________" +
                    $"_________________");
            }

        }
        catch(Exception e)
        {
            WriteLine(e.Message);
        }
    }

    private static string? ExecuteSqlAndRead(string query)
    {
        try
        {
            SqlCommand cmd = new SqlCommand(query, Connection);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            if (reader.GetFieldType(0).ToString() == "System.Int32")
            {
                return reader.GetInt32(0).ToString();
            }
            return reader.GetString(0);

        }
        catch(Exception e)
        {
            WriteLine(e.Message);
        }
        return null;
    }

    private static List<string> ExecuteSqlAndAddToList(string query)
    {
        List<string> list = new List<string>();
        try
        {
            SqlCommand cmd = new SqlCommand(query,Connection);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                if (reader.GetFieldType(0).ToString() == "System.Int32")
                {
                    list.Add(reader.GetInt32(0).ToString());
                }
                if (reader.GetFieldType(0).ToString() == "System.DateTime")
                {
                    list.Add(reader.GetDateTime(0).ToString());
                }
                else
                {
                    list.Add(reader.GetString(0));
                }
            }
        }
        catch (Exception e)
        {
            WriteLine(e.Message);
        }
            return list;
    }

    public static void StaffMenu()
    {
        try
        {
            while (true)
            {
                Clear();
                Write("\n\n  \t[1] Get staff of all positions \n\n" +
                  "  \t[2] Specify the position type \n\n\n" +
                "   \t-Choose:  ");
                int.TryParse(ReadLine(), out int option);
                switch (option)
                {
                    case 1:
                        Clear();
                        ExecuteSqlAndPrint("SELECT * FROM Staff");
                        return;
                    case 2:
                        Clear();
                        StaffWherePosition();
                        return;
                    default:
                        WriteLine("\t\tInvalid input!   " +
                            "Please choose from 1 - 2\n");
                        ReadKey();
                        break;
                }
            }
        }
        catch (Exception e)
        {
            WriteLine(e.Message);
        }
    }

    private static void StaffWherePosition()
    {
            List<string> positions = new List<string>();
            string query = "SELECT COUNT(DISTINCT Position) FROM Staff";
        try
        {
            SqlCommand cmd = new SqlCommand(query, Connection);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            int positionTypes = reader.GetInt32(0);

            query = "SELECT DISTINCT Position FROM Staff";
            cmd = new SqlCommand(query, Connection);
            reader = cmd.ExecuteReader();

            for (int i = 0; i < positionTypes; i++)
            {
                reader.Read();
                Write($"\n\n\t[{i + 1}] {reader.GetString(0)}");
                positions.Add(reader.GetString(0));
            }
        }
        catch(Exception e)
        {
            WriteLine(e.Message);
        }
        SelectStaffPosition(positions, out string position);
        query = $"SELECT * FROM Staff WHERE  Position= '{position}'";
        ExecuteSqlAndPrint(query);
    }

    private static void SelectStaffPosition(List<string> list, out string position)
    {
        int choose = 0;
        while ((choose < 1) || (choose > list.Count))
        {
            Write("\n\n\tChoose the position type you want to inquire : ");
            int.TryParse(ReadLine(), out choose);
        }
        position = list[choose - 1];
    }

    private static string LastSubmittedGrades()
    {
        return "SELECT Math.StudentId, CONCAT(Student.FirstName,' ' " +
            ",Student.LastName), Math.Grade, 'Math'" +
            " , Math.GradeSubmitted , CONCAT(Staff.FirstName," +
            " ' ', Staff.LastName) FROM Student, Math," +
            " Staff WHERE DATEDIFF(MONTH, [Date], GETDATE()) <= 6 AND Grade IS NOT NULL" +
            " AND Staff.id = TeacherId AND Math.StudentId = Student.id " +
            "UNION " +
            "SELECT English.StudentId, CONCAT(Student.FirstName,' ' " +
            ",Student.LastName) , English.Grade, 'English'" +
            " , English.GradeSubmitted , CONCAT(Staff.FirstName," +
            " ' ', Staff.LastName) FROM Student, English," +
            " Staff WHERE DATEDIFF(MONTH, [Date], GETDATE()) <= 6 AND Grade IS NOT NULL" +
            " AND Staff.id = TeacherId AND English.StudentId = Student.id " +
            "UNION " +
            "SELECT Programming.StudentId, CONCAT(Student.FirstName,' ' " +
            ",Student.LastName) , Programming.Grade, 'Programming'" +
            " , Programming.GradeSubmitted , CONCAT(Staff.FirstName," +
            " ' ', Staff.LastName) FROM Student, Programming," +
            " Staff WHERE DATEDIFF(MONTH, [Date], GETDATE()) <= 6 AND Grade IS NOT NULL" +
            " AND Staff.id = TeacherId AND Programming.StudentId = Student.id " +
            "ORDER BY GradeSubmitted DESC ";
    }

    private static void MaxMinAvgGrades()
    {
        string[] mathGrades = new string[6];
        string[] englishGrades = new string[6];
        string[] programmingGrades = new string[6];
        //[0] = Lowest, [1] = Avg, [2] = Highest, [3] = Subject, [4] = MaleAvg, [5] = FemaleAvg.

        mathGrades[0] = ExecuteSqlAndRead("SELECT MAX(Math.Grade) FROM Math;");
        mathGrades[1] = ExecuteSqlAndRead("SELECT AVG(CASE " +
            "WHEN Grade = 'F' THEN 0 WHEN Grade = 'E' THEN 1 WHEN Grade = 'D' THEN 2 " +
            "WHEN Grade = 'C' THEN 3 WHEN Grade = 'B' THEN 4 WHEN Grade = 'A' THEN 5 " +
            "END) FROM Math");
        mathGrades[2] = ExecuteSqlAndRead("SELECT MIN(Math.Grade) FROM Math;");
        mathGrades[3] = "Math";
        mathGrades[4] = ExecuteSqlAndRead("SELECT AVG(CASE WHEN Grade = 'F' THEN" +
            " 0 WHEN Grade = 'E' THEN 1 WHEN Grade = 'D' THEN 2 WHEN Grade = 'C'" +
            " THEN 3 WHEN Grade = 'B' THEN 4 WHEN Grade = 'A' THEN 5 END) FROM Student," +
            " Math WHERE SUBSTRING(Student.SSN, 9,1) % 2 != 0 AND Grade IS NOT NULL" +
            " AND Math.StudentId = Student.id");
        mathGrades[5] = ExecuteSqlAndRead("SELECT AVG(CASE WHEN Grade = 'F' THEN" +
            " 0 WHEN Grade = 'E' THEN 1 WHEN Grade = 'D' THEN 2 WHEN Grade = 'C'" +
            " THEN 3 WHEN Grade = 'B' THEN 4 WHEN Grade = 'A' THEN 5 END) FROM Student," +
            " Math WHERE SUBSTRING(Student.SSN, 9,1) % 2 = 0 AND Grade IS NOT NULL" +
            " AND Math.StudentId = Student.id");

        englishGrades[0] = ExecuteSqlAndRead("SELECT MAX(English.Grade) FROM English;");
        englishGrades[1] = ExecuteSqlAndRead("SELECT AVG(CASE " +
            "WHEN Grade = 'F' THEN 0 WHEN Grade = 'E' THEN 1 WHEN Grade = 'D' THEN 2 " +
            "WHEN Grade = 'C' THEN 3 WHEN Grade = 'B' THEN 4 WHEN Grade = 'A' THEN 5 " +
            "END) FROM English");
        englishGrades[2] = ExecuteSqlAndRead("SELECT MIN(English.Grade) FROM English;");
        englishGrades[3] = "English";
        englishGrades[4] = ExecuteSqlAndRead("SELECT AVG(CASE WHEN Grade = 'F' THEN" +
            " 0 WHEN Grade = 'E' THEN 1 WHEN Grade = 'D' THEN 2 WHEN Grade = 'C'" +
            " THEN 3 WHEN Grade = 'B' THEN 4 WHEN Grade = 'A' THEN 5 END) FROM Student," +
            " English WHERE SUBSTRING(Student.SSN, 9,1) % 2 != 0 AND Grade IS NOT NULL" +
            " AND English.StudentId = Student.id");
        englishGrades[5] = ExecuteSqlAndRead("SELECT AVG(CASE WHEN Grade = 'F' THEN" +
            " 0 WHEN Grade = 'E' THEN 1 WHEN Grade = 'D' THEN 2 WHEN Grade = 'C'" +
            " THEN 3 WHEN Grade = 'B' THEN 4 WHEN Grade = 'A' THEN 5 END) FROM Student," +
            " English WHERE SUBSTRING(Student.SSN, 9,1) % 2 = 0 AND Grade IS NOT NULL" +
            " AND English.StudentId = Student.id");

        programmingGrades[0] = ExecuteSqlAndRead("SELECT MAX(Programming.Grade) FROM Programming;");
        programmingGrades[1] = ExecuteSqlAndRead("SELECT AVG(CASE " +
            "WHEN Grade = 'F' THEN 0 WHEN Grade = 'E' THEN 1 WHEN Grade = 'D' THEN 2 " +
            "WHEN Grade = 'C' THEN 3 WHEN Grade = 'B' THEN 4 WHEN Grade = 'A' THEN 5 " +
            "END) FROM Programming");
        programmingGrades[2] = ExecuteSqlAndRead("SELECT MIN(Programming.Grade) FROM Programming;");
        programmingGrades[3] = "Programming";
        programmingGrades[4] = ExecuteSqlAndRead("SELECT AVG(CASE WHEN Grade = 'F' THEN" +
            " 0 WHEN Grade = 'E' THEN 1 WHEN Grade = 'D' THEN 2 WHEN Grade = 'C'" +
            " THEN 3 WHEN Grade = 'B' THEN 4 WHEN Grade = 'A' THEN 5 END) FROM Student," +
            " Programming WHERE SUBSTRING(Student.SSN, 9,1) % 2 != 0 AND Grade IS NOT NULL" +
            " AND Programming.StudentId = Student.id");
        programmingGrades[5] = ExecuteSqlAndRead("SELECT AVG(CASE WHEN Grade = 'F' THEN" +
            " 0 WHEN Grade = 'E' THEN 1 WHEN Grade = 'D' THEN 2 WHEN Grade = 'C'" +
            " THEN 3 WHEN Grade = 'B' THEN 4 WHEN Grade = 'A' THEN 5 END) FROM Student," +
            " Programming WHERE SUBSTRING(Student.SSN, 9,1) % 2 = 0 AND Grade IS NOT NULL" +
            " AND Programming.StudentId = Student.id");
        ChangeGrades(mathGrades);
        ChangeGrades(englishGrades);
        ChangeGrades(programmingGrades);
        PrintMinAvgMaxGrades(mathGrades);
        PrintMinAvgMaxGrades(englishGrades);
        PrintMinAvgMaxGrades(programmingGrades);
    }

    private static void ConvertToABCD(ref string grade)
    {
        switch (grade)
        {
            case "0":
                grade = "F";
                return;
            case "1":
                grade = "E";
                return;
            case "2":
                grade = "D";
                return;
            case "3":
                grade = "C";
                return;
            case "4":
                grade = "B";
                return;
            case "5":
                grade = "A";
                return;
        }
    }
    private static void ChangeGrades(string[] grades)
    {
        ConvertToABCD(ref grades[1]);    //[1] = Avg,
        ConvertToABCD(ref grades[4]);    //[4] = MaleAvg,
        ConvertToABCD(ref grades[5]);    //[5] = FemaleAvg,
    }
    private static void PrintMinAvgMaxGrades(string[] subject)
    {
        WriteLine($"\n\n\t\t{subject[3]}\n\n\tLowest grade: {subject[0]}" +
            $"\n\n\tAverage grade: {subject[1]}\n\n\tHighest grade: {subject[2]}" +
            $"\n\n\tAverage grade for males: {subject[4]}" +
            $"\n\n\tAverage grade for females: {subject[5]}");
    }

    private static void AddStudentInput()
    {
        Write("\n\n\t\tAdd Student\n");
        string firstName = string.Empty;
        string lastName = string.Empty;
        string email = string.Empty;
        string className = string.Empty;
        string ssn = "1000000000";

        while(firstName == "" || firstName.Length < 2 || firstName.Any(char.IsDigit))
        {
            Write("\n\n\tType in the first name: ");
            firstName = ReadLine().Trim();
        }
        while(lastName == "" || lastName.Length < 2 || lastName.Any(char.IsDigit))
        {
            Write("\n\n\tType in the last name: ");
            lastName = ReadLine().Trim();
        }
        while(ssn == "1000000000" || ssn.ToString().Length != 10 )
        {
            Write("\n\n\tType in the SSN (10 digits):  ");
            ssn = ReadLine().Trim();
        }
        email = $"{firstName.ToLower()}.{lastName.ToLower()}@happyvalley.com";
        className = PrintClassesSql();
        string addStudentQuery = $"INSERT INTO Student(FirstName, LastName, Email," +
            $" SSN, Class) VALUES ('{firstName}', '{lastName}', '{email}'," +
            $" '{ssn}', '{className}')";
        try
        {
            SqlCommand cmd = new SqlCommand(addStudentQuery, Connection);
            cmd.ExecuteReader();
        }
        catch (Exception ex)
        {
            WriteLine(ex.Message);
        }
    }

    private static string PrintClassesSql() 
    {
        Clear();
        List<string> classes;
        string countQuery = "SELECT COUNT(DISTINCT(Class)) FROM Student";
        string classesQuery = "SELECT DISTINCT(Class) FROM Student";
        int classesCount = int.Parse(ExecuteSqlAndRead(countQuery));
        classes = ExecuteSqlAndAddToList(classesQuery);
        for (int i = 0; i < classesCount; i++)
        {
            WriteLine($"\n\n\t[{i + 1}] {classes[i]}");
        }
        SelectClass(classes, out string className);
        return className;
    }

    private static void ViewRecentGrades()
    {
        string query = "SELECT * FROM [Recent Grades];";
        ExecuteSqlAndPrint(query);
    }
}
