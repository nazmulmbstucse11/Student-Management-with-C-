using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace setJsonFileNamespace
{
    internal class SetJsonFile
    {
        // Set student json file
        static string currentDirectory = Directory.GetCurrentDirectory();
        private string jsonFileStudent = System.IO.Path.Combine(currentDirectory, "data", "student.json");
        internal void setStudentJsonFile(object myStudentRoot, object myStudent)
        {
            if (new FileInfo(jsonFileStudent).Length == 0)
            {
                try
                {
                    var Json = JsonConvert.SerializeObject(myStudentRoot, Formatting.Indented);
                    File.WriteAllText(jsonFileStudent, Json);
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Student Adding Error : " + ex.Message.ToString());
                }
            }

            else
            {
                try
                {
                    var jsonRoot = File.ReadAllText(jsonFileStudent);
                    var jsonObj = JObject.Parse(jsonRoot);
                    var studentArray = jsonObj.GetValue("studentroot") as JArray;

                    var JsonNew = JsonConvert.SerializeObject(myStudent, Formatting.Indented);
                    var newStudent = JObject.Parse(JsonNew);

                    studentArray!.Add(newStudent);

                    jsonObj["studentroot"] = studentArray;
                    string newJsonResult = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                    File.WriteAllText(jsonFileStudent, newJsonResult);
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Student Adding Error : " + ex.Message.ToString());
                }
            }
        }

        // Set semester json file
        private string jsonFileSemester = System.IO.Path.Combine(currentDirectory, "data", "semester.json");
        internal void setSemesterJsonFile(object mySemesterRoot, object mySemester)
        {
            if (new FileInfo(jsonFileSemester).Length == 0)
            {
                try
                {
                    var Json = JsonConvert.SerializeObject(mySemesterRoot, Formatting.Indented);
                    File.WriteAllText(jsonFileSemester, Json);
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Semester Adding Error : " + ex.Message.ToString());
                }
            }

            else
            {
                try
                {
                    var jsonRoot = File.ReadAllText(jsonFileSemester);
                    var jsonObj = JObject.Parse(jsonRoot);
                    var semesterArray = jsonObj.GetValue("semesterroot") as JArray;

                    var JsonNew = JsonConvert.SerializeObject(mySemester, Formatting.Indented);
                    var newSemester = JObject.Parse(JsonNew);

                    semesterArray!.Add(newSemester);

                    jsonObj["semesterroot"] = semesterArray;
                    string newJsonResult = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                    File.WriteAllText(jsonFileSemester, newJsonResult);
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Semester Adding Error : " + ex.Message.ToString());
                }
            }
        }

        // Set course json file
        private string jsonFileCourse = System.IO.Path.Combine(currentDirectory, "data", "courseInfo.json");
        internal void setCourseJsonFile(object myCourseRoot, object myCourse)
        {
            if (new FileInfo(jsonFileCourse).Length == 0)
            {
                try
                {
                    var Json = JsonConvert.SerializeObject(myCourseRoot, Formatting.Indented);
                    File.WriteAllText(jsonFileCourse, Json);
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Course Adding Error : " + ex.Message.ToString());
                }
            }

            else
            {
                try
                {
                    var jsonRoot = File.ReadAllText(jsonFileCourse);
                    var jsonObj = JObject.Parse(jsonRoot);
                    var courseArray = jsonObj.GetValue("courseroot") as JArray;

                    var JsonNew = JsonConvert.SerializeObject(myCourse, Formatting.Indented);
                    var newCourse = JObject.Parse(JsonNew);

                    courseArray!.Add(newCourse);

                    jsonObj["courseroot"] = courseArray;
                    string newJsonResult = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                    File.WriteAllText(jsonFileCourse, newJsonResult);
                }

                catch (Exception ex)
                {
                    Console.WriteLine("Course Adding Error : " + ex.Message.ToString());
                }
            }
        }
    }
}