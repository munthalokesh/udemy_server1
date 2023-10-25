using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace udemy_server.Models.Entities
{
    public class LearningSections
    {
        public int id { get; set; }
        public int order { get; set; }

        public List<LearningItems> items { get; set; }
    }
}