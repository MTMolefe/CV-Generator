using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CV_generator.Models
{
    public class ResumeData
    {
        [Key]
        public int Id { get; set; }

        public Personal Personal { get; set; }
        public string Summary { get; set; }
        public List<Education> Education { get; set; }
        public List<Experience> Experience { get; set; }
        public List<string> Skills { get; set; }
        public List<Project> Projects { get; set; }

        public ResumeData()
        {
            Personal = new Personal();
            Education = new List<Education>();
            Experience = new List<Experience>();
            Skills = new List<string>();
            Projects = new List<Project>();
        }
    }

    public class Personal
    {
        // These properties were missing!
        public string Name { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string LinkedIn { get; set; }
        public string GitHub { get; set; }
        public string Address { get; set; }

        public Personal()
        {
            // Initialize with default values if needed
            Name = "John Doe";
            Title = "Software Engineer";
            Email = "john.doe@example.com";
            Phone = "123-456-7890";
            LinkedIn = "linkedin.com/in/johndoe";
            GitHub = "github.com/johndoe";
            Address = "123 Main St, Anytown, USA";
        }
    }

    public class Education
    {
        public string Degree { get; set; }
        public string University { get; set; }
        public string Year { get; set; }
        public string Details { get; set; }
    }

    public class Experience
    {
        public string Title { get; set; }
        public string Company { get; set; }
        public string Years { get; set; }
        public List<string> Description { get; set; }

        public Experience()
        {
            Description = new List<string>();
        }
    }

    public class Project
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
    }
}
