using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace udemy_server.Models.Entities
{
    public class MergedData
    {
        public EmpDetails EmpDetails { get; set; }
        public UserActivity Activity { get; set; }
         public List<UserCourseActivity> Courses { get; set; }

        public List<PathsData> LearningPaths { get; set; }

        
    }
}