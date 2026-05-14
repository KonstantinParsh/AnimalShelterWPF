using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4ParshArestGol.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public int RoleId {  get; set; }
        public string RoleName
        {
            get
            {
                switch (RoleId)
                {
                    case 1: return "Администратор";
                    case 2: return "Клиент";
                    case 3: return "Волонтер";
                    case 4: return "Врач";
                    default: return "Гость";
                }
            }
        }
    }
}
