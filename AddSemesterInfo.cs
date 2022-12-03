using allClassStoreNamespace;
using setJsonFileNamespace;
using addCourseInfoNamespace;
using extensionMethodNamespace;
using Newtonsoft.Json.Linq;

namespace addSemesterInfoNamesapce
{
    internal class AddSemesterInfo
    {
        static string currentDirectory = Directory.GetCurrentDirectory();
        private string jsonFileSemester = System.IO.Path.Combine(currentDirectory, "data", "semester.json");
        private class SemesterRoot
        {
            public List<InheritSemester>? semesterroot { get; set; }
        }

        private class InheritSemester : Semester
        {
            //Semester inheriated from allclassstore
        }

        enum semesterCode
        {
            Summer = 1, Fall, Spring
        }

        internal void loadSemesterInfo(string? studentId)
        {
            Console.WriteLine("Enter Semester Code :- 1.Summer, 2.Fall, 3.Spring");
            string? semester = Enum.Parse(typeof(semesterCode), Console.ReadLine()!).ToString();

            string? year = "";
            while (true)
            {
                Console.Write("Enter Year In the Formate XXXX : ");
                string? yy = Console.ReadLine();
                bool chk = yy!.validYear(); // // Calling extension method for year validation
                if (chk) { year = yy; break; }
                else Console.WriteLine("Invaild formate, please enter again.");
            }

            string chkId = studentId + "@" + semester + "-" + year;

            var jsonSemester = File.ReadAllText(jsonFileSemester);
            var semesterjObject = JObject.Parse(jsonSemester);
            JArray semesterArray = (JArray)semesterjObject["semesterroot"]!;
            var semesterDetails = semesterArray.FirstOrDefault(obj => obj["SemesterId"]!.Value<string>() == chkId);

            if (semesterDetails == null)
            {
                var mySemesterRoot = new SemesterRoot
                {
                    semesterroot = new List<InheritSemester>
                    {
                    new InheritSemester
                    {
                        StudentId = studentId,
                        Code = semester,
                        Year = year,
                        SemesterId = studentId + "@" + semester + "-" + year
                    }
                   }
                };

                var mySemester = new InheritSemester
                {
                    StudentId = studentId,
                    Code = semester,
                    Year = year,
                    SemesterId = studentId + "@" + semester + "-" + year
                };

                SetJsonFile setJson = new SetJsonFile();
                setJson.setSemesterJsonFile(mySemesterRoot, mySemester);
                
                Console.WriteLine("\nData added successfully.");
            }

            else
            {
                Console.WriteLine("\nSemester already exist.");
            }

            AddCourseInfo courseInfo = new AddCourseInfo();
            courseInfo.loadCourse(studentId + "@" + semester + "-" + year, studentId);
        }
    }
}