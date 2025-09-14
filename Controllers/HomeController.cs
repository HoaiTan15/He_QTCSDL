using System.Web.Mvc;

namespace LTWeb02_Bai03.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Nếu đã login -> set dữ liệu cho Dashboard
            if (Session["User"] != null)
            {
                ViewBag.TotalProducts = 120;
                ViewBag.LowStockCount = 8;
                ViewBag.TotalCustomers = 35;
                ViewBag.TotalSuppliers = 12;
            }

            return View();
        }
    }
}
