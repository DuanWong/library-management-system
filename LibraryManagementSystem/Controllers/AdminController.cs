using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        /*private readonly IEmailSender _emailSender;*/ //--> intentionally commented.

        public AdminController(UserManager<IdentityUser> userManager,
                               RoleManager<IdentityRole> roleManager
                               /*IEmailSender emailSender*/)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            //_emailSender = emailSender;
        }

        public async Task<IActionResult> Index()
        {
            var allUsers = await _userManager.Users.ToListAsync();
            var librarians = new List<LibrarianViewModel>();

            foreach (var user in allUsers)
            {
                if (await _userManager.IsInRoleAsync(user, "Librarian"))
                {
                    librarians.Add(new LibrarianViewModel
                    {
                        Id = user.Id,
                        Email = user.Email,
                        PhoneNumber = user.PhoneNumber
                    });
                }
            }

            return View(librarians);
        }


        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null || !await _userManager.IsInRoleAsync(user, "Librarian"))
            {
                return NotFound();
            }

            var model = new LibrarianViewModel
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }

        public IActionResult Create()
        {
            return View(new LibrarianViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LibrarianViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            IdentityUser user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            IdentityResult result = await _userManager.CreateAsync(user, "Temp123!"); // Temporal password 

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            if (!await _roleManager.RoleExistsAsync("Librarian"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Librarian"));
            }

            await _userManager.AddToRoleAsync(user, "Librarian");

            // Generate token for password change.
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new
            {
                userId = user.Id,
                code = token
            }, protocol: Request.Scheme);

            // OPTION 1:
            /*
            await _emailSender.SendEmailAsync(email,
                "Set your password",
                $"Please set your password by <a href='{callbackUrl}'>clicking here</a>.");
            */

            // OPTION 2:
            ViewBag.ResetLink = callbackUrl;

            TempData["Success"] = TempData["Success"] = $"Librarian created. <a href='{callbackUrl}'>Click here to set password</a>.";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null || !await _userManager.IsInRoleAsync(user, "Librarian"))
            {
                return NotFound();
            }

            var model = new LibrarianViewModel
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LibrarianViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null || !await _userManager.IsInRoleAsync(user, "Librarian"))
            {
                return NotFound();
            }

            user.Email = model.Email;
            user.UserName = model.Email; // También se actualiza el UserName si es necesario
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                TempData["Error"] = "Error updating user.";
                return View(model);
            }

            TempData["Success"] = "Librarian updated successfully.";
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null || !await _userManager.IsInRoleAsync(user, "Librarian"))
            {
                return NotFound();
            }

            var model = new LibrarianViewModel
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null || !await _userManager.IsInRoleAsync(user, "Librarian"))
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                TempData["Error"] = "Error deleting user.";
                return RedirectToAction("Index");
            }

            TempData["Success"] = "Librarian deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
