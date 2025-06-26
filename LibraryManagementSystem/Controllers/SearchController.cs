using System.Security.Claims;
using LibraryManagementSystem.BLL;
using LibraryManagementSystem.Common.DTOs;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    public class SearchController : Controller
    {
        private readonly SearchService _searchService;
        private readonly BookService _bookService;
        private readonly ReaderService _readerService;

        public SearchController(SearchService searchService, BookService bookService, ReaderService readerService)
        {
            _searchService = searchService;
            _bookService = bookService;
            _readerService = readerService;
        }

        public async Task<IActionResult> Search()
        {
            await SetViewBag();

            return View(new SearchFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(SearchFormViewModel model)
        {
            await SetViewBag();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            List<SearchResultDTO> resultDTO = await _searchService.SearchBooks(model.Value, model.Option);

            model.Results = resultDTO.Count == 0 ? null : resultDTO.Select(dto => new SearchResultViewModel
            {
                BookId = dto.BookId,
                Title = dto.Title,
                SubjectId = dto.SubjectId,
                Subject = dto.Subject,
                Synopsis = dto.Synopsis,
                Authors = dto.Authors,
                Available = dto.Available,
                Rate = dto.Rate,
                Comment = dto.Comment
            }).ToList();

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            Book? book = await _bookService.GetBookById(id);

            await GetDataForCurrentRating(id);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> Rate(int bookId, short rate, string comment)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            (string? userEmail, Reader? reader) = await GetDataForCurrentRating(bookId);

            if (rate < 1 || rate > 5)
            {
                TempData["Error"] = "Rating must be between 1 and 5.";

                return RedirectToAction(nameof(Details), new { id = bookId });
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Share your reaction.";

                return RedirectToAction(nameof(Details), new { id = bookId });
            }


            if (reader is null)
                return Forbid();   

            await _readerService.UpdateRating(reader, bookId, rate, comment);
            TempData["Success"] = "Reaction successfully saved!";

            return RedirectToAction(nameof(Details), new { id = bookId });
        }

        private async Task SetViewBag()
        {
            ViewBag.Options = new List<string>() { "Title", "ISBN", "Author", "Editorial", "Edition Number", "Edition Year" };
        }

        private async Task<(string?, Reader?)> GetDataForCurrentRating(int bookId)
        {
            string? userEmail = User.FindFirstValue(ClaimTypes.Email);
            Reader? reader = await _readerService.GetReaderByEmail(userEmail);

            (short rate, string comment) = await _readerService.GetCurrentReaction(reader.ReaderId, bookId);
            ViewBag.CurrentRating = rate;
            ViewBag.CurrentComment = comment;

            return (userEmail, reader);
        }
    }
}
