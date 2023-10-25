using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace udemy_server.Models.Entities
{
    public class PathActivtyResponse
    {
        public List<PathsData> results { get; set; }
        public string next { get; set; }
        public int count { get; set; }
    }
}