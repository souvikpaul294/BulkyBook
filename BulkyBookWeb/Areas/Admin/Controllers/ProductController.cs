using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHost;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHost)
        {
            _unitOfWork = unitOfWork;
            _webHost = webHost;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            //ViewData["listOfCategory"] = listOfCategory();
            //ViewData["listOfCoverType"] = listOfCoverType();
            var productVM = new ProductVM()
            {
                product = new Product(),
                CategoryList = listOfCategory(),
                CoverTypeList = listOfCoverType()

            };
            if (id == null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
                return View(productVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (!object.ReferenceEquals(file, null))
                {
                    var rootPath = _webHost.WebRootPath;
                    var uploadFolderPath = Path.Combine(rootPath, @"images\products");
                    var imageName = Guid.NewGuid().ToString();
                    var imageExtension = Path.GetExtension(file.FileName);
                    if(!string.IsNullOrEmpty(obj.product.ImageUrl))
                        if (System.IO.File.Exists(Path.Combine(rootPath,obj.product.ImageUrl))) {
                            System.IO.File.Delete(Path.Combine(rootPath, obj.product.ImageUrl));
                        }
                    using (var fileStream = new FileStream(Path.Combine(uploadFolderPath, (imageName + imageExtension)), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    obj.product.ImageUrl = $@"images\products\{imageName}{imageExtension}";
                    if (obj.product.Id == 0 || object.ReferenceEquals(obj.product.Id, null))
                    {
                        _unitOfWork.Product.Add(obj.product);
                        TempData["success"] = "Product created successfully";
                    }
                    else
                    {
                        _unitOfWork.Product.Update(obj.product);
                        TempData["success"] = "Product updated successfully";
                    }
                    _unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            var productVM = new ProductVM()
            {
                product = obj.product,
                CategoryList = listOfCategory(),
                CoverTypeList = listOfCoverType()
            };
            TempData["error"] = "Product could not be created";
            return View(productVM);
        }
        private List<SelectListItem> listOfCategory()
        {
            return _unitOfWork.Category.GetAll().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
        }
        private List<SelectListItem> listOfCoverType()
        {
            return _unitOfWork.CoverType.GetAll().Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
        }
        #region API Calls
        [HttpGet]
        public  IActionResult GetAllProducts()
        {
            var list = _unitOfWork.Product.GetAll("Category,CoverType");
            return Json(new { data = list });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
            if (object.ReferenceEquals(obj, null))
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            if (!string.IsNullOrEmpty(obj.ImageUrl))
                if (System.IO.File.Exists(Path.Combine(_webHost.WebRootPath, obj.ImageUrl)))
                {
                    System.IO.File.Delete(Path.Combine(_webHost.WebRootPath, obj.ImageUrl));
                }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Product deleted successfully" });
        }
        #endregion
    }
}
