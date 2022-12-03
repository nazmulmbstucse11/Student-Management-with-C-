using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using allClassStoreNamespace;
using setJsonFileNamespace;

namespace addCourseInfoNamespace
{
    internal class AddCourseInfo
    {
        static string currentDirectory = Directory.GetCurrentDirectory();
        private string jsonFileFetch = System.IO.Path.Combine(currentDirectory, "data", "course.json");
        private string jsonFileCourse = System.IO.Path.Combine(currentDirectory, "data", "courseInfo.json");

        private class CourseRoot
        {
            public List<InheritCourse>? courseroot { get; set; }
        }

        private class InheritCourse : CourseInfo
        {
            // CourseInfo inheriated from allclassstore
        }
        internal void loadCourse(string? SemesterCodeId, string? studentId)
        {
            Console.Write("\nEnter the number of course in this semsester to be added :- ");
            int courseNumber = Convert.ToInt32(Console.ReadLine());

            try
            {
                var courseList = new Dictionary<int, InheritCourse>();

                // Fetching data from json file for course
                var json = File.ReadAllText(jsonFileFetch);
                var jObject = JObject.Parse(json);
                JArray courseArrary = (JArray)jObject["course"]!;

                if (courseArrary != null)
                {
                    Console.WriteLine("\nAvailable course for this student.");
                    Console.WriteLine("Course Serial" + "\t" + "Course Id" + "\t" + "Name" + "\t\t" + "Instructor" + "\t" + "Number of Credits");
                    Console.WriteLine("-------------------------------------------------------------------------------");
                    int i = 1;
                    int checker = 0;

                    // Checking availabe course
                    foreach (var item in courseArrary)
                    {
                        dynamic values = findValue<string>(studentId!);

                        int flag = 0;

                        foreach (var cs in values)
                        {
                            string? test1 = cs.c;
                            string? test2 = item["courseID"]!.ToString();

                            if (test1 == test2)
                            {
                                flag = 1;
                                break;
                            }
                        }

                        if (flag == 0)
                        {
                            Console.WriteLine(i + "-->\t\t" + item["courseID"] + "\t\t" + item["courseName"] + "\t\t" + item["instructorName"] + "\t\t" + item["numberOfCredits"]);

                            InheritCourse fetch = new InheritCourse();
                            fetch.CourseId = item["courseID"]!.ToString();
                            fetch.CourseName = item["courseName"]!.ToString();
                            fetch.InstructorName = item["instructorName"]!.ToString();
                            fetch.NumberOfCredits = Convert.ToInt32(item["numberOfCredits"]);
                            fetch.SemesterId = SemesterCodeId;
                            fetch.StudentId = studentId;
                            courseList[i] = fetch;
                            checker = 1;

                            i++;
                        }

                        flag = 0;
                    }

                    if (checker == 0)
                    {
                        Console.WriteLine("\nNo available course for this student.");
                    }
                }

                // Adding Info into json file

                for (int j = 1; j <= courseNumber; j++)
                {
                    Console.Write("\nEnter Serial Number To Add Particular Course :- ");
                    int serial = Convert.ToInt32(Console.ReadLine());

                    dynamic values = findValue<string>(studentId!);

                    int exist = 0;

                    foreach (var cs in values)
                    {
                        if (cs.c == courseList[serial].CourseId)
                        {
                            exist = 1;
                            break;
                        }
                    }

                    if (exist == 1)
                    {
                        Console.WriteLine("\nAlready added.");
                    }

                    else
                    {
                        var myCourseRoot = new CourseRoot
                        {
                            courseroot = new List<InheritCourse>
                                {
                                new InheritCourse
                                {
                                    CourseId = courseList[serial].CourseId,
                                    CourseName = courseList[serial].CourseName,
                                    InstructorName = courseList[serial].InstructorName,
                                    NumberOfCredits = courseList[serial].NumberOfCredits,
                                    SemesterId = courseList[serial].SemesterId,
                                    StudentId = courseList[serial].StudentId,
                                }
                                }
                        };

                        var myCourse = new InheritCourse
                        {
                            CourseId = courseList[serial].CourseId,
                            CourseName = courseList[serial].CourseName,
                            InstructorName = courseList[serial].InstructorName,
                            NumberOfCredits = courseList[serial].NumberOfCredits,
                            SemesterId = courseList[serial].SemesterId,
                            StudentId = courseList[serial].StudentId
                        };

                        SetJsonFile setJson = new SetJsonFile();
                        setJson.setCourseJsonFile(myCourseRoot, myCourse);

                        Console.WriteLine("Added successfully.");
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Course Adding Error : " + ex.Message.ToString());
            }
        }

        // Generic Method for searching added course
        public dynamic findValue<T>(T id)
        {
            string? searchId = id!.ToString();
            var jsonCourse = File.ReadAllText(jsonFileCourse);
            CourseRoot testcourse = JsonConvert.DeserializeObject<CourseRoot>(jsonCourse)!;

            dynamic values = from cor in testcourse.courseroot
                             where (cor.StudentId == searchId)
                             select new
                             {
                                 a = cor.StudentId,
                                 b = cor.SemesterId,
                                 c = cor.CourseId,
                             };
            return values;
        }
    }
}