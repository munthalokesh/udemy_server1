using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace udemy_server.Models.Entities
{
    public class EmpDetails
    {
        public string ID { get; set; }
        public string Name { get; set; }

        public string Email_Id { get; set; }
        public string BU {  get; set; }
        public string Region { get; set; }
        public string Band { get; set; }
        public DateTime DOJ { get; set; }
        [JsonProperty("License Type")]
        public string LicenseType { get; set; }

    }
}