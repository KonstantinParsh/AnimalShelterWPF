using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4ParshArestGol.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Species { get; set; }
        public int Gender { get; set; }
        public string Breed { get; set; }
        public string Color { get; set; }
        public int AgeMonths { get; set; }
        public decimal Weight { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }
        public string Vaccinations { get; set; }

        public string GenderText => Gender == 0 ? "Самец" : "Самка";
    }
}
