using Dapper;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using WardDapperMVC.Models.Domain;

namespace WardManagementSystem.Controllers
{
    public class EmployeeSearchController : Controller
    {
        private readonly IDbConnection _db;

        public EmployeeSearchController(IConfiguration configuration)
        {
            _db = new SqlConnection(configuration.GetConnectionString("conn"));
        }

        [HttpGet]
        public IActionResult EmployeeSearch()
        {
            return View(new EmployeeSearchViewModel());
        }

        [HttpPost]
        public IActionResult EmployeeSearch(EmployeeSearchViewModel model)
        {
            var sql = "SELECT * FROM Users WHERE 1=1"; // Basic query

            if (!string.IsNullOrEmpty(model.Role))
            {
                sql += " AND Role = @Role";
            }

            if (model.StartDate.HasValue)
            {
                sql += " AND EmploymentDate >= @StartDate";
            }

            if (model.EndDate.HasValue)
            {
                sql += " AND EmploymentDate <= @EndDate";
            }

            model.Employees = _db.Query<User>(sql, new { model.Role, model.StartDate, model.EndDate }).ToList();

            return View(model);
        }

        [HttpPost]
        public IActionResult GenerateReport(EmployeeSearchViewModel model)
        {
            var sqlEmployees = @"SELECT * FROM Users WHERE 1=1";

            if (!string.IsNullOrEmpty(model.Role))
            {
                sqlEmployees += " AND Role = @Role";
            }

            if (model.StartDate.HasValue)
            {
                sqlEmployees += " AND EmploymentDate >= @StartDate";
            }

            if (model.EndDate.HasValue)
            {
                sqlEmployees += " AND EmploymentDate <= @EndDate";
            }

            var employees = _db.Query<User>(sqlEmployees, new { model.Role, model.StartDate, model.EndDate }).ToList();
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
                document.Add(new Paragraph("Employee Report")
                    .SetFontSize(18)
                    .SetBold());

                // Conditional date range display
                if (model.StartDate.HasValue || model.EndDate.HasValue)
                {
                    document.Add(new Paragraph($"From: {model.StartDate?.ToShortDateString() ?? "N/A"} To: {model.EndDate?.ToShortDateString() ?? "N/A"}"));
                }

                document.Add(new Paragraph("\n")); // Add some space

                // Add Executive Summary
                document.Add(new Paragraph("Executive Summary")
                    .SetFontSize(16)
                    .SetBold());
                document.Add(new Paragraph("This report provides an overview of employee data during the specified period, including roles and employment dates."));
                document.Add(new Paragraph("\n")); // Add some space

                // Add Table of Contents
                document.Add(new Paragraph("Table of Contents")
                    .SetFontSize(16)
                    .SetBold());
                document.Add(new Paragraph("1. Hospital Information"));
                document.Add(new Paragraph("2. Employee Overview"));
                document.Add(new Paragraph("3. Detailed Employee Report"));
                document.Add(new Paragraph("\n")); // Add some space

                // Add Table
                var table = new Table(4); // 4 columns
                table.AddHeaderCell(new Cell().Add(new Paragraph("Employee Number")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Name")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Role")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                table.AddHeaderCell(new Cell().Add(new Paragraph("Employment Date")).SetBackgroundColor(ColorConstants.LIGHT_GRAY));

                // Check if employees list is not null and has entries
                if (employees != null && employees.Any())
                {
                    foreach (var employee in employees)
                    {
                        table.AddCell(employee.EmployeeNumber ?? "N/A");
                        table.AddCell($"{employee.FirstName} {employee.LastName}");
                        table.AddCell(employee.Role);
                        table.AddCell(employee.EmploymentDate.ToShortDateString());
                    }
                }
                else
                {
                    var noEmployeesCell = new Cell(1, 4) // 1 row, 4 columns
                        .Add(new Paragraph("No employees found for the specified criteria."))
                        .SetTextAlignment(TextAlignment.CENTER);
                    table.AddCell(noEmployeesCell);
                }

                document.Add(table);
                document.Add(new Paragraph("\n")); // Add some space

                // Add Conclusion
                document.Add(new Paragraph("Conclusion")
                    .SetFontSize(16)
                    .SetBold());
                document.Add(new Paragraph("This report serves as a vital tool for understanding employee trends and improving hospital management strategies. For further inquiries, please contact us."));
                document.Add(new Paragraph("\n")); // Add some space

                // Add Footer
                document.Add(new Paragraph($"Generated on: {DateTime.Now.ToShortDateString()} by {hospitalInfo.HospitalName}")
                    .SetFontSize(10)
                    .SetItalic());
            }

            return File(stream.ToArray(), "application/pdf", "EmployeeReport.pdf");
        }

        private HospitalInformation GetHospitalInfo()
        {
            var sql = "SELECT * FROM HospitalInfo WHERE InfoID = 1"; // Adjust as necessary
            return _db.QuerySingleOrDefault<HospitalInformation>(sql);
        }


    }
}