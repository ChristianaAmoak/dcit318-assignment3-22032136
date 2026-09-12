public class Student
{
    public int Id { get; }
    public string FullName { get; }
    public int Score { get; }

    public Student(int id, string fullName, int score)
    {
        Id = id;
        FullName = fullName;
        Score = score;
    }

    public string GetGrade()
    {
        if (Score >= 80 && Score <= 100)
            return "A";

        if (Score >= 70)
            return "B";

        if (Score >= 60)
            return "C";

        if (Score >= 50)
            return "D";

        return "F";
    }
}

public class InvalidScoreFormatException : Exception
{
    public InvalidScoreFormatException(string message)
        : base(message)
    {
    }
}

public class MissingFieldException : Exception
{
    public MissingFieldException(string message)
        : base(message)
    {
    }
}

public class StudentResultProcessor
{
    public List<Student> ReadStudentsFromFile(string inputFilePath)
    {
        List<Student> students = new();

        using StreamReader reader = new(inputFilePath);

        string? line;

        while ((line = reader.ReadLine()) != null)
        {
            string[] parts = line.Split(',');

            if (parts.Length != 3)
            {
                throw new MissingFieldException(
                    $"Missing field in record: {line}"
                );
            }

            if (!int.TryParse(parts[0].Trim(), out int id))
            {
                throw new MissingFieldException(
                    $"Invalid student ID in record: {line}"
                );
            }

            string fullName = parts[1].Trim();

            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new MissingFieldException(
                    $"Student name is missing in record: {line}"
                );
            }

            if (!int.TryParse(parts[2].Trim(), out int score))
            {
                throw new InvalidScoreFormatException(
                    $"Invalid score format in record: {line}"
                );
            }

            students.Add(
                new Student(id, fullName, score)
            );
        }

        return students;
    }

    public void WriteReportToFile(
        List<Student> students,
        string outputFilePath
    )
    {
        using StreamWriter writer = new(outputFilePath);

        foreach (Student student in students)
        {
            writer.WriteLine(
                $"{student.FullName} (ID: {student.Id}): Score = {student.Score}, Grade = {student.GetGrade()}"
            );
        }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
       string inputFilePath = Path.Combine(
    "StudentGradingSystem",
    "students.txt"
);

string outputFilePath = Path.Combine(
    "StudentGradingSystem",
    "report.txt"
);

        StudentResultProcessor processor = new();

        try
        {
            List<Student> students =
                processor.ReadStudentsFromFile(inputFilePath);

            processor.WriteReportToFile(
                students,
                outputFilePath
            );

            Console.WriteLine(
                "Student report generated successfully."
            );
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine(
                $"File not found: {ex.Message}"
            );
        }
        catch (InvalidScoreFormatException ex)
        {
            Console.WriteLine(
                $"Invalid score: {ex.Message}"
            );
        }
        catch (MissingFieldException ex)
        {
            Console.WriteLine(
                $"Missing field: {ex.Message}"
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"Unexpected error: {ex.Message}"
            );
        }
    }
}
