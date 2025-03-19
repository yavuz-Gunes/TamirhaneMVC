using Microsoft.AspNetCore.Mvc;
using TamirhaneMVC.Data;
using TamirhaneMVC.Models;
using System.Linq;

namespace TamirhaneMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult TekSayfa()
        {
            var hazirIslemler = _context.HazirIslemler.ToList();
            ViewBag.HazirIslemler = hazirIslemler;
            return View();
        }

        [HttpPost]
public IActionResult Kaydet([FromBody] KaydetModel model)
{
    if (model == null || string.IsNullOrEmpty(model.Plaka) || model.SecilenIslemler == null || model.SecilenIslemler.Length == 0)
    {
        return BadRequest(new { success = false, message = "Plaka ve işlemler boş olamaz." });
    }

    try
    {
        // Aracı kaydet
        var arac = new Arac { Plaka = model.Plaka, GirişTarihi = System.DateTime.Now };
        _context.Arabalar.Add(arac);
        _context.SaveChanges();

        // Seçilen işlemleri kaydet
        foreach (var islemId in model.SecilenIslemler)
        {
            var hazirIslem = _context.HazirIslemler.Find(islemId);
            if (hazirIslem != null)
            {
                _context.Islemler.Add(new Islem
                {
                    AracId = arac.Id,
                    IslemAdi = hazirIslem.IslemAdi,
                    Fiyat = hazirIslem.Fiyat
                });
            }
        }

        _context.SaveChanges();
        return Json(new { success = true });
    }
    catch (Exception ex)
    {
        return BadRequest(new { success = false, message = "Hata: " + ex.Message });
    }
}

// Model Tanımı
public class KaydetModel
{
    public string Plaka { get; set; }
    public int[] SecilenIslemler { get; set; }
}

    }
}
