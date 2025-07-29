CV Generator (Online Resume Builder)
This is an ASP.NET MVC project designed to help users create and manage their resumes online, with dynamic preview capabilities and PDF export. Future enhancements will include AI-powered suggestions and improvements.

Core Features
User-Friendly Forms: Intuitive sections for personal information, summary, education, experience, skills, and projects.

Dynamic Live Preview: Real-time updates of the resume as users fill out the forms, allowing for immediate visual feedback.

Multiple Templates: Selection of various professional templates (Modern, Classic, Minimal) to customize the resume's appearance.

PDF Export: Ability to export the generated resume as a high-quality PDF document for easy sharing and printing.

Data Persistence: Resume data is saved to a SQL Server database using Entity Framework Core, ensuring data is retained across sessions.

Technologies Used
Backend: ASP.NET Core MVC (C#)

Database: SQL Server (LocalDB for development)

ORM: Entity Framework Core

Frontend:

HTML5

CSS (Tailwind CSS via CDN)

JavaScript (Vanilla JS for dynamic form elements and preview logic)

PDF Generation (Client-side):

html2canvas: For capturing the HTML content as an image.

jsPDF: For converting the captured image into a PDF document.

Icons: Font Awesome (via CDN)

Fonts: Google Fonts (Inter)
