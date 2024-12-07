using System;
using System.Collections.Generic;
using System.Linq;
using NorthwindApp.Models;

namespace NorthwindApp.Repositories
{
    public class ProductRepository
    {
        private readonly NorthwindContext _context = new();

        public IEnumerable<Product> GetAllProducts(bool includeDiscontinued)
        {
            return includeDiscontinued
                ? _context.Products.ToList()
                : _context.Products.Where(p => !p.Discontinued).ToList();
        }
        public IEnumerable<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }
        public Product GetProductById(int productId)
        {
            return _context.Products.Find(productId);
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void DeleteProduct(int productId)
        {
            var product = _context.Products.Find(productId);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}
