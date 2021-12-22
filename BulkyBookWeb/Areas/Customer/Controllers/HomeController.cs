using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace BulkyBookWeb.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View(_unitOfWork.Product.GetAll());
        }
        [HttpGet("[area]/[controller]/[action]/{productId:int:min(1)}")]
        public IActionResult Details(int? productId)
        {
            var shoppingCart = new ShoppingCart()
            {
                Count = 1,
                ProductId = (int)productId,
                Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == productId, "Category,CoverType"),
            };
            return View(shoppingCart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;
            var shoppingCartObj = _unitOfWork.ShoppingCart.GetFirstOrDefault(x => x.ProductId == shoppingCart.ProductId && x.ApplicationUserId == userId);
            if (object.ReferenceEquals(shoppingCartObj, null))
                _unitOfWork.ShoppingCart.Add(shoppingCart);
            else
            {
                shoppingCartObj.Count = _unitOfWork.ShoppingCart.IncrementCount(shoppingCartObj, shoppingCart.Count);
                _unitOfWork.ShoppingCart.Update(shoppingCartObj);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}