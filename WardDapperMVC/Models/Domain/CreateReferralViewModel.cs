using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WardDapperMVC.Models.Domain
{
    public class CreateReferralViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Date { get; set; } // Ensure this is DateTime
        public int DoctorID { get; set; }
        public string Status { get; set; }
    }

}
