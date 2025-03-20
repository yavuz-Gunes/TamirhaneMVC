using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TamirhaneMVC.Data;
using TamirhaneMVC.Models;

namespace TamirhaneMVC.Controllers
{
    public class AracController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AracController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string plaka, DateTime? baslangicTarihi, DateTime? bitisTarihi)
        {
            ViewBag.PlakFilter = plaka;
            ViewBag.BaslangicTarihi = baslangicTarihi?.ToString("yyyy-MM-dd");
            ViewBag.BitisTarihi = bitisTarihi?.ToString("yyyy-MM-dd");

            var araclar = _context.Arabalar.AsQueryable();

            // Filtreleri uygula
            if (!string.IsNullOrEmpty(plaka))
            {
                araclar = araclar.Where(a => a.Plaka.Contains(plaka));
            }

            if (baslangicTarihi.HasValue)
            {
                araclar = araclar.Where(a => a.GirişTarihi >= baslangicTarihi.Value);
            }

            if (bitisTarihi.HasValue)
            {
                var bitisTarihiSonu = bitisTarihi.Value.AddDays(1).AddTicks(-1);
                araclar = araclar.Where(a => a.GirişTarihi <= bitisTarihiSonu);
            }

            return View(araclar.ToList());
        }

        public IActionResult Detay(int id)
        {
            var arac = _context.Arabalar
                .Include(a => a.Islemler)
                    .ThenInclude(i => i.KullanilanParcalar)
                        .ThenInclude(kp => kp.Parca)
                .FirstOrDefault(a => a.Id == id);

            if (arac == null)
            {
                return NotFound();
            }

            return View(arac);
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
                TempData["Mesaj"] = "Araç başarıyla eklendi.";
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
                TempData["Mesaj"] = "Araç başarıyla güncellendi.";
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
                TempData["Mesaj"] = "Araç başarıyla silindi.";
            }
            return RedirectToAction("Index");
        }
    }
}
