using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace udemy_server.Models.Entities
{
    public class AllLearningPathsResponse
    {
        
        public int count { get; set; }
        public string next { get; set; }
        public string? previous { get; set; }

        public List<AllLearningPaths> results { get; set; }
    }
}