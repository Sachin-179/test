using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using CsvToSqlConverter.Models;
using Microsoft.EntityFrameworkCore;

namespace CsvToSqlConverter.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (UploadedFile != null && UploadedFile.Length > 0)
            {
                // Parse the file and get a list of Employee objects
                var employees = new List<Employee>();
                using (var reader = new StreamReader(UploadedFile.OpenReadStream()))
                using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    employees = csv.GetRecords<Employee>().ToList();
                }

                // Insert data into the database
                await _context.employee.AddRangeAsync(employees);
                await _context.SaveChangesAsync();

                ViewData["Message"] = "File successfully uploaded and data inserted into the database.";
            }
            else
            {
                ViewData["Message"] = "Please upload a file.";
            }

            return Page();
        }
    }
}
