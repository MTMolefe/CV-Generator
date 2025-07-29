using CV_generator.Models;
using CV_generator.Data; // Required for ApplicationDbContext
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore; // Required for .FirstOrDefaultAsync() and .Include()
using System.Threading.Tasks; // Required for Task<IActionResult>

namespace CV_generator.Controllers
{
    public class ResumeController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructor to inject the database context
        public ResumeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Resume/Index
        // This action displays the form for building the resume.
        public async Task<IActionResult> Index()
        {
            // Try to load an existing resume (e.g., the first one, or by user ID in a real app)
            // For simplicity, we'll try to load the first resume found.
            // In a multi-user application, you would filter by a user ID.
            var model = await _context.Resumes
                                      .Include(r => r.Personal)
                                      .Include(r => r.Education)
                                      .Include(r => r.Experience)
                                      .Include(r => r.Projects)
                                      .FirstOrDefaultAsync();

            if (model == null)
            {
                // If no resume exists, initialize a new one with default values
                model = new ResumeData();
                model.Summary = "Highly motivated and results-oriented professional seeking opportunities to apply technical skills and contribute to innovative projects.";
                model.Education.Add(new Education { Degree = "Bachelor of Science", University = "Example University", Year = "2020", Details = "Relevant coursework in..." });
                model.Experience.Add(new Experience { Title = "Junior Developer", Company = "Tech Co.", Years = "2020 - 2022", Description = new List<string> { "Developed features for...", "Collaborated with team..." } });
                model.Skills.AddRange(new List<string> { "C#", "ASP.NET MVC", "JavaScript", "HTML", "CSS" });
                model.Projects.Add(new Project { Name = "Portfolio Website", Description = "Built a personal portfolio site.", Link = "github.com/yourprofile/portfolio" });
            }

            return View(model);
        }

        // POST: Resume/Create
        // This action handles the form submission and saves/updates the resume data in the database.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ResumeData resumeData)
        {
            if (ModelState.IsValid)
            {
                // Check if a resume with this Id already exists (for updates)
                if (resumeData.Id == 0)
                {
                    // New resume, add to database
                    _context.Resumes.Add(resumeData);
                }
                else
                {
                    // Existing resume, update it
                    // Fetch the existing entity to ensure all owned collections are tracked correctly for updates.
                    var existingResume = await _context.Resumes
                        .Include(r => r.Personal)
                        .Include(r => r.Education)
                        .Include(r => r.Experience)
                        .Include(r => r.Projects)
                        .FirstOrDefaultAsync(r => r.Id == resumeData.Id);

                    if (existingResume != null)
                    {
                        // Update scalar properties
                        existingResume.Summary = resumeData.Summary;
                        existingResume.Skills = resumeData.Skills; // This handles JSON conversion for Skills

                        // Update Personal owned entity
                        // SetValues updates properties of an existing entity from another entity
                        _context.Entry(existingResume.Personal).CurrentValues.SetValues(resumeData.Personal);

                        // Update Education collection (clear and re-add for simplicity with owned collections)
                        existingResume.Education.Clear();
                        foreach (var edu in resumeData.Education)
                        {
                            existingResume.Education.Add(edu);
                        }

                        // Update Experience collection
                        existingResume.Experience.Clear();
                        foreach (var exp in resumeData.Experience)
                        {
                            existingResume.Experience.Add(exp);
                        }

                        // Update Projects collection
                        existingResume.Projects.Clear();
                        foreach (var proj in resumeData.Projects)
                        {
                            existingResume.Projects.Add(proj);
                        }

                        // Mark the main entity as modified if its scalar properties were changed
                        _context.Resumes.Update(existingResume);
                    }
                    else
                    {
                        // If Id was provided but not found, treat as new (or handle error)
                        _context.Resumes.Add(resumeData);
                    }
                }

                await _context.SaveChangesAsync(); // Save changes to the database

                ViewBag.Message = "Resume data saved successfully!";
                return View("Index", resumeData); // Return to the index view with submitted data
            }

            // If model state is not valid, return the view with validation errors
            return View("Index", resumeData);
        }
    }
}
