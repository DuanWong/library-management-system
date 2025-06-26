using System.Security.Claims;
using LibraryManagementSystem.BLL;
using LibraryManagementSystem.Common.DTOs;
using LibraryManagementSystem.Model;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Librarian,Reader")]
    public class ReportsController : Controller
    {
        private readonly ReportsService _reportsService;
        private readonly ReaderService _readerService;

        public ReportsController(ReportsService reportsService, ReaderService readerService)
        {
            _reportsService = reportsService;
            _readerService = readerService;
        }

        public IActionResult LoansPerReader()
        {
            LoansPerReaderFormViewModel model = new LoansPerReaderFormViewModel();

            if (!User.IsInRole("Librarian"))
            {
                model.Email = User.FindFirstValue(ClaimTypes.Email);
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoansPerReader(LoansPerReaderFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            List<LoansPerReaderResultDTO> resultDTO = await _reportsService.LoansPerReader(model.ReaderFullName, model.Email);

            model.Results = resultDTO.Count == 0 ? null : resultDTO.Select(dto => new LoansPerReaderResultViewModel
            {
                ReaderId = dto.ReaderId,
                ReaderFullName = dto.ReaderFullName,
                Email = dto.Email,
                Age = dto.Age,
                Loans = dto.Loans
            }).ToList();

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            Reader? reader = await _readerService.GetReaderById(id);

            if (reader == null)
            {
                return NotFound();
            }

            return View(reader);
        }
    }
}
