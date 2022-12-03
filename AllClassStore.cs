namespace allClassStoreNamespace
{
    public class Student
    {
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? StudentId { get; set; }
        public string? Semester { get; set; }
        public string? Year { get; set; }
        public string? Department { get; set; }
        public string? Degree { get; set; }
    }

    public class Semester
    {
        public string? StudentId { get; set; }
        public string? Code { get; set; }
        public string? Year { get; set; }
        public string? SemesterId { get; set; }
    }

    public class CourseInfo
    {
        public string? CourseId { get; set; }
        public string? CourseName { get; set; }
        public string? InstructorName { get; set; }
        public int NumberOfCredits { get; set; }
        public string? SemesterId { get; set; }
        public string? StudentId { get; set; }
    }
}