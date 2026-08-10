using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels
{
    public class AnalyticsViewModel
    {
        public  int  TotalMembers { get; set; }
        public  int  ActiveMembers { get; set; }
        public  int  Trainers { get; set; }
        public  int  UPcomingSession { get; set; }
        public  int  OngoningSession { get; set; }
        public  int CompletedSession { get; set; }


    }
}
