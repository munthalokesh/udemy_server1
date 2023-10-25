using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace udemy_server.Models.Entities
{
    public class AllLearningPaths
    {
        public string _class { get; set; }
        public int id { get; set; }
        public string url { get; set; }

        public string title { get; set; }
        public DateTime created {  get; set; }
        public int estimated_content_length { get; set; }
        public int number_of_content_items { get; set; }
        public bool is_pro_path {  get; set; }

        public List<LearningSections> sections { get; set; }
    }
}