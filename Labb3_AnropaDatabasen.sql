/* Hämta ut personal (ska lösas med SQL) */
SELECT * FROM Staff;
SELECT * FROM Staff WHERE  Position= 'Teacher'

/* Hämta ut alla betyg som sats senaste månaden. 
(Har gjort en query för att hämta senaste månaden genom VIEW, och en för att hämta senaste 6 månader) */ 
SELECT * FROM [Recent Grades]; /* VIEW */

SELECT Math.StudentId, CONCAT(Student.FirstName,' ' ,Student.LastName), Math.Grade, 'Math',
Math.GradeSubmitted , CONCAT(Staff.FirstName,  ' ', Staff.LastName) FROM Student, Math,
Staff WHERE DATEDIFF(MONTH, [Date], GETDATE()) <= 6 AND Grade IS NOT NULL AND 
Staff.id = TeacherId AND Math.StudentId = Student.id 
UNION 
SELECT English.StudentId, CONCAT(Student.FirstName,' ' ,Student.LastName) ,
English.Grade, 'English' , English.GradeSubmitted , 
CONCAT(Staff.FirstName, ' ', Staff.LastName) FROM Student, English, Staff 
WHERE DATEDIFF(MONTH, [Date], GETDATE()) <= 6 AND Grade IS NOT NULL AND
Staff.id = TeacherId AND English.StudentId = Student.id 
UNION
SELECT Programming.StudentId, CONCAT(Student.FirstName,' ' ,Student.LastName)
, Programming.Grade, 'Programming', Programming.GradeSubmitted , CONCAT(Staff.FirstName,
' ', Staff.LastName) FROM Student, Programming, Staff 
WHERE DATEDIFF(MONTH, [Date], GETDATE()) <= 6 AND Grade IS NOT NULL AND 
Staff.id = TeacherId AND Programming.StudentId = Student.id 
ORDER BY GradeSubmitted DESC;

/* Hämta ut en lista med alla kurser och det snittbetyg som eleverna fått på den kursen
samt det högsta och lägsta betyget som någon fått i kursen (ska lösas med SQL) */

/* Lägsta betyg */							/* Högsta betyg */  
SELECT MAX(Math.Grade) FROM Math;		SELECT MIN(Math.Grade) FROM Math;
SELECT MAX(English.Grade) FROM English;		SELECT MIN(English.Grade) FROM English;
SELECT MAX(Programming.Grade) FROM Programming;		SELECT MIN(Programming.Grade) FROM Programming;

/* För snittbetyget har jag omvandlat till siffra och sen körde en metod i programmet
för att ändra tillbaka till en bokstav*/

/* Males - Math*/
SELECT AVG(CASE
WHEN Grade = 'F' THEN 0 
WHEN Grade = 'E' THEN 1
WHEN Grade = 'D' THEN 2
WHEN Grade = 'C' THEN 3
WHEN Grade = 'B' THEN 4
WHEN Grade = 'A' THEN 5 
END) FROM Student, Math
WHERE SUBSTRING(Student.SSN, 9,1) % 2 != 0
AND Grade IS NOT NULL AND Math.StudentId = Student.id

/* Females - Math*/
SELECT AVG(CASE
WHEN Grade = 'F' THEN 0 
WHEN Grade = 'E' THEN 1
WHEN Grade = 'D' THEN 2
WHEN Grade = 'C' THEN 3
WHEN Grade = 'B' THEN 4
WHEN Grade = 'A' THEN 5 
END) FROM Student, Math
WHERE SUBSTRING(Student.SSN, 9,1) % 2 = 0
AND Grade IS NOT NULL AND Math.StudentId = Student.id


/*----------------*/


/* Males - English*/
SELECT AVG(CASE
WHEN Grade = 'F' THEN 0 
WHEN Grade = 'E' THEN 1
WHEN Grade = 'D' THEN 2
WHEN Grade = 'C' THEN 3
WHEN Grade = 'B' THEN 4
WHEN Grade = 'A' THEN 5 
END) FROM Student, English
WHERE SUBSTRING(Student.SSN, 9,1) % 2 != 0
AND Grade IS NOT NULL AND English.StudentId = Student.id

/* Females - English*/
SELECT AVG(CASE
WHEN Grade = 'F' THEN 0 
WHEN Grade = 'E' THEN 1
WHEN Grade = 'D' THEN 2
WHEN Grade = 'C' THEN 3
WHEN Grade = 'B' THEN 4
WHEN Grade = 'A' THEN 5 
END) FROM Student, English
WHERE SUBSTRING(Student.SSN, 9,1) % 2 = 0
AND Grade IS NOT NULL AND English.StudentId = Student.id


/*----------------*/


/* Males - Programming*/
SELECT AVG(CASE
WHEN Grade = 'F' THEN 0 
WHEN Grade = 'E' THEN 1
WHEN Grade = 'D' THEN 2
WHEN Grade = 'C' THEN 3
WHEN Grade = 'B' THEN 4
WHEN Grade = 'A' THEN 5 
END) FROM Student, Programming
WHERE SUBSTRING(Student.SSN, 9,1) % 2 != 0
AND Grade IS NOT NULL AND Programming.StudentId = Student.id

/* Females - Programming*/
SELECT AVG(CASE
WHEN Grade = 'F' THEN 0 
WHEN Grade = 'E' THEN 1
WHEN Grade = 'D' THEN 2
WHEN Grade = 'C' THEN 3
WHEN Grade = 'B' THEN 4
WHEN Grade = 'A' THEN 5 
END) FROM Student, Programming
WHERE SUBSTRING(Student.SSN, 9,1) % 2 = 0
AND Grade IS NOT NULL AND Programming.StudentId = Student.id


/* Lägg till en nya elever */
/* string addStudentQuery = $"INSERT INTO Student(FirstName, LastName, Email," +
            $" SSN, Class) VALUES ('{firstName}', '{lastName}', '{email}'," +
            $" '{ssn}', '{className}')";
     
            SqlCommand cmd = new SqlCommand(addStudentQuery, Connection);
            cmd.ExecuteReader(); */