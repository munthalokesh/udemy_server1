using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace udemy_server.Models.Entities
{
    public class UserActivity
    {
       
        public string user_email { get; set; }
        public string user_role { get; set; }
        public string user_joined_date { get; set; }
       
        public bool user_is_deactivated { get; set; }
        public double? num_new_enrolled_courses { get; set; }
        public double? num_new_assigned_courses { get; set; }
        public double? num_new_started_courses { get; set; }
        public double? num_completed_courses { get; set; }
        public double? num_completed_lectures { get; set; }
        public double? num_completed_quizzes { get; set; }
        public double? num_video_consumed_minutes { get; set; }
        public double? num_web_visited_days { get; set; }
        public string last_date_visit { get; set; }
        public double completion_per_fourty { get; set; } = 0;
    }
    public class UserActivityResponse
    {
        public List<UserActivity> results { get; set; }
        public string next { get; set; }
        public int count { get; set; }
    }
}