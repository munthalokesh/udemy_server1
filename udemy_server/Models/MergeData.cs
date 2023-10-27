using Microsoft.Ajax.Utilities;
using Newtonsoft.Json; // Import Newtonsoft.Json namespace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using udemy_server.Controllers;
using udemy_server.Models.Entities;

namespace udemy_server.Models
{
    public class MergeData
    {
        public List<MergedData> MergeDat(List<UserActivity> userActivity, List<UserCourseActivity> userCourseActivity, List<PathsData> AllUsersPathsActivity, List<AllLearningPaths> learningPaths, List<EmpDetails> empDetails)
        {
            List<MergedData> mergedDatas = new List<MergedData>();

            // Read JSON data using Newtonsoft.Json
            /*string jsonString = System.IO.File.ReadAllText("C:\\Users\\lokesh.muntha\\source\\repos\\udemy_server\\udemy_server\\Models\\Entities\\json.json");
            List<EmpDetails> data = JsonConvert.DeserializeObject<List<EmpDetails>>(jsonString); // Deserialize using JsonConvert
*/
            for (int i = 0; i < empDetails.Count; i++)
            {
                MergedData mergedData = new MergedData();
                UserActivity user = userActivity.Where(u => string.Equals(u.user_email, empDetails[i].Email_Id, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                /*EmpDetails e = data.Where(u => string.Equals(u.Email_Id, userActivity[i].user_email, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (e != null)
                {
                    mergedData.EmpDetails = e;*/
                bool found = false;
                if (user == null)
                {
                    if (empDetails[i].LicenseType == "Enterprise" || empDetails[i].LicenseType == "Enterprise, Pro")
                    {
                        mergedData.EmpDetails = empDetails[i];
                        mergedData.Activity = null;
                        found=true;
                    }
                }
                else
                {
                    if (user.user_is_deactivated == false && (empDetails[i].LicenseType == "Enterprise" || empDetails[i].LicenseType == "Enterprise, Pro"))
                    {
                        mergedData.EmpDetails = empDetails[i];
                        mergedData.Activity = user;
                        found=true;
                    }
                }
                if(found)
                {
                    mergedData.LearningPaths = AllUsersPathsActivity.Where(u => string.Equals(u.user_email, empDetails[i].Email_Id, StringComparison.OrdinalIgnoreCase)).ToList();
                    Dictionary<int, string> pathAssignedByDictionary = AllUsersPathsActivity
                    .Where(u => string.Equals(u.user_email, empDetails[i].Email_Id, StringComparison.OrdinalIgnoreCase))
                    .ToDictionary(
                        u => u.path_id,
                        u => u.path_assigned_by != null ? u.path_assigned_by : null
                    );
                    Dictionary<int, List<int>> pathUrlsDictionary = new Dictionary<int, List<int>>();
                    /*Dictionary<int,thumbnail> Coursethumbnails=new Dictionary<int, thumbnail>();*/
                    List<UserCourseActivity> courses = userCourseActivity.Where(u => string.Equals(u.user_email, empDetails[i].Email_Id, StringComparison.OrdinalIgnoreCase)).ToList();
                    List<int> courseIDs = userCourseActivity.Where(u => string.Equals(u.user_email, empDetails[i].Email_Id, StringComparison.OrdinalIgnoreCase)).Select(u => u.course_id).ToList();
                    foreach (int key in pathAssignedByDictionary.Keys)
                    {
                        AllLearningPaths paths = learningPaths.Where(u => u.id == key).FirstOrDefault();
                        if (paths != null)
                        {

                            List<LearningSections> sections = paths.sections;
                            List<int> CourseIds = new List<int>();
                            if (sections != null)
                            {

                                foreach (var section in sections)
                                {
                                    List<LearningItems> items = section.items;
                                    if (items != null)
                                    {
                                        foreach (var item in items)
                                        {
                                            string url = item.resource_url;
                                            if (url != null)
                                            {
                                                string[] parts = url.Split('/');
                                                if (parts.Length > 0)
                                                {
                                                    int.TryParse(parts[parts.Length - 2], out int courseId);
                                                    if (!courseIDs.Contains(courseId))
                                                    {
                                                        UserCourseActivity c = new UserCourseActivity();
                                                        c.course_id = courseId;
                                                        /*c.user_id = paths.;*/
                                                        c.user_email = empDetails[i].Email_Id;
                                                        c.user_role = AllUsersPathsActivity.Where(u => string.Equals(u.user_email, empDetails[i].Email_Id, StringComparison.OrdinalIgnoreCase)).FirstOrDefault().user_role;
                                                        c.path_id = key;
                                                        c.course_title = item.title;
                                                        c.course_category = null;
                                                        c.course_duration = item.duration;
                                                        c.completion_ratio = 0;
                                                        c.num_video_consumed_minutes = 0;
                                                        c.course_enroll_date = null;
                                                        c.course_start_date = null;
                                                        c.course_completion_date = null;
                                                        c.course_first_completion_date = null;
                                                        c.course_last_accessed_date = null;
                                                        c.is_assigned = true;
                                                        c.assigned_by = pathAssignedByDictionary[key];
                                                        c.user_is_deactivated = false;
                                                        courses.Add(c);

                                                    }

                                                    CourseIds.Add(courseId);
                                                    /*if(!Coursethumbnails.ContainsKey(courseId))
                                                    {
                                                        Coursethumbnails[courseId] = item.thumbnail;
                                                    }*/
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            pathUrlsDictionary[paths.id] = CourseIds;
                        }
                    }


                    if (courses.Count > 0 && courses != null)
                    {
                        foreach (UserCourseActivity course in courses)
                        {
                            if (pathUrlsDictionary.Values.Any(courseIdsList => courseIdsList.Contains(course.course_id)))
                            {
                                int pathKey = pathUrlsDictionary.FirstOrDefault(kvp => kvp.Value.Contains(course.course_id)).Key;

                                if (pathAssignedByDictionary.ContainsKey(pathKey))
                                {

                                    course.is_assigned = true;
                                    course.assigned_by = string.Join(" ", pathAssignedByDictionary[pathKey]);
                                    course.path_id = pathKey;
                                    PathsData p=  AllUsersPathsActivity.Where(u => string.Equals(u.user_email, empDetails[i].Email_Id, StringComparison.OrdinalIgnoreCase) && u.path_id == pathKey).FirstOrDefault();
                                    if(p!=null)
                                    {
                                        course.assigned_on = p.path_assigned_on;
                                    }
                                }
                            }
                            /*if(Coursethumbnails.ContainsKey(course.course_id))
                            {
                                course.thumbnail = Coursethumbnails[course.course_id];
                            }*/
                        }
                    }
                    int currentYear = DateTime.Now.Year;
                    double totalDurationInCurrentYear = courses
                    .Where(course => course.is_assigned &&
                                     course.course_enroll_date.HasValue &&
                                     course.course_enroll_date.Value.Year == currentYear &&
                                     course.assigned_on.HasValue &&
                                     course.assigned_on.Value.Year == currentYear &&
                                     course.completion_ratio.HasValue &&
                                     course.completion_ratio.Value==100 &&
                                     /*mergedData.LearningPaths.Where(u=>u.path_id==course.path_id).FirstOrDefault().path_assigned_on.Value.Year==currentYear &&*/
                                     course.course_completion_date.HasValue &&
                                     course.course_completion_date.Value.Year == currentYear)
                    .Sum(course => course.num_video_consumed_minutes);
                    /*foreach (int key in pathAssignedByDictionary.Keys)
                    { 
                        if(pathUrlsDictionary.ContainsKey(key))
                        {
                            List<int> CourseIds = pathUrlsDictionary[key];
                            foreach (int courseId in CourseIds)
                            {
                                if(!courseIds.Contains(courseId))
                                {
                                    UserCourseActivity c = new UserCourseActivity();
                                    c.course_id = courseId;
                                    c.user_id = courses[0].user_id;
                                    c.user_email = courses[0].user_email;
                                    c.user_role = courses[0].user_role;
                                    c.path_id = key;
                                    c.course_title=learningPaths.;
                                    c.course_category= AllCoursesList.Where(u => u.id == courseId).FirstOrDefault().categories[0];
                                    c.course_duration = AllCoursesList.Where(u => u.id == courseId).FirstOrDefault().estimated_content_length;
                                    c.completion_ratio = 0;
                                    c.num_video_consumed_minutes = 0;
                                    c.course_enroll_date = null;
                                    c.course_start_date = null;
                                    c.course_completion_date = null;
                                    c.course_first_completion_date= null;
                                    c.course_last_accessed_date = null;
                                    c.is_assigned = true;
                                    c.assigned_by = pathAssignedByDictionary[key];
                                    c.user_is_deactivated = false;
                                    courses.Add(c);

                                }
                            }
                        }
                    }*/
                    mergedData.Courses = courses;
                    if (totalDurationInCurrentYear > 0)
                    {
                        mergedData.Activity.completion_per_fourty = totalDurationInCurrentYear / 60;
                    }

                    mergedDatas.Add(mergedData);
                }
                else
                {
                    mergedData.EmpDetails = empDetails[i];
                    mergedData.Activity = null;
                    mergedData.Courses = null;
                    mergedData.LearningPaths = null;
                    mergedDatas.Add(mergedData);
                }
                
            }
            return mergedDatas;
        }
    }
}
    

