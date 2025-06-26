using LibraryManagementSystem.BLL;
using LibraryManagementSystem.Common;
using LibraryManagementSystem.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Librarian")]
    public class LoanController : Controller
    {
        private readonly LoanService _loanService;
        private readonly BookService _bookService;
        private readonly ReaderService _readerService;

        public LoanController(LoanService loanService, BookService bookService, ReaderService readerService)
        {
            _loanService = loanService;
            _bookService = bookService;
            _readerService = readerService;
        }

        public async Task<IActionResult> Index()
        {
            List<Loan> loans = await _loanService.GetAllLoans();

            return View(loans);
        }

        public async Task<IActionResult> Details(int id)
        {
            Loan? loan = await _loanService.GetLoanById(id);

            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        public async Task<IActionResult> Create()
        {
            await SetViewBag();

            return View(new Loan());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Loan loan)
        {
            await SetViewBag();

            if (!ModelState.IsValid)
            {
                return View(loan);
            }

            ServiceResult result = await _loanService.AddLoan(loan);

            if (!result.Success)
            {
                if (result.Field != "") 
                    ModelState.AddModelError(result.Field, result.Message);
                else
                    TempData["Error"] = result.Message;

                return View(loan);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            await SetViewBag();

            Loan? loan = await _loanService.GetLoanById(id);

            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Loan loan)
        {
            await SetViewBag();

            if (!ModelState.IsValid)
            {
                return View(loan);
            }

            ServiceResult result = await _loanService.UpdateLoan(loan);

            if (!result.Success)
            {
                if (result.Field != "")
                    ModelState.AddModelError(result.Field, result.Message);
                else
                    TempData["Error"] = result.Message;

                return View(loan);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            Loan? loan = await _loanService.GetLoanById(id);

            if (loan == null)
            {
                return NotFound();
            }

            return View(loan);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Loan? loan = await _loanService.GetLoanById(id);
            ServiceResult result = await _loanService.DeleteLoan(loan);

            if (!result.Success)
            {
                if (result.Field != "")
                    ModelState.AddModelError(result.Field, result.Message);
                else
                    TempData["Error"] = result.Message;

                return View(loan);
            }

            return RedirectToAction("Index");
        }

        private async Task SetViewBag()
        {
            List<Book> books = await _bookService.GetAllBooks();
            ViewBag.Books = books.Select(b => new { b.BookId, b.Title }).ToList();

            List<Reader> readers = await _readerService.GetAllReaders();
            ViewBag.Readers = readers.Select(b => new { b.ReaderId, b.FullName }).ToList();
        }
    }
}
