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
            var islemler = _context.HazirIslemler.ToList();
            return View(islemler);
        }

        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Ekle(HazirIslem hazirIslem)
        {
            if (ModelState.IsValid)
            {
                _context.HazirIslemler.Add(hazirIslem);
                _context.SaveChanges();
                TempData["Mesaj"] = "İşçilik başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(hazirIslem);
        }

        public IActionResult Duzenle(int id)
        {
            var hazirIslem = _context.HazirIslemler.Find(id);
            if (hazirIslem == null)
            {
                return NotFound();
            }
            return View(hazirIslem);
        }

        [HttpPost]
        public IActionResult Duzenle(HazirIslem hazirIslem)
        {
            if (ModelState.IsValid)
            {
                _context.HazirIslemler.Update(hazirIslem);
                _context.SaveChanges();
                TempData["Mesaj"] = "İşçilik başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(hazirIslem);
        }

        public IActionResult Sil(int id)
        {
            var hazirIslem = _context.HazirIslemler.Find(id);
            if (hazirIslem == null)
            {
                return NotFound();
            }
            return View(hazirIslem);
        }

        [HttpPost]
        public IActionResult SilOnay(int id)
        {
            var hazirIslem = _context.HazirIslemler.Find(id);
            if (hazirIslem == null)
            {
                return NotFound();
            }

            _context.HazirIslemler.Remove(hazirIslem);
            _context.SaveChanges();
            TempData["Mesaj"] = "İşçilik başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
