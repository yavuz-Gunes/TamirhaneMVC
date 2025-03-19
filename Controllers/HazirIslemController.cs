using Microsoft.AspNetCore.Mvc;
using TamirhaneMVC.Data;
using TamirhaneMVC.Models;
using System.Linq;

namespace TamirhaneMVC.Controllers
{
    public class HazirIslemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HazirIslemController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var hazirIslemler = _context.HazirIslemler.ToList();
            return View(hazirIslemler);
        }

        public IActionResult Ekle()
        {
            return View();
        }

       [HttpPost]
public IActionResult Ekle(HazirIslem islem)
{
    if (ModelState.IsValid)
    {
        _context.HazirIslemler.Add(islem);
        _context.SaveChanges();
        return Redirect("/Home/TekSayfa"); // İşlem ekleyince Tek Sayfa sistemine geri dönecek
    }
    return View(islem);
}


        public IActionResult Duzenle(int id)
        {
            var islem = _context.HazirIslemler.Find(id);
            if (islem == null) return NotFound();
            return View(islem);
        }

        [HttpPost]
        public IActionResult Duzenle(HazirIslem islem)
        {
            if (ModelState.IsValid)
            {
                _context.HazirIslemler.Update(islem);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(islem);
        }

        public IActionResult Sil(int id)
        {
            var islem = _context.HazirIslemler.Find(id);
            if (islem == null) return NotFound();
            return View(islem);
        }

        [HttpPost, ActionName("Sil")]
        public IActionResult SilOnayla(int id)
        {
            var islem = _context.HazirIslemler.Find(id);
            if (islem != null)
            {
                _context.HazirIslemler.Remove(islem);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
