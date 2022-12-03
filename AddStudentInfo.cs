using allClassStoreNamespace;
using setJsonFileNamespace;
using extensionMethodNamespace;
using Newtonsoft.Json.Linq;

namespace addStudentNamespace
{
    internal class AddStudentInfo
    {
        // Using events with delegates for showing success message
        private delegate void DelEventHandler();
        private static event DelEventHandler? message;
        static void successMsg()
        {
            Console.WriteLine("\nData added successfully.");
        }

        // Using custom attribute for clarification
        private class HelpAttribute : Attribute
        {
            public string? HelpText { get; set; }

        }
        private class StudentRoot
        {
            public List<InheritStudent>? studentroot { get; set; } // Using get/set properties
        }

        [Help(HelpText = "This is an inherited class")]
        private class InheritStudent : Student
        {
            //Student inherited from allclassstore
        }

        // Using enum type for setting department and degree
        enum departmentName
        {
            ComputerScience = 1, BBA, English
        }

        enum degreeName
        {
            BSC = 1, BBA, BA, MSC, MBA, MA
        }

        static string currentDirectory = Directory.GetCurrentDirectory();
        private string jsonFileStudent = System.IO.Path.Combine(currentDirectory, "data", "student.json");

        internal void addStudentData()
        {
            Console.Write("\nEnter FirstName :- ");
            string? firstName = Console.ReadLine();

            Console.Write("\nEnter MiddleName :- ");
            string? middleName = Console.ReadLine();

            Console.Write("\nEnter LastName :- ");
            string? lastName = Console.ReadLine();

            string? studentId = "";
            while (true)
            {
                Console.Write("\nEnter Student Id In the Formate XXX-XXX-XXX : ");
                string? Id = Console.ReadLine();
                bool chk = Id!.validId(); // Calling extension method for id validation
                if (chk) { studentId = Id; break; }
                else Console.WriteLine("\nInvaild id, please enter again.");
            }

            DateTime dt = DateTime.Now;
            var month = dt.Month;
            string? year = dt.Year.ToString();

            string? semester = "Spring";

            if (month >= 1 && month <= 4)
            {
                semester = "Summer";
            }

            else if (month >= 5 && month <= 8)
            {
                semester = "Fall";
            }

            Console.WriteLine("\nEnter Joining Batch :- " + semester + "--" + year);

            Console.WriteLine("\nEnter Department Name :- 1.ComputerScience, 2.BBA, 3.English");
            var department = Enum.Parse(typeof(departmentName), Console.ReadLine()!);

            Console.WriteLine("\nEnter Degree  Name :- 1.BSC, 2.BBA, 3.BA, 4.MSC, 5.MBA, 6.MA");
            var degree = Enum.Parse(typeof(degreeName), Console.ReadLine()!);

            var jsonStudent = File.ReadAllText(jsonFileStudent);
            var studentjObject = JObject.Parse(jsonStudent);
            JArray studentArray = (JArray)studentjObject["studentroot"]!;
            var studentDetails = studentArray.FirstOrDefault(obj => obj["StudentId"]!.Value<string>() == studentId); // Lambda expresion

            if (studentDetails == null)
            {
                // Adding Info into json file
                var myStudentRoot = new StudentRoot
                {
                    studentroot = new List<InheritStudent>
                    {
                    new InheritStudent
                    {
                        FirstName = firstName,
                        MiddleName = middleName,
                        LastName = lastName,
                        StudentId = studentId,
                        Semester = semester,
                        Year = year,
                        Department = department.ToString(),
                        Degree = degree.ToString(),
                    }
                   }
                };

                var myStudent = new InheritStudent
                {
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName,
                    StudentId = studentId,
                    Semester = semester,
                    Year = year,
                    Department = department.ToString(),
                    Degree = degree.ToString(),
                };

                SetJsonFile setJson = new SetJsonFile();

                setJson.setStudentJsonFile(myStudentRoot, myStudent);

                // Calling events using delegates
                message += new DelEventHandler(successMsg);
                message.Invoke();
            }

            else
            {
                Console.WriteLine("\nThis id is already exist.");
            }
        }
    }
}