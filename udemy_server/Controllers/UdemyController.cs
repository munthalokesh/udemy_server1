using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Http;
using udemy_server.Models;
using udemy_server.Models.Entities;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Web.Http.Cors;
using System.Web.UI.WebControls;

namespace udemy_server.Controllers
{
   
    public class UdemyController : ApiController
    {
        // GET api/<controller>
        private readonly HttpClient _httpClient;
        private System.Timers.Timer timer;
        private static DateTime lastRefreshTime;
        private static List<MergedData> mergedData;
        private static bool isDataRefreshed = false;
        public static List<EmpDetails> empDetails;
        public static List<UserActivity> AllUsersActivity;
        public static List<UserCourseActivity> AllUsersCoursesActivities;
        public static List<PathsData> AllUsersPathsActivities;
        public static List<AllLearningPaths> AllearningPaths;
        public UdemyController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://techwavelearninghub.udemy.com"); 
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            /*if (!isDataRefreshed)
            {
                
                var timer = new System.Threading.Timer(
                    async _ =>
                    {
                        await RefreshData();
                    },
                    null,
                    TimeSpan.FromMinutes(0), // Initial delay
                    TimeSpan.FromMinutes(10)   // Interval between subsequent invocations
                );

                isDataRefreshed = true;
            }*/
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "dDdxRmJQaUlFN3N6MVd1TGs2NDdLUzRBcmVYRHZxSXdTMTBYeWdmaDpRd1diN3d3Y29ST1IyRWsza0RuMEQ3RWdRdE43TFE2ODhlNlE2eHhLdnkzZUpBRjg5NGdlOThrWDlIY1JFOFpKWmVkQmsyVk9ibFVYNThHREVOdVN2bFBYbXA4QUpzeWRBSUo3ejdBRWF4UGVLeUZNczVobEgyY3dOeHJjMHQ2RQ==");
            /*timer = new System.Timers.Timer(600000); // 8 hours in milliseconds
            timer.Elapsed += OnTimerElapsed;
            timer.Start();*/
        }
        /*private async void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {



            List<UserActivity> AllUserActivity = await GetUserActivity();
            List<UserCourseActivity> AllUsersCoursesActivity = await GetUserCourseActivity();
            List<PathsData> AllUsersPathsActivity = await GetPathActivity();
            List<AllLearningPaths> learningPaths=await GetAllLearningPaths();
            MergeData m = new MergeData();
            List<MergedData> mergedData = m.MergeDat(AllUserActivity, AllUsersCoursesActivity, AllUsersPathsActivity,learningPaths);

            string jsonData = JsonConvert.SerializeObject(mergedData, Formatting.Indented);
            // Store the data in a JSON file
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "merged_data.json");
            File.WriteAllText(filePath, jsonData);
        }*/

        /*[HttpGet]
        [Route("api/Udemy/GetUserActivity")]*/
        public async Task<List<UserActivity>> GetUserActivity()
        {
            try
            {
                 List<UserActivity> AllUserActivity = new List<UserActivity>();
                HttpResponseMessage response = await _httpClient.GetAsync("/api-2.0/organizations/239630/analytics/user-activity/");
                
                
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<UserActivityResponse>(jsonResponse);
                if (data != null && data.results != null)
                {
                    AllUserActivity.AddRange(data.results.Where(u=>u.user_email!= "Anonymized User"));
                }
                while (!string.IsNullOrEmpty(data.next))
                {
                     response = await _httpClient.GetAsync(data.next.Replace("https://techwavelearninghub.udemy.com","")); 
                   
                    while (response.StatusCode == (HttpStatusCode)429) // 429 status code
                    {
                        // You can implement a retry logic here
                        // Wait for a while and then retry the same request
                        await Task.Delay(2000); // Wait for a minute (you can adjust the delay as needed)

                        // Then retry the same request
                        response = await _httpClient.GetAsync(data.next.Replace("https://techwavelearninghub.udemy.com", ""));
                    }

                    jsonResponse = await response.Content.ReadAsStringAsync();
                   data = JsonConvert.DeserializeObject<UserActivityResponse>(jsonResponse);
                    if (data != null && data.results != null)
                    {
                        AllUserActivity.AddRange(data.results.Where(u => u.user_email != "Anonymized User"));
                    }
                }
                return AllUserActivity;
                /*return Ok(AllUserActivity);*/
            }
            catch (HttpRequestException ex)
            {
                throw ex;
                return null;
               /* return BadRequest(ex.Message);*/
            }
        }
        
        /*[Route("api/Udemy/GetUserCourseActivity")]*/
        public async Task<List<UserCourseActivity>> GetUserCourseActivity()
        {
            try
            
            {
                List<UserCourseActivity> AllUsersCoursesActivity = new List<UserCourseActivity>();
                HttpResponseMessage response = await _httpClient.GetAsync("/api-2.0/organizations/239630/analytics/user-course-activity/"); 
                

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<UserCourseActivityResponse>(jsonResponse);
                if (data != null && data.results != null)
                {
                    AllUsersCoursesActivity.AddRange(data.results.Where(u => u.user_email != "Anonymized User"));
                }
                while (!string.IsNullOrEmpty(data.next))
                {
                    response = await _httpClient.GetAsync(data.next.Replace("https://techwavelearninghub.udemy.com", "")); 
                    
                    while (response.StatusCode == (HttpStatusCode)429) // 429 status code
                    {
                        
                        await Task.Delay(2000); 

                        response = await _httpClient.GetAsync(data.next.Replace("https://techwavelearninghub.udemy.com", ""));
                    }

                    jsonResponse = await response.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<UserCourseActivityResponse>(jsonResponse);
                    if (data != null && data.results != null)
                    {
                        AllUsersCoursesActivity.AddRange(data.results.Where(u => u.user_email != "Anonymized User"));
                    }
                }
                return AllUsersCoursesActivity;
                /*return Ok(AllUsersCoursesActivity);*/
            }
            catch (HttpRequestException ex)
            {
                return null;
                /*return BadRequest(ex.Message);*/
            }
        }
        /*[Route("api/Udemy/GetLp")]*/
        public async Task<List<AllLearningPaths>> GetAllLearningPaths()
        {
            try
            
            {
                List<AllLearningPaths> LearningPaths = new List<AllLearningPaths>();
                HttpResponseMessage response = await _httpClient.GetAsync("/api-2.0/organizations/239630/learning-paths/list/"); 
                

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<AllLearningPathsResponse>(jsonResponse);
                if (data != null && data.results != null)
                {
                    LearningPaths.AddRange(data.results);
                }
                while (!string.IsNullOrEmpty(data.next))
                {
                    await Task.Delay(3000);     
                    response = await _httpClient.GetAsync(data.next.Replace("https://techwavelearninghub.udemy.com", "")); 
                    while (response.StatusCode == (HttpStatusCode)429) // 429 status code
                    {
                        
                        await Task.Delay(2000);
                        response = await _httpClient.GetAsync(data.next.Replace("https://techwavelearninghub.udemy.com", ""));
                    }

                    jsonResponse = await response.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<AllLearningPathsResponse>(jsonResponse);
                    if (data != null && data.results != null)
                    {
                        LearningPaths.AddRange(data.results);
                    }
                }
                return LearningPaths;
                /*return Ok(AllUsersCoursesActivity);*/
            }
            catch (HttpRequestException ex)
            {
                throw ex;
                return null;
                /*return BadRequest(ex.Message);*/
            }
            
        }

       /* public async Task<List<AllCourses>> GetAllCourses()
        {
            try

            {
                List<AllCourses> AllCoursesList = new List<AllCourses>();
                HttpResponseMessage response = await _httpClient.GetAsync("/api-2.0/organizations/239630/courses/list/");


                string jsonResponse = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<AllCoursesResponse>(jsonResponse);
                if (data != null && data.results != null)
                {
                    AllCoursesList.AddRange(data.results);
                }
                while (!string.IsNullOrEmpty(data.next))
                {
                    await Task.Delay(3000);
                    response = await _httpClient.GetAsync(data.next.Replace("https://techwavelearninghub.udemy.com", ""));
                    while (response.StatusCode == (HttpStatusCode)429) // 429 status code
                    {
                        await Task.Delay(2000);
                        response = await _httpClient.GetAsync(data.next.Replace("https://techwavelearninghub.udemy.com", ""));
                    }

                    jsonResponse = await response.Content.ReadAsStringAsync();
                    try
                    {
                        data = JsonConvert.DeserializeObject<AllCoursesResponse>(jsonResponse);
                    }
                    catch(Exception ex){
                        throw ex;
                    }
                    if (data != null && data.results != null)
                    {
                        AllCoursesList.AddRange(data.results);
                    }
                }
                return AllCoursesList;
                *//*return Ok(AllUsersCoursesActivity);*//*
            }
            catch (HttpRequestException ex)
            {
                throw ex;
                return null;
                *//*return BadRequest(ex.Message);*//*
            }

        }*/

        public async Task<List<PathsData>> GetPathActivity()
        {
            try
            {
                List<PathsData> AllUsersPathsActivity = new List<PathsData>();
                HttpResponseMessage response = await _httpClient.GetAsync("/api-2.0/organizations/239630/analytics/user-path-activity/");
                

                string jsonResponse = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<PathActivtyResponse>(jsonResponse);
                if (data != null && data.results != null)
                {
                    AllUsersPathsActivity.AddRange(data.results.Where(u => u.user_email != "Anonymized User"));
                }
                while (!string.IsNullOrEmpty(data.next))
                {
                    response = await _httpClient.GetAsync(data.next.Replace("https://techwavelearninghub.udemy.com", ""));
                    while (response.StatusCode == (HttpStatusCode)429) // 429 status code
                    {
                        
                        await Task.Delay(2000); 
                        response = await _httpClient.GetAsync(data.next.Replace("https://techwavelearninghub.udemy.com", ""));
                    }

                    jsonResponse = await response.Content.ReadAsStringAsync();
                    data = JsonConvert.DeserializeObject<PathActivtyResponse>(jsonResponse);
                    if (data != null && data.results != null)
                    {
                        AllUsersPathsActivity.AddRange(data.results.Where(u => u.user_email != "Anonymized User"));
                    }
                }
                return AllUsersPathsActivity;
                /*return Ok(AllUsersCoursesActivity);*/
            }
            catch (HttpRequestException ex)
            {
                return null;
                /*return BadRequest(ex.Message);*/
            }
        }
        [Route("api/Udemy/GetAll")] 
        /*public async Task<IHttpActionResult> GetAllData()
        {
            DateTime currentTime = DateTime.Now;
            TimeSpan timeSinceLastRefresh = currentTime - lastRefreshTime;
            if (timeSinceLastRefresh.TotalMinutes > 60 || mergedData == null)
            {
                List<UserActivity> AllUserActivity = await GetUserActivity();
                await Task.Delay(3000);
                List<UserCourseActivity> AllUsersCoursesActivity = await GetUserCourseActivity();
                await Task.Delay(3000);
                List<PathsData> AllUsersPathsActivity = await GetPathActivity();
                await Task.Delay(3000);
                List<AllLearningPaths> learningPaths = await GetAllLearningPaths();

                MergeData m = new MergeData();
                mergedData = m.MergeDat(AllUserActivity, AllUsersCoursesActivity, AllUsersPathsActivity, learningPaths);
                lastRefreshTime = DateTime.Now;
            }
            
            var newResult = new
            {
                results = mergedData, // The existing data under the "results" key
                time = lastRefreshTime.ToString("yyyy-MM-dd HH:mm:ss") // Add the timestamp under the "time" key
            };
            return Ok(newResult);
            *//*string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "merged_data.json");
            string jsonData = File.ReadAllText(filePath);
            dynamic responseData = JsonConvert.DeserializeObject(jsonData);
            return Ok(responseData);*//*
        }*/
        public async Task<IHttpActionResult> GetAllData()
        {
            // Use the cached data if available
            if (AllUsersActivity != null && AllUsersCoursesActivities != null && AllUsersPathsActivities != null && AllUsersPathsActivities.Count > 0 && AllearningPaths != null && AllearningPaths.Count > 0 && empDetails != null && empDetails.Count > 0 && AllUsersActivity.Count > 0 && AllUsersCoursesActivities.Count > 0)
            {
                MergeData m = new MergeData();
                mergedData = m.MergeDat(AllUsersActivity, AllUsersCoursesActivities, AllUsersPathsActivities, AllearningPaths, empDetails);
            }
            if (mergedData != null)
            {
                var newresult = new
                {
                    results = mergedData,
                    time = new { last_updated = lastRefreshTime.ToString("yyyy-MM-dd HH:mm:ss") }
                };
                return Ok(newresult);
            }

            return BadRequest("Data is not available yet. Please wait for the initial refresh.");
        }
        public async Task RefreshData()
        {
            DateTime currentTime = DateTime.Now;
            AllUsersActivity = await GetUserActivity();
            await Task.Delay(3000);
            AllUsersCoursesActivities = await GetUserCourseActivity();
            await Task.Delay(3000);
            AllUsersPathsActivities = await GetPathActivity();
            await Task.Delay(3000);
            AllearningPaths = await GetAllLearningPaths();
            /*await Task.Delay(3000);
            List<AllCourses> AllCoursesList=await GetAllCourses();*/

            lastRefreshTime = currentTime;



        }


        // POST api/<controller>
        [Route("api/Udemy/PostEmps")]
        public void Post([FromBody] List<EmpDetails> data)
        {
            if (data != null)
            {
                empDetails = data;
            }

        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}