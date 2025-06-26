using LibraryManagementSystem.BLL;
using LibraryManagementSystem.Common;
using LibraryManagementSystem.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Librarian")]
    public class BookController : Controller
    {
        private readonly BookService _bookService;
        private readonly SubjectService _subjectService;
        private readonly EditorialService _editorialService;

        public BookController(BookService bookService, SubjectService subjectService, EditorialService editorialService)
        {
            _bookService = bookService;
            _subjectService = subjectService;
            _editorialService = editorialService;
        }

        public async Task<IActionResult> Index()
        {
            List<Book> books = await _bookService.GetAllBooks();

            return View(books);
        }

        public async Task<IActionResult> Details(int id)
        {
            Book? book = await _bookService.GetBookById(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        public async Task<IActionResult> Create()
        {
            await SetViewBag();

            return View(new Book());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            await SetViewBag();

            if (!ModelState.IsValid)
            {
                if (book.Editions == null || !book.Editions.Any())
                {
                    TempData["Error"] = Constants.EDITIONS_REQUIRED_ERRORS;
                }

                return View(book);
            }

            ServiceResult result = await _bookService.AddBook(book);

            if (!result.Success)
            {
                if (result.Field != "")
                    ModelState.AddModelError(result.Field, result.Message);
                else
                    TempData["Error"] = result.Message;

                return View(book);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            await SetViewBag();

            Book? book = await _bookService.GetBookById(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book book)
        {
            await SetViewBag();

            if (!ModelState.IsValid)
            {
                if (book.Editions == null || !book.Editions.Any())
                {
                    TempData["Error"] = Constants.EDITIONS_REQUIRED_ERRORS;
                }

                return View(book);
            }

            ServiceResult result = await _bookService.UpdateBook(book);

            if (!result.Success)
            {
                if (result.Field != "")
                    ModelState.AddModelError(result.Field, result.Message);
                else
                    TempData["Error"] = result.Message;

                return View(book);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            Book? book = await _bookService.GetBookById(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Book? book = await _bookService.GetBookById(id);
            ServiceResult result = await _bookService.DeleteBook(book);

            if (!result.Success)
            {
                if (result.Field != "")
                    ModelState.AddModelError(result.Field, result.Message);
                else
                    TempData["Error"] = result.Message;

                return View(book);
            }

            return RedirectToAction("Index");
        }

        private async Task SetViewBag()
        {
            List<Subject> subjets = await _subjectService.GetAllSubjects();
            ViewBag.Subjects = subjets.Select(s => new { s.SubjectId, s.Name }).ToList();

            List<Editorial> editorials = await _editorialService.GetAllEditorials();
            ViewBag.Editorials = editorials.Select(e => new { e.EditorialId, e.Name }).ToList();
        }
    }
}