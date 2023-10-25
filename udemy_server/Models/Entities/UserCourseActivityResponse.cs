using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace udemy_server.Models.Entities
{
    public class UserCourseActivityResponse
    {
       
            public List<UserCourseActivity> results { get; set; }
            public string next { get; set; }
            public int count { get; set; }
        
    }
}