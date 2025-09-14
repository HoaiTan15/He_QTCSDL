using System.Collections.Generic;
using System.Web.Mvc;
using QuanLyKho.Models;

namespace QuanLyKho.Controllers
{
    public class ProductController : Controller
    {
        // Tạm tạo danh sách mẫu (sau này sẽ dùng DbContext)
        private static List<Product> products = new List<Product>
        {
            new Product { ProductID=1, Name="Chuột Logitech", Quantity=20, Price=250000 },
            new Product { ProductID=2, Name="Bàn phím cơ", Quantity=15, Price=850000 }
        };

        public ActionResult Index()
        {
            return View(products);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Product p)
        {
            p.ProductID = products.Count + 1;
            products.Add(p);
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var product = products.Find(x => x.ProductID == id);
            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product p)
        {
            var product = products.Find(x => x.ProductID == p.ProductID);
            if (product != null)
            {
                product.Name = p.Name;
                product.Price = p.Price;
                product.Quantity = p.Quantity;
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            var product = products.Find(x => x.ProductID == id);
            if (product != null) products.Remove(product);
            return RedirectToAction("Index");
        }
    }
}
