using Newtonsoft.Json.Linq;
using addStudentNamespace;
using showStudentDetailsNamesapce;
using deleteStudentInfoNamespace;

namespace mainProgram
{
    internal class Program
    {
        internal delegate void ScreenShow(string? value);
        static void Main(string[] args)
        {
            AddStudentInfo add = new AddStudentInfo();
            ShowStudentDetails details = new ShowStudentDetails();
            DeleteStudentInfo delete = new DeleteStudentInfo();

            // Showing info of existing students using anonymous method 

            ScreenShow screenDetails = delegate (string? jsonFile)
            {
                try
                {
                    var jsonStudent = File.ReadAllText(jsonFile!);
                    var jObject = JObject.Parse(jsonStudent);
                    JArray studentArray = (JArray)jObject["studentroot"]!;

                    Console.WriteLine("\nStudent Id\t\tStudent Name\t\t\tDepartment");
                    Console.WriteLine("-----------------------------------------------------------------------");

                    int flag = 0;

                    if (studentArray != null)
                    {
                        foreach (var item in studentArray)
                        {
                            Console.WriteLine(item["StudentId"] + "\t\t" + item["FirstName"] + " " + item["MiddleName"] + " " + item["LastName"] + "\t\t" + item["Department"]);
                            flag = 1;
                        }
                    }

                    else
                    {
                        Console.WriteLine("\nNo data has been added.");
                    }

                    if (flag == 0)
                    {
                        Console.WriteLine("\nNo data has been added.");
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Showing Error : " + ex.Message.ToString());
                }
            };

            while (true)
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                string jsonFileStudent = System.IO.Path.Combine(currentDirectory, "data", "student.json");

                screenDetails(jsonFileStudent);

                Console.WriteLine("\nChoose Your Options : 1-Add New Student, 2-View Student Details, 3-Delete Student\n");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        add.addStudentData();
                        break;

                    case "2":
                        details.getStudentDetails();
                        break;

                    case "3":
                        delete.deleteStudentInfo();
                        break;

                    default:
                        Main(null!);
                        break;
                }
            }
        }
    }
}
