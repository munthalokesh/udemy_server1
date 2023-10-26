using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace udemy_server.Models.Entities
{
    public class UserCourseActivity
    {
       /* public int user_id { get; set; }*/
        
        public string user_email { get; set; }
        public string user_role { get; set; }
       
        public int course_id { get; set; }
        /*public thumbnail thumbnail { get; set; }*/
        public int? path_id { get; set; } = null;
        public string course_title { get; set; }
        public string course_category { get; set; }
        public double course_duration { get; set; }
        public double? completion_ratio { get; set; }
        public double num_video_consumed_minutes { get; set; }
        public DateTime? course_enroll_date { get; set; }
        public DateTime? assigned_on { get; set; }
        public DateTime? course_start_date { get; set; }
        public DateTime? course_completion_date { get; set; }
        public DateTime? course_first_completion_date { get; set; }
        public DateTime? course_last_accessed_date { get; set; }
        public bool is_assigned { get; set; }
        public string assigned_by { get; set; }
        public bool user_is_deactivated { get; set; }

        
    }
    
}