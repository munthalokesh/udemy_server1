using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace udemy_server.Models.Entities
{
    public class AllCourses
    {
        public int id { get; set; }
        public string title { get; set; }
        public List<string> categories { get; set; }
        public int num_lectures { get; set; }
        public double estimated_content_length { get; set; }
    }
}