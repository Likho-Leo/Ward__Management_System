using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WardDapperMVC.Model.Domain
{
    public class VisitSchedule
    {
        public int ScheduleID { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;
        public string? VisitType { get; set; }
        [ForeignKey("DoctorID"), DisplayName("Doctor")]
        public int DoctorID { get; set; }
        [ForeignKey("PatientID"), DisplayName("Patient")]
        public int PatientID { get; set; }
        public char InActive { get; set; }
        public DateTime FollowUpAppointmentDate { get; set; }
        public string? PatientFullName { get; set; }
        public string? DoctorFullName { get; set; }
    }
}
