using LibraryManagementSystem.BLL;
using LibraryManagementSystem.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register dbcontext with the connection string from appsettings.json.
            builder.Services.AddDbContext<LibraryDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>() // Add support for roles.
                .AddEntityFrameworkStores<LibraryDbContext>();

            // Register DAL and BLL services.
            builder.Services.AddScoped<AuthorRepository>();
            builder.Services.AddScoped<BookRepository>();
            builder.Services.AddScoped<EditorialRepository>();
            builder.Services.AddScoped<ReaderRepository>();
            builder.Services.AddScoped<LoanRepository>();
            builder.Services.AddScoped<SubjectRepository>();
            builder.Services.AddScoped<SearchRepository>();
            builder.Services.AddScoped<ReportsRepository>();

            builder.Services.AddScoped<AuthorService>();
            builder.Services.AddScoped<BookService>();
            builder.Services.AddScoped<EditorialService>();
            builder.Services.AddScoped<ReaderService>();
            builder.Services.AddScoped<LoanService>();
            builder.Services.AddScoped<SubjectService>();
            builder.Services.AddScoped<SearchService>();
            builder.Services.AddScoped<ReportsService>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Seed roles and users here.
            SeedRolesAndUsersAsync(app.Services).GetAwaiter().GetResult();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }

        static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                UserManager<IdentityUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                try
                {
                    string[] roles = { "Librarian", "Reader" };
                    foreach (string role in roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            await roleManager.CreateAsync(new IdentityRole(role));
                            Console.WriteLine($"Rol created: {role}");
                        }
                    }

                    string librarianEmail = "johndoe@library.com";
                    IdentityUser librarianUser = await userManager.FindByEmailAsync(librarianEmail);

                    if (librarianUser == null)
                    {
                        librarianUser = new IdentityUser
                        {
                            UserName = librarianEmail,
                            Email = librarianEmail,
                            EmailConfirmed = true
                        };

                        IdentityResult result = await userManager.CreateAsync(librarianUser, "Admin123!");

                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(librarianUser, "Librarian");
                            Console.WriteLine("User librarian created and assignated to the Librarian rol.");
                        }
                        else
                        {
                            Console.WriteLine($"Error creating librarian: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("User librarian already exists.");
                    }

                    var readers = userManager.Users.ToList();

                    foreach (var reader in readers)
                    {
                        if (!await userManager.IsInRoleAsync(reader, "Reader"))
                        {
                            await userManager.AddToRoleAsync(reader, "Reader");
                            Console.WriteLine($"'Reader' rol asssignated to: {reader.Email}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in SeedRolesAndUsersAsync: {ex.Message}");
                }
            }
        }
    }
}
