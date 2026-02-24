using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WardDapperMVC.Models.Domain
{
    public class CreateScript
    {
        public int PrescriptionID { get; set; }
        public string Script { get; set; }
        public DateTime Date { get; set; }
        public string ProcessStatus { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }
        
    }
}
