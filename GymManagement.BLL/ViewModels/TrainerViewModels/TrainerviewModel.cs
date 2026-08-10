using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.TrainerViewModels
{
    public class TrainerviewModel
    {

        public  int Id{ get; set; }

        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public string Specialties { get; set; } = default!;

        // Trainer Details
        public string? Address { get; set; } = default!;
        public string? DateOfBirth{ get; set; } = default!;


    }
}
