using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WardDapperMVC.Models.Domain
{
    public class Prescription
    {
        public int PrescriptionID { get; set; }
        public string Script { get; set; }
        public DateTime   Date { get; set; }
        public string ProcessStatus { get; set; }
        public int PatientID { get; set; }
        public int DoctorID { get; set; }


        //This is for patient with Only Details that we want
        [ForeignKey("PatientId")]
        public int PatientId { get; set; } // Foreign key

        // Patient details
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PatientNumber { get; set; }

        public string? EmployeeNumber { get; set; }

        //This is for EMPLOYEE with Only Details that we want
        public string? DoctorFirstName { get; set; }
        public string? DoctorLastName { get; set; }

        public int UserID { get; set; } // Foreign key
    }
}