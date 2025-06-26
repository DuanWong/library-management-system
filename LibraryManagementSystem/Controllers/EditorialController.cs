using LibraryManagementSystem.BLL;
using LibraryManagementSystem.Common;
using LibraryManagementSystem.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers {

    [Authorize(Roles = "Librarian")]
    public class EditorialController : Controller {
        private readonly EditorialService _editorialService;

        public EditorialController(EditorialService editorialService) {
            _editorialService = editorialService;
        }

        public async Task<IActionResult> Index() {
            List<Editorial> editorials = await _editorialService.GetAllEditorials();

            return View(editorials);
        }

        public async Task<IActionResult> Details(int id) {
            Editorial? editorial = await _editorialService.GetEditorialById(id);

            if (editorial == null) {
                return NotFound();
            }

            return View(editorial);
        }

        public IActionResult Create() {
            return View(new Editorial());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Editorial editorial) {
            if (!ModelState.IsValid) {
                return View(editorial);
            }

            ServiceResult result = await _editorialService.AddEditorial(editorial);

            if (!result.Success) {
                if (result.Field != "")
                    ModelState.AddModelError(result.Field, result.Message);
                else
                    TempData["Error"] = result.Message;

                return View(editorial);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id) {
            Editorial? editorial = await _editorialService.GetEditorialById(id);

            if (editorial == null) {
                return NotFound();
            }

            return View(editorial);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Editorial editorial) {
            if (!ModelState.IsValid) {
                return View(editorial);
            }

            ServiceResult result = await _editorialService.UpdateEditorial(editorial);

            if (!result.Success) {
                if (result.Field != "")
                    ModelState.AddModelError(result.Field, result.Message);
                else
                    TempData["Error"] = result.Message;

                return View(editorial);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id) {
            Editorial? editorial = await _editorialService.GetEditorialById(id);

            if (editorial == null) {
                return NotFound();
            }

            return View(editorial);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            Editorial? editorial = await _editorialService.GetEditorialById(id);
            ServiceResult result = await _editorialService.DeleteEditorial(editorial);

            if (!result.Success) {
                if (result.Field != "")
                    ModelState.AddModelError(result.Field, result.Message);
                else
                    TempData["Error"] = result.Message;

                return View(editorial);
            }

            return RedirectToAction("Index");
        }
    }
}
