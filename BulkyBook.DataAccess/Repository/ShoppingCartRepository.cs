using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCart
    {
        private readonly ApplicationDbContext _db;

        public ShoppingCartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public int DecrementCount(ShoppingCart cart, int count)
        {
            if (cart.Count != 0)
                cart.Count -= count;
            return cart.Count;
        }

        public int IncrementCount(ShoppingCart cart, int count)
        {
            return cart.Count + count;
        }

        public void Update(ShoppingCart cart)
        {
            _db.ShoppingCarts.Update(cart);
        }
    }
}
