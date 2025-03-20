using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TamirhaneMVC.Data;
using TamirhaneMVC.Models;

namespace TamirhaneMVC.Controllers
{
    public class ParcaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParcaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string parcaAdi, string stokDurumu)
        {
            ViewBag.ParcaAdiFilter = parcaAdi;
            ViewBag.StokDurumu = stokDurumu;

            var parcalar = _context.Parcalar.AsQueryable();

            // Filtreleri uygula
            if (!string.IsNullOrEmpty(parcaAdi))
            {
                parcalar = parcalar.Where(p => p.ParcaAdi.Contains(parcaAdi));
            }

            if (!string.IsNullOrEmpty(stokDurumu))
            {
                switch (stokDurumu)
                {
                    case "kritik":
                        parcalar = parcalar.Where(p => p.StokMiktari > 0 && p.StokMiktari <= 10);
                        break;
                    case "mevcut":
                        parcalar = parcalar.Where(p => p.StokMiktari > 10);
                        break;
                    case "tukenmis":
                        parcalar = parcalar.Where(p => p.StokMiktari <= 0);
                        break;
                }
            }

            return View(parcalar.ToList());
        }

        public IActionResult Ekle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Ekle(Parca parca)
        {
            if (ModelState.IsValid)
            {
                _context.Parcalar.Add(parca);
                _context.SaveChanges();
                TempData["Mesaj"] = "Parça başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(parca);
        }

        public IActionResult Duzenle(int id)
        {
            var parca = _context.Parcalar.Find(id);
            if (parca == null)
            {
                return NotFound();
            }
            return View(parca);
        }

        [HttpPost]
        public IActionResult Duzenle(Parca parca)
        {
            if (ModelState.IsValid)
            {
                _context.Parcalar.Update(parca);
                _context.SaveChanges();
                TempData["Mesaj"] = "Parça başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            return View(parca);
        }

        public IActionResult Sil(int id)
        {
            var parca = _context.Parcalar.Find(id);
            if (parca == null)
            {
                return NotFound();
            }
            return View(parca);
        }

        [HttpPost]
        public IActionResult SilOnay(int id)
        {
            var parca = _context.Parcalar.Find(id);
            if (parca == null)
            {
                return NotFound();
            }

            _context.Parcalar.Remove(parca);
            _context.SaveChanges();
            TempData["Mesaj"] = "Parça başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult StokEkle(int id)
        {
            var parca = _context.Parcalar.Find(id);
            if (parca == null)
            {
                return NotFound();
            }
            return View(parca);
        }

        [HttpPost]
        public IActionResult StokEkleOnay(int id, int EklenecekMiktar)
        {
            var parca = _context.Parcalar.Find(id);
            if (parca == null)
            {
                return NotFound();
            }

            parca.StokMiktari += EklenecekMiktar;
            _context.SaveChanges();
            TempData["Mesaj"] = $"{parca.ParcaAdi} parçasına {EklenecekMiktar} adet stok eklendi.";
            return RedirectToAction(nameof(Index));
        }
    }
} 