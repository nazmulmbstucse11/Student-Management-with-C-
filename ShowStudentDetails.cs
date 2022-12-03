using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using allClassStoreNamespace;
using addSemesterInfoNamesapce;

namespace showStudentDetailsNamesapce
{
    internal class ShowStudentDetails
    {
        static string currentDirectory = Directory.GetCurrentDirectory();
        private string jsonFileStudent = System.IO.Path.Combine(currentDirectory, "data", "student.json");
        private string jsonFileSemester = System.IO.Path.Combine(currentDirectory, "data", "semester.json");
        private string jsonFileCourse = System.IO.Path.Combine(currentDirectory, "data", "courseInfo.json");

        private class StudentRoot
        {
            public List<InheritStudent>? studentroot { get; set; }
        }
        private class InheritStudent : Student
        {
            //Student inheriated from allclassstore
        }

        private class SemesterRoot
        {
            public List<InheritSemester>? semesterroot { get; set; }
        }

        private class InheritSemester : Semester
        {
            //Semester inheriated from allclassstore
        }

        private class CourseRoot
        {
            public List<InheritCourseInfo>? courseroot { get; set; }
        }

        private class InheritCourseInfo : CourseInfo
        {
            //CourseInfo inheriated from allclassstore
        }

        internal void getStudentDetails()
        {
            Console.Write("Enter Student Id to Show Details : ");
            string? studentId = Console.ReadLine(); // Using nullable type

            var jsonStudent = File.ReadAllText(jsonFileStudent);
            var jsonSemester = File.ReadAllText(jsonFileSemester);
            var jsonCourse = File.ReadAllText(jsonFileCourse);

            int flag = 0;

            try // fetch student info
            {
                var jObject = JObject.Parse(jsonStudent);
                JArray studentArray = (JArray)jObject["studentroot"]!;
                var studentDetails = studentArray.FirstOrDefault(obj => obj["StudentId"]!.Value<string>() == studentId); // Using Lambda expression

                if (studentDetails != null)
                {
                    var studentInfo = JsonConvert.SerializeObject(studentDetails, Formatting.Indented);

                    dynamic data = JObject.Parse(studentInfo);
                    string[] infoArray = { data.FirstName, data.MiddleName, data.LastName, data.StudentId, data.Semester, data.Year, data.Department, data.Degree };

                    // Call Using Interface
                    StudentInfoInterface studentinterface;
                    studentinterface = new StudentInfoDetails();
                    studentinterface.showStudentInfo(infoArray);

                    flag = 1;
                }

                else
                {
                    Console.WriteLine("\nStudent Id not exist.");
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Finding Error : " + ex.Message.ToString());
            }

            try // fetch semester and course info
            {
                StudentRoot teststudent = JsonConvert.DeserializeObject<StudentRoot>(jsonStudent)!;
                SemesterRoot testsemester = JsonConvert.DeserializeObject<SemesterRoot>(jsonSemester)!;
                CourseRoot testcourse = JsonConvert.DeserializeObject<CourseRoot>(jsonCourse)!;

                // joing for semester and course info using LINQ
                dynamic items = from cor in testcourse.courseroot
                                join sem in testsemester.semesterroot! on cor.SemesterId equals sem.SemesterId
                                join stu in teststudent.studentroot! on sem.StudentId equals stu.StudentId
                                into value
                                from stu in value.DefaultIfEmpty()
                                where ((stu.StudentId == studentId))
                                orderby (sem.SemesterId)

                                select new
                                {
                                    StuId = stu.StudentId,

                                    SemeId = sem.SemesterId,
                                    SemeCode = sem.Code,
                                    SemeYear = sem.Year,

                                    CId = cor.CourseId,
                                    CName = cor.CourseName,
                                    CInsName = cor.InstructorName,
                                    CCredit = cor.NumberOfCredits
                                };

                // Call Using Interface
                StudentInfoInterface studentinterface;
                studentinterface = new SemesterAndCourseDetails();
                studentinterface.showSemesterAndCourseInfo(items);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Main Joining Error : " + ex.Message.ToString());
            }

            if (flag == 1)
            {
                AddSemesterInfo seminfo = new AddSemesterInfo();

                Console.WriteLine("\nChoose Your Options : 1-Add New Semester, 2-Return Main Menu\n");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        seminfo.loadSemesterInfo(studentId);
                        break;

                    default:
                        break;
                }
            }
        }

        // Show Student Info Using Interface
        private interface StudentInfoInterface
        {
            void showStudentInfo(params string[] info);
            void showSemesterAndCourseInfo(dynamic fact);
        }

        private class StudentInfoDetails : StudentInfoInterface
        {
            public void showStudentInfo(params string[] info) // Using Param Arrays
            {
                Console.WriteLine("\nStudent Details:-");
                Console.WriteLine("-------------------------");
                Console.WriteLine("First Name:- " + info[0]);
                Console.WriteLine("Middle Name:- " + info[1]);
                Console.WriteLine("Last Name:- " + info[2]);
                Console.WriteLine("Student Id:- " + info[3]);
                Console.WriteLine("Joining Batch:- " + info[4] + "--" + info[5]);
                Console.WriteLine("Department:- " + info[6]);
                Console.WriteLine("Degree:- " + info[7]);
            }

            public void showSemesterAndCourseInfo(dynamic fact) { }
        }

        private class SemesterAndCourseDetails : StudentInfoInterface
        {
            public void showStudentInfo(params string[] info) { }
            public void showSemesterAndCourseInfo(dynamic fact) // Using dynamic type
            {
                string? str = "";
                int flag = 0;

                foreach (var val in fact)
                {
                    if (val.SemeId != str)
                    {
                        Console.WriteLine("\nSemesterCode:- " + val.SemeCode + "\t\tYear:- " + val.SemeYear);
                        Console.WriteLine("----------------------------------------------------------------------------");
                    }

                    Console.WriteLine("Course Id:- " + val.CId + "   Course Name:- " + val.CName + "     Instructor Name:- " + val.CInsName + "    Credits:- " + val.CCredit);

                    str = val.SemeId;
                    flag = 1;
                }

                if (flag == 0)
                {
                    Console.WriteLine("\nSemester Info is Empty.");
                }
            }
        }
    }
}