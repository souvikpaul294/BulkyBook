using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BulkyBookWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var claims = (ClaimsIdentity)User.Identity;
            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            var shoppingCartObj = new ShoppingCartVM()
            {
                ListCart = _unitOfWork.ShoppingCart.GetAll("Product").Where(x => x.ApplicationUserId == userId).ToList()
            };
            foreach (var item in shoppingCartObj.ListCart)
            {
                shoppingCartObj.Total += GetTotal(item.Product, item.Count);
            }
            return View(shoppingCartObj);
        }
        [HttpGet]
        public IActionResult Plus(int cartId)
        {
            var cartObj = _unitOfWork.ShoppingCart.GetFirstOrDefault(x => x.Id == cartId);
            if (!object.ReferenceEquals(cartObj, null))
            {
                cartObj.Count = _unitOfWork.ShoppingCart.IncrementCount(cartObj, 1);
                _unitOfWork.ShoppingCart.Update(cartObj);
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Minus(int cartId)
        {
            var cartObj = _unitOfWork.ShoppingCart.GetFirstOrDefault(x => x.Id == cartId);
            if (!object.ReferenceEquals(cartObj, null))
            {
                if (cartObj.Count == 1)
                    _unitOfWork.ShoppingCart.Remove(cartObj);
                else
                {
                    cartObj.Count = _unitOfWork.ShoppingCart.DecrementCount(cartObj, 1);
                    _unitOfWork.ShoppingCart.Update(cartObj);
                }
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Remove(int cartId)
        {
            var cartObj = _unitOfWork.ShoppingCart.GetFirstOrDefault(x => x.Id == cartId);
            if (!object.ReferenceEquals(cartObj, null))
            {
                _unitOfWork.ShoppingCart.Remove(cartObj);
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));
        }
        [NonAction]
        private double GetTotal(Product product, int count)
        {
            if (!object.ReferenceEquals(product, null) && count > 0)
            {
                if (count > 100)
                    return count * product.Price100;
                else if (count >= 51 && count < 100)
                    return count * product.Price50;
                else
                    return count * product.Price;
            }
            return 0;
        }
    }
}
