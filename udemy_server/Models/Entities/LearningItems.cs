using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace udemy_server.Models.Entities
{
    public class LearningItems
    {
        /*public thumbnail thumbnail { get; set; }*/
        public string title { get; set; }
        public double duration { get; set; }
        public string resource_url {  get; set; }
        
    }
}