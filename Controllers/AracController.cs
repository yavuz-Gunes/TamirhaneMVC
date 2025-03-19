using Microsoft.AspNetCore.Mvc;
using TamirhaneMVC.Data;
using TamirhaneMVC.Models;
using System.Linq;

namespace TamirhaneMVC.Controllers
{
    public class AracController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AracController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var araclar = _context.Arabalar.ToList();
            return View(araclar);
        }

        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Ekle(Arac arac)
        {
            if (ModelState.IsValid)
            {
                _context.Arabalar.Add(arac);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(arac);
        }

        public IActionResult Duzenle(int id)
        {
            var arac = _context.Arabalar.Find(id);
            if (arac == null)
            {
                return NotFound();
            }
            return View(arac);
        }

        [HttpPost]
        public IActionResult Duzenle(Arac arac)
        {
            if (ModelState.IsValid)
            {
                _context.Arabalar.Update(arac);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(arac);
        }

        public IActionResult Sil(int id)
        {
            var arac = _context.Arabalar.Find(id);
            if (arac == null)
            {
                return NotFound();
            }
            return View(arac);
        }

        [HttpPost, ActionName("Sil")]
        public IActionResult SilOnayla(int id)
        {
            var arac = _context.Arabalar.Find(id);
            if (arac != null)
            {
                _context.Arabalar.Remove(arac);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
