using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View(_unitOfWork.Category.GetAll());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpGet("/[Area]/Category/Edit/{id:int:min(1)}")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var item = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View(category);
        }
        [HttpGet("/Category/Delete/{id:int:min(1)}")]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var item = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
        [HttpPost("/Category/Delete/{id:int:min(1)}"),ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategory(int? id)
        {
            var obj = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            if (obj != null)
            {
                _unitOfWork.Category.Remove(obj);
                _unitOfWork.Save();
                TempData["success"] = "Category deleted successfully";
                return RedirectToAction("Index");
            }
            return View(obj);

        }
    }
}
