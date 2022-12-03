using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using allClassStoreNamespace;

namespace deleteStudentInfoNamespace
{
    internal class DeleteStudentInfo
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
        internal void deleteStudentInfo()
        {
            Console.Write("Enter student Id to delete student info : ");
            string? studentId = Console.ReadLine();

            var jsonStudent = File.ReadAllText(jsonFileStudent);
            var jsonSemester = File.ReadAllText(jsonFileSemester);
            var jsonCourse = File.ReadAllText(jsonFileCourse);

            bool chk = existOrNot(studentId);

            if (chk)
            {
                try
                {
                    StudentRoot teststudent = JsonConvert.DeserializeObject<StudentRoot>(jsonStudent)!;
                    SemesterRoot testsemester = JsonConvert.DeserializeObject<SemesterRoot>(jsonSemester)!;
                    CourseRoot testcourse = JsonConvert.DeserializeObject<CourseRoot>(jsonCourse)!;

                    var items = from cor in testcourse.courseroot // Using LINQ
                                join sem in testsemester.semesterroot! on cor.StudentId equals sem.StudentId
                                join stu in teststudent.studentroot! on sem.StudentId equals stu.StudentId
                                into value
                                from stu in value.DefaultIfEmpty()
                                where ((stu.StudentId == studentId))
                                orderby (sem.SemesterId)

                                select new
                                {
                                    StuId = stu.StudentId,
                                    SemeId = sem.SemesterId,
                                    CId = cor.CourseId,
                                };

                    try
                    {
                        // using polymorphism
                        DeleteSudentDetials delstu = new DeleteSudentDetials();
                        DeleteSudentDetials delsem = new DeleteSemesterDetails();
                        DeleteSudentDetials delcor = new DeleteCourseDetails();

                        int flag = 0;

                        foreach (var val in items)
                        {
                            delstu.deleteInfo(val.StuId);
                            delsem.deleteInfo(val.StuId);
                            delcor.deleteInfo(val.StuId);

                            flag = 1;
                        }

                        if (flag == 0)
                        {
                            delstu.deleteInfo(studentId);
                        }
                    }

                    catch (Exception ex)
                    {
                        Console.WriteLine("Deleting Error : " + ex.Message.ToString());
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Joining Error : " + ex.Message.ToString());
                }

                Console.WriteLine("Data deleted successfully.");
            }

            else
            {
                Console.WriteLine("Id not exist");
            }
        }

        // Delete Info From Student Json File
        public class DeleteSudentDetials  // Base class (parent) for polymorphism
        {
            public virtual void deleteInfo(string? Id)
            {
                try
                {
                    string jsonFileStudent = System.IO.Path.Combine(currentDirectory, "data", "student.json");
                    var jsonStudent = File.ReadAllText(jsonFileStudent);
                    var studentjObject = JObject.Parse(jsonStudent);
                    JArray studentArray = (JArray)studentjObject["studentroot"]!;
                    var studentDetails = studentArray.FirstOrDefault(obj => obj["StudentId"]!.Value<string>() == Id);

                    if (studentDetails != null)
                        studentArray.Remove(studentDetails);

                    string output = JsonConvert.SerializeObject(studentjObject, Formatting.Indented);
                    File.WriteAllText(jsonFileStudent, output);
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Deleting Student Error : " + ex.Message.ToString());
                }
            }
        }

        // Delete Info From Semester Json File
        public class DeleteSemesterDetails : DeleteSudentDetials  // Derived class (child) for polymorphism
        {
            public override void deleteInfo(string? Id)
            {
                try
                {
                    string jsonFileSemester = System.IO.Path.Combine(currentDirectory, "data", "semester.json");
                    var jsonSemester = File.ReadAllText(jsonFileSemester);
                    var semesterjObject = JObject.Parse(jsonSemester);
                    JArray semesterArray = (JArray)semesterjObject["semesterroot"]!;
                    var semesterDetails = semesterArray.FirstOrDefault(obj => obj["StudentId"]!.Value<string>() == Id);

                    if (semesterDetails != null)
                        semesterArray.Remove(semesterDetails);

                    string output1 = JsonConvert.SerializeObject(semesterjObject, Formatting.Indented);
                    File.WriteAllText(jsonFileSemester, output1);
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Deleting Semester Error : " + ex.Message.ToString());
                }
            }
        }

        // Delete Info From Courseinfo Json File
        public class DeleteCourseDetails : DeleteSudentDetials  // Derived class (child) for polymorphism
        {
            public override void deleteInfo(string? Id)
            {
                try
                {
                    string jsonFileCourse = System.IO.Path.Combine(currentDirectory, "data", "courseInfo.json");
                    var jsonCourse = File.ReadAllText(jsonFileCourse);
                    var coursejObject = JObject.Parse(jsonCourse);
                    JArray courseArray = (JArray)coursejObject["courseroot"]!;
                    var courseDetails = courseArray.FirstOrDefault(obj => obj["StudentId"]!.Value<string>() == Id);

                    if (courseDetails != null)
                        courseArray.Remove(courseDetails);

                    string output2 = JsonConvert.SerializeObject(coursejObject, Formatting.Indented);
                    File.WriteAllText(jsonFileCourse, output2);
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Deleting Course Error : " + ex.Message.ToString());
                }
            }
        }

        public bool existOrNot(string? str)
        {
            try
            {
                var jsonStudent = File.ReadAllText(jsonFileStudent);
                var studentjObject = JObject.Parse(jsonStudent);
                JArray studentArray = (JArray)studentjObject["studentroot"]!;
                var studentDetails = studentArray.FirstOrDefault(obj => obj["StudentId"]!.Value<string>() == str);

                if (studentDetails != null)
                    return true;

                else
                    return false;
            }

            catch (Exception)
            {
                Console.WriteLine("Finding student id error");
            }

            return false;
        }
    }
}