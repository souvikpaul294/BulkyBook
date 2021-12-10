using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Product product)
        {
            var obj = _db.Products.FirstOrDefault(x => x.Id == product.Id);
            if(obj != null)
            {
                obj.Title = product.Title;
                obj.Author = product.Author;
                obj.ISBN = product.ISBN;
                obj.Description = product.Description;
                obj.ListPrice = product.ListPrice;
                obj.Price = product.Price;
                obj.Price50 = product.Price50;
                obj.Price100 = product.Price100;
                obj.CategoryId = product.CategoryId;
                obj.CoverTypeId = product.CoverTypeId;
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    obj.ImageUrl = product.ImageUrl;
                }
                //_db.Products.Update(obj);
            }
        }
    }
}
