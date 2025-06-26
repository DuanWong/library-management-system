using System.Security.Claims;
using LibraryManagementSystem.BLL;
using LibraryManagementSystem.Common;
using LibraryManagementSystem.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class ReaderController : Controller
    {
        private readonly ReaderService _readerService;
        private readonly LoanService _loanService;
        private readonly UserManager<IdentityUser> _userManager;

        public ReaderController(ReaderService readerService, LoanService loanService, UserManager<IdentityUser> userManager)
        {
            _readerService = readerService;
            _loanService = loanService;
            _userManager = userManager;
        }

        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> Index()
        {
            List<Reader> readers = await _readerService.GetAllReaders();

            return View(readers);
        }

        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> Details(int id)
        {
            Reader? reader = await _readerService.GetReaderById(id);

            if (reader == null)
            {
                return NotFound();
            }

            return View(reader);
        }

        public async Task<IActionResult> Create()
        {
            return View(new Reader());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Reader reader)
        {
            if (!ModelState.IsValid)
            {
                return View(reader);
            }

            ServiceResult result = await _readerService.AddReader(reader);

            if (!result.Success)
            {
                if (result.Field != "") 
                    ModelState.AddModelError(result.Field, result.Message);
                else
                    TempData["Error"] = result.Message;

                return View(reader);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> Edit(int id)
        {
            Reader? reader = await _readerService.GetReaderById(id);

            if (reader == null)
            {
                return NotFound();
            }

            return View(reader);
        }

        [HttpPost]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> Edit(Reader reader)
        {
            if (!ModelState.IsValid)
            {
                return View(reader);
            }

            ServiceResult result = await _readerService.UpdateReader(reader);

            if (!result.Success)
            {
                if (result.Field != "")
                    ModelState.AddModelError(result.Field, result.Message);
                else
                    TempData["Error"] = result.Message;

                return View(reader);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> Delete(int id)
        {
            Reader? reader = await _readerService.GetReaderById(id);

            if (reader == null)
            {
                return NotFound();
            }

            return View(reader);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Reader? reader = await _readerService.GetReaderById(id);
            ServiceResult result = await _readerService.DeleteReader(reader);

            if (!result.Success)
            {
                if (result.Field != "")
                    ModelState.AddModelError(result.Field, result.Message);
                else
                    TempData["Error"] = result.Message;

                return View(reader);
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> MyAccount() {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var allReaders = await _readerService.GetAllReaders();
            var reader = allReaders.FirstOrDefault(r => r.CoreId == userId);

            if (reader == null) {
                return NotFound("Reader not found.");
            }

            var loans = (await _loanService.GetAllLoans())
                        .Where(l => l.ReaderId == reader.ReaderId)
                        .ToList();

            MyAccountViewModel viewModel = new MyAccountViewModel {
                Reader = reader,
                Loans = loans
            };

            return View(viewModel);
        }
    }
}
