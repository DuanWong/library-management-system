using LibraryManagementSystem.BLL;
using LibraryManagementSystem.Common;
using LibraryManagementSystem.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Librarian")]
    public class AuthorController : Controller
    {
        private readonly AuthorService _authorService;
        private readonly BookService _bookService;

        public AuthorController(AuthorService authorService, BookService bookService)
        {
            _authorService = authorService;
            _bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
            List<Author> authors = await _authorService.GetAllAuthors();

            return View(authors);
        }

        public async Task<IActionResult> Details(int id)
        {
            Author? author = await _authorService.GetAuthorById(id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        public async Task<IActionResult> Create()
        {
            await SetViewBag();

            return View(new Author());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author)
        {
            await SetViewBag();

            if (!ModelState.IsValid)
            {
                return View(author);
            }

            ServiceResult result = await _authorService.AddAuthor(author);

            if (!result.Success)
            {
                if (result.Field != "") 
                    ModelState.AddModelError(result.Field, result.Message);
                else
                    TempData["Error"] = result.Message;

                return View(author);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            await SetViewBag();

            Author? author = await _authorService.GetAuthorById(id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Author author)
        {
            await SetViewBag();

            if (!ModelState.IsValid)
            {
                return View(author);
            }

            ServiceResult result = await _authorService.UpdateAuthor(author);

            if (!result.Success)
            {
                if (result.Field != "")
                    ModelState.AddModelError(result.Field, result.Message);
                else
                    TempData["Error"] = result.Message;

                return View(author);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            Author? author = await _authorService.GetAuthorById(id);

            if (author == null)
            {
                return NotFound();
            }

            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Author? author = await _authorService.GetAuthorById(id);
            ServiceResult result = await _authorService.DeleteAuthor(author);

            if (!result.Success)
            {
                if (result.Field != "")
                    ModelState.AddModelError(result.Field, result.Message);
                else
                    TempData["Error"] = result.Message;

                return View(author);
            }

            return RedirectToAction("Index");
        }

        private async Task SetViewBag()
        {
            List<Book> books = await _bookService.GetAllBooks();
            ViewBag.Books = books.Select(b => new { b.BookId, b.Title }).ToList();
        }
    }
}
