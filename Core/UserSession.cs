using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Lab4ParshArestGol.Core
{
    public static class UserSession
    {
        public static Frame MainFrame { get; set; }
        public static int CurrentUserId { get; set; }
        public static string CurrentUserFullName { get; set; }
        public static int CurrentRoleId { get; set; }

        public static bool IsAuthorized()
        {
            return CurrentUserId > 0;
        }

        public static void Logout()
        {
            CurrentUserId = 0;
            CurrentUserFullName = null;
            CurrentRoleId = 0;
        }
    }
}
