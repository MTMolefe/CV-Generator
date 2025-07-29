using CV_generator.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CV_generator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Configure ApplicationDbContext for ResumeData
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Note: The following lines for Identity are typically used when you have
            // ASP.NET Core Identity integrated. If your ApplicationDbContext is
            // solely for ResumeData, and not inheriting from IdentityDbContext,
            // you might need a separate DbContext for Identity or adjust ApplicationDbContext.
            // For now, assuming ApplicationDbContext handles both if needed, or Identity is separate.
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>(); // This line assumes ApplicationDbContext includes Identity tables.

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); // Added for Identity
            app.UseAuthorization();

            // Removed app.MapStaticAssets() and .WithStaticAssets() as they are not standard MVC routing
            // unless specific custom extensions are in use.

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Resume}/{action=Index}/{id?}"); // Changed default controller to Resume

            app.MapRazorPages(); // Added for Identity Razor Pages

            app.Run();
        }
    }
}