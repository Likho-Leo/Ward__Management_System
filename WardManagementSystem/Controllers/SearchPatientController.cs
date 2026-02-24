using Dapper;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using iText.Kernel.Colors;
using System.Data;
using System.Data.SqlClient;
using WardDapperMVC.Models.Domain;

namespace WardManagementSystem.Controllers
{
    public class SearchPatientController : Controller
    {
        private readonly IDbConnection _db;

        public SearchPatientController(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("conn"));
        }

        public IActionResult Search()
        {
            var viewModel = new PatientSearchViewModel
            {
                HospitalInformation = GetHospitalInfo() // Fetch hospital info
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Search(PatientSearchViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sql = @"SELECT pf.Status,pf.AdmitDate,p.FirstName,p.LastName,pf.FolderID,w.WardName,b.BedNo
                           FROM PatientFolder AS pf
                           INNER JOIN Patient AS p ON pf.PatientId=p.PatientId
                           INNER JOIN Ward AS w ON pf.WardId=w.WardID
                           INNER JOIN Bed AS b ON pf.BedID=b.BedID 
                           WHERE AdmitDate BETWEEN @StartDate AND @EndDate";

                model.Patients = _db.Query<PatientFolder>(sql, new { model.StartDate, model.EndDate }).ToList();
            }

            model.HospitalInformation = GetHospitalInfo(); // Fetch hospital info again
            return View(model);
        }

        private HospitalInformation GetHospitalInfo()
        {
            var sql = "SELECT * FROM HospitalInfo"; // Adjust table name as necessary
            return _db.QueryFirstOrDefault<HospitalInformation>(sql);
        }

        public IActionResult GeneratePdf(DateTime startDate, DateTime endDate)
        {
            var sqlPatients = @"SELECT pf.Status, pf.AdmitDate, p.FirstName, p.LastName, pf.FolderID, w.WardName, b.BedNo
                        FROM PatientFolder AS pf
                        INNER JOIN Patient AS p ON pf.PatientId = p.PatientId
                        INNER JOIN Ward AS w ON pf.WardId = w.WardID
                        INNER JOIN Bed AS b ON pf.BedID = b.BedID 
                        WHERE AdmitDate BETWEEN @StartDate AND @EndDate";

            var patients = _db.Query<PatientFolder>(sqlPatients, new { StartDate = startDate, EndDate = endDate }).ToList();
            var hospitalInfo = GetHospitalInfo(); // Get hospital information

            using var stream = new MemoryStream();
            using (var writer = new PdfWriter(stream))
            {
                using var pdf = new PdfDocument(writer);
                var document = new Document(pdf);

                // Add Hospital Information
                document.Add(new Paragraph(hospitalInfo.HospitalName)
                    .SetFontSize(20)
                    .SetBold()
                    .SetFontColor(ColorConstants.BLUE));
                document.Add(new Paragraph($"Phone: {hospitalInfo.TellNO}"));
                document.Add(new Paragraph($"Email: {hospitalInfo.Email}"));
                document.Add(new Paragraph($"Address: {hospitalInfo.Address}"));
                document.Add(new Paragraph($"Slogan: {hospitalInfo.Slogan}"));
                document.Add(new Paragraph("\n")); // Add some space

                // Add Title
                document.Add(new Paragraph("Patient Admission Report")
                    .SetFontSize(18)
                    .SetBold()
                    .SetFontColor(ColorConstants.DARK_GRAY));
                document.Add(new Paragraph($"From: {startDate.ToShortDateString()} To: {endDate.ToShortDateString()}"));
                document.Add(new Paragraph("\n")); // Add some space

                // Add Executive Summary
                document.Add(new Paragraph("Executive Summary")
                    .SetFontSize(16)
                    .SetBold()
                    .SetFontColor(ColorConstants.DARK_GRAY));
                document.Add(new Paragraph("This report provides an overview of patient admissions during the specified period. The data included highlights the number of admissions, patient demographics, and bed occupancy rates."));
                document.Add(new Paragraph("\n")); // Add some space

                // Add Table of Contents
                document.Add(new Paragraph("Table of Contents")
                    .SetFontSize(16)
                    .SetBold()
                    .SetFontColor(ColorConstants.DARK_GRAY));
                document.Add(new Paragraph("1. Hospital Information"));
                document.Add(new Paragraph("2. Patient Admission Overview"));
                document.Add(new Paragraph("3. Detailed Patient Report"));
                document.Add(new Paragraph("\n")); // Add some space

                // Add Table
                var table = new Table(6); // 6 columns
                table.SetBackgroundColor(ColorConstants.LIGHT_GRAY);
                table.AddHeaderCell(new Cell().Add(new Paragraph("Folder ID")).SetBackgroundColor(ColorConstants.GRAY));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Patient Name")).SetBackgroundColor(ColorConstants.GRAY));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Admit Date")).SetBackgroundColor(ColorConstants.GRAY));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Status")).SetBackgroundColor(ColorConstants.GRAY));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Ward Name")).SetBackgroundColor(ColorConstants.GRAY));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Bed No")).SetBackgroundColor(ColorConstants.GRAY));

                foreach (var patient in patients)
                {
                    table.AddCell(patient.FolderID.ToString());
                    table.AddCell($"{patient.FirstName} {patient.LastName}");
                    table.AddCell(patient.AdmitDate.ToShortDateString());
                    table.AddCell(patient.Status);
                    table.AddCell(patient.WardName);
                    table.AddCell(patient.BedNo?.ToString() ?? "N/A");
                }

                document.Add(table);
                document.Add(new Paragraph("\n")); // Add some space

                // Add Conclusion
                document.Add(new Paragraph("Conclusion")
                    .SetFontSize(16)
                    .SetBold()
                    .SetFontColor(ColorConstants.DARK_GRAY));
                document.Add(new Paragraph("This report serves as a vital tool for understanding patient trends and improving hospital management strategies. For further inquiries, please contact us."));
                document.Add(new Paragraph("\n")); // Add some space

                // Add Footer
                document.Add(new Paragraph($"Generated on: {DateTime.Now.ToShortDateString()} by {hospitalInfo.HospitalName}")
                    .SetFontSize(10)
                    .SetItalic()
                    .SetFontColor(ColorConstants.GRAY));
            }

            return File(stream.ToArray(), "application/pdf", "PatientReport.pdf");
        }

    }
}