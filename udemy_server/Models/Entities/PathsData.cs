using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace udemy_server.Models.Entities
{
    public class PathsData
    {
        
        public string user_email { get; set; }
        public string user_role { get; set; }
        public string user_external_id { get; set; }
        
        public int path_id { get; set; }
        public string path_title { get; set; }
        public bool is_pro_path { get; set; }
        public bool is_private_path { get; set; }
        public double? completion_ratio { get; set; }
        public DateTime? path_enrolled_date { get; set; }
        public double? path_consumed_minutes { get; set; }
        public DateTime? path_completion_date { get; set; }
        public bool is_path_assigned { get; set; }
        public DateTime? path_assigned_on { get; set; }
        public DateTime? path_assigned_due_date { get; set; }
        public  string  path_assigned_by { get; set; }
        public DateTime? path_first_activity_date { get; set; }
        public DateTime? path_last_activity_date { get; set; }
    }
}