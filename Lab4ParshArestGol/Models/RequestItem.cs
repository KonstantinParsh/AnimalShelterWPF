using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4ParshArestGol.Models
{
    public class RequestItem
    {
        public int RequestId { get; set; }
        public int UserId { get; set; }
        public string ClientName { get; set; }
        public int AnimalId { get; set; }
        public string AnimalName { get; set; }
        public string AnimalType { get; set; }
        public string AnimalBreed { get; set; }
        public string RequestDate { get; set; }
        public string RequestType { get; set; }
        public string Status { get; set; }
    }
}
