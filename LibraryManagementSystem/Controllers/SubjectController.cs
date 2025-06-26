using LibraryManagementSystem.BLL;
using LibraryManagementSystem.Common;
using LibraryManagementSystem.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize(Roles = "Librarian")]
    public class SubjectController : Controller
    {
        private readonly SubjectService _subjectService;

        public SubjectController(SubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        public async Task<IActionResult> Index()
        {
            List<Subject> subjects = await _subjectService.GetAllSubjects();

            return View(subjects);
        }

        public async Task<IActionResult> Details(int id)
        {
            Subject? subject = await _subjectService.GetSubjectById(id);

            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        public IActionResult Create()
        {
            return View(new Subject());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subject subject)
        {
            if (!ModelState.IsValid)
            {
                return View(subject);
            }

            ServiceResult result = await _subjectService.AddSubject(subject);

            if (!result.Success)
            {
                if (result.Field != "")
                    ModelState.AddModelError(result.Field, result.Message);
                else
                    TempData["Error"] = result.Message;

                return View(subject);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            Subject? subject = await _subjectService.GetSubjectById(id);

            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Subject subject)
        {
            if (!ModelState.IsValid)
            {
                return View(subject);
            }

            ServiceResult result = await _subjectService.UpdateSubject(subject);

            if (!result.Success)
            {
                if (result.Field != "")
                    ModelState.AddModelError(result.Field, result.Message);
                else
                    TempData["Error"] = result.Message;

                return View(subject);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            Subject? subject = await _subjectService.GetSubjectById(id);

            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Subject? subject = await _subjectService.GetSubjectById(id);
            ServiceResult result = await _subjectService.DeleteSubject(subject);

            if (!result.Success)
            {
                if (result.Field != "")
                    ModelState.AddModelError(result.Field, result.Message);
                else
                    TempData["Error"] = result.Message;

                return View(subject);
            }

            return RedirectToAction("Index");
        }
    }
}
