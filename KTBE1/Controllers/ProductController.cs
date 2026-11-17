using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using KTBE1.Models;

namespace KTBE1.Controllers
{
    public class ProductController : Controller
    {
        private KTBEEntities db = new KTBEEntities();

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var product = db.Product.Include(p => p.Category).FirstOrDefault(p => p.ProductID == id);
            if (product == null) return HttpNotFound();
            return View(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}