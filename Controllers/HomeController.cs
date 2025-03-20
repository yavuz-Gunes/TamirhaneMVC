using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TamirhaneMVC.Data;
using TamirhaneMVC.Models;
using System.Linq;
using System.Collections.Generic;
using System;

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
            return View();
        }

        // İşlem ekleme sayfası
        public IActionResult IslemEkle(int id)
        {
            var arac = _context.Arabalar.Find(id);
            if (arac == null)
            {
                return NotFound();
            }

            ViewBag.HazirIslemler = _context.HazirIslemler.ToList();
            ViewBag.Parcalar = _context.Parcalar.Where(p => p.StokMiktari > 0).ToList();
            ViewBag.AracId = id;
            ViewBag.Plaka = arac.Plaka;

            return View();
        }

        [HttpPost]
        public IActionResult IslemEkle(IslemEkleModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.HazirIslemler = _context.HazirIslemler.ToList();
                ViewBag.Parcalar = _context.Parcalar.Where(p => p.StokMiktari > 0).ToList();
                ViewBag.AracId = model.AracId;
                ViewBag.Plaka = _context.Arabalar.Find(model.AracId)?.Plaka;
                return View(model);
            }

            // Yeni işlem oluştur
            var yeniIslem = new Islem
            {
                AracId = model.AracId,
                IslemAdi = model.IslemAdi,
                Fiyat = model.Fiyat,
                IslemDetayi = model.IslemDetayi,
                IslemTarihi = DateTime.Now
            };

            _context.Islemler.Add(yeniIslem);
            _context.SaveChanges();

            // Seçilen parçaları stoktan düş ve işleme ekle
            if (model.SecilenParcalar != null && model.SecilenParcalar.Any())
            {
                foreach (var parcaId in model.SecilenParcalar)
                {
                    var parca = _context.Parcalar.Find(parcaId);
                    if (parca != null && parca.StokMiktari > 0)
                    {
                        // Parçayı işleme ekle
                        var islemParca = new IslemParca
                        {
                            IslemId = yeniIslem.Id,
                            ParcaId = parcaId,
                            KullanilanMiktar = 1 // Varsayılan olarak 1 adet kullanıldığını kabul ediyoruz
                        };

                        _context.IslemParcalar.Add(islemParca);

                        // Stoktan düş
                        parca.StokMiktari -= 1;
                        _context.Parcalar.Update(parca);
                    }
                }

                _context.SaveChanges();
            }

            TempData["Mesaj"] = "İşlem başarıyla eklendi.";
            return RedirectToAction("Detay", "Arac", new { id = model.AracId });
        }

        [HttpGet]
        public IActionResult GetIslemler()
        {
            // Tüm işçilikleri getir, filtreleme yapma
            var islemler = _context.HazirIslemler.ToList();
                
            // Eğer hazır işlem yoksa, örnek işlemler ekle
            if (!islemler.Any())
            {
                var hazirIslemler = new List<HazirIslem>
                {
                    new HazirIslem { IslemAdi = "Yağ Değişimi", Fiyat = 500 },
                    new HazirIslem { IslemAdi = "Balata Değişimi", Fiyat = 700 },
                    new HazirIslem { IslemAdi = "Arka Fren", Fiyat = 1000 },
                    new HazirIslem { IslemAdi = "Şanzıman Tamiri", Fiyat = 3000 }
                };
                
                _context.HazirIslemler.AddRange(hazirIslemler);
                _context.SaveChanges();
                
                // Tekrar tüm işçilikleri al
                islemler = _context.HazirIslemler.ToList();
            }
            
            return Json(islemler);
        }

        [HttpGet]
        public IActionResult GetParcalar()
        {
            var parcalar = _context.Parcalar.ToList();
            
            // Eğer parça yoksa, örnek parçalar ekle
            if (!parcalar.Any())
            {
                var ornek_parcalar = new List<Parca>
                {
                    new Parca { ParcaAdi = "Motor Yağı", Fiyat = 350 },
                    new Parca { ParcaAdi = "Yağ Filtresi", Fiyat = 150 },
                    new Parca { ParcaAdi = "Polen Filtresi", Fiyat = 200 },
                    new Parca { ParcaAdi = "Yakıt Filtresi", Fiyat = 250 },
                    new Parca { ParcaAdi = "Balata", Fiyat = 500 },
                    new Parca { ParcaAdi = "Antifriz", Fiyat = 180 },
                    new Parca { ParcaAdi = "Rot Kolu", Fiyat = 420 }
                };
                
                _context.Parcalar.AddRange(ornek_parcalar);
                _context.SaveChanges();
                
                parcalar = _context.Parcalar.ToList();
            }
            
            return Json(parcalar);
        }

        [HttpGet]
        public IActionResult GetBugunIslemler()
        {
            try
            {
                // Bugünün başlangıcı (00:00:00)
                var bugun = DateTime.Today;
                var yarin = bugun.AddDays(1);

                // Bugün yapılan işlemleri getir
                var bugunAraclar = _context.Arabalar
                    .Where(a => a.GirişTarihi >= bugun && a.GirişTarihi < yarin)
                    .OrderByDescending(a => a.GirişTarihi)
                    .Take(10) // Son 10 işlem yeterli
                    .ToList();

                var gunlukIslemler = new List<BugunIslemModel>();

                foreach (var arac in bugunAraclar)
                {
                    // Araca ait işlemleri bul
                    var islemler = _context.Islemler
                        .Where(i => i.AracId == arac.Id)
                        .ToList();

                    // İşlem türlerini ve toplam tutarı hesapla
                    var islemTurleri = string.Join(", ", islemler
                        .Select(i => i.IslemAdi)
                        .Take(3)); // İlk 3 işlemi göster
                    
                    if (islemler.Count > 3)
                        islemTurleri += $" ve {islemler.Count - 3} diğer işlem";

                    var toplamTutar = islemler.Sum(i => i.Fiyat);

                    gunlukIslemler.Add(new BugunIslemModel
                    {
                        Plaka = arac.Plaka,
                        IslemTuru = islemTurleri,
                        ToplamTutar = toplamTutar
                    });
                }

                return Json(gunlukIslemler);
            }
            catch (Exception ex)
            {
                // Hata durumunda boş liste dön
                return Json(new List<BugunIslemModel>());
            }
        }

        [HttpPost]
        public IActionResult YeniHazirIslem([FromBody] YeniHazirIslemModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.IslemAdi) || model.Fiyat <= 0)
            {
                return BadRequest(new { success = false, message = "İşlem adı ve fiyat girmelisiniz." });
            }

            try
            {
                var yeniIslem = new HazirIslem
                {
                    IslemAdi = model.IslemAdi,
                    Fiyat = model.Fiyat
                };

                _context.HazirIslemler.Add(yeniIslem);
                _context.SaveChanges();

                return Json(new { success = true, data = yeniIslem });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult YeniParca([FromBody] YeniParcaModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.ParcaAdi) || model.Fiyat <= 0)
            {
                return BadRequest(new { success = false, message = "Parça adı ve fiyat girmelisiniz." });
            }

            try
            {
                var yeniParca = new Parca
                {
                    ParcaAdi = model.ParcaAdi,
                    Fiyat = model.Fiyat
                };

                _context.Parcalar.Add(yeniParca);
                _context.SaveChanges();

                return Json(new { success = true, data = yeniParca });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Hata: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Kaydet([FromBody] KaydetModel model)
        {
            try {
                // Geçerlilik kontrolü
                if (string.IsNullOrWhiteSpace(model.Plaka) || 
                    (model.IslemIdleri == null || !model.IslemIdleri.Any()) && 
                    (model.ParcaIdleri == null || !model.ParcaIdleri.Any()))
                {
                    return Json(new { success = false, message = "Lütfen en az bir işçilik veya parça seçiniz" });
                }

                // Plaka veri tabanında var mı diye kontrol et
                var arac = _context.Arabalar.FirstOrDefault(a => a.Plaka == model.Plaka.ToUpper().Trim());
                
                // Yoksa yeni araç oluştur
                if (arac == null)
                {
                    arac = new Arac
                    {
                        Plaka = model.Plaka.ToUpper().Trim(),
                        GirişTarihi = DateTime.Now
                    };
                    
                    _context.Arabalar.Add(arac);
                    _context.SaveChanges();
                }
                
                // Toplam fiyat hesaplama
                decimal toplamFiyat = 0;
                
                // Ana işlem kaydı oluştur
                var anaIslem = new Islem
                {
                    AracId = arac.Id,
                    IslemAdi = "Tamirhane İşlemi",
                    Fiyat = 0,
                    IslemTarihi = DateTime.Now,
                    IslemDetayi = !string.IsNullOrWhiteSpace(model.Notlar) ? model.Notlar : "Genel bakım ve onarım"
                };
                
                _context.Islemler.Add(anaIslem);
                _context.SaveChanges();
                
                // Seçilen işlemleri kaydet
                if (model.IslemIdleri != null && model.IslemIdleri.Any())
                {
                    foreach (var islemId in model.IslemIdleri)
                    {
                        var hazirIslem = _context.HazirIslemler.Find(islemId);
                        if (hazirIslem != null)
                        {
                            // İşlemi ana işleme bağla
                            var islemDetay = new IslemDetay
                            {
                                IslemId = anaIslem.Id,
                                HazirIslemId = hazirIslem.Id,
                                IslemAdi = hazirIslem.IslemAdi,
                                Fiyat = hazirIslem.Fiyat
                            };
                            
                            _context.IslemDetaylar.Add(islemDetay);
                            
                            // Ana işlemin fiyatına ekle
                            toplamFiyat += hazirIslem.Fiyat;
                        }
                    }
                    
                    // Toplam fiyatı güncelle
                    anaIslem.Fiyat += toplamFiyat;
                    _context.Islemler.Update(anaIslem);
                }
                
                // Seçilen parçaları kaydet
                if (model.ParcaIdleri != null && model.ParcaIdleri.Any())
                {
                    decimal parcaToplamFiyat = 0;
                    
                    foreach (var parcaId in model.ParcaIdleri)
                    {
                        var parca = _context.Parcalar.Find(parcaId);
                        if (parca != null && parca.StokMiktari > 0)
                        {
                            // Parçayı ekle ve stoktan düş
                            var islemParca = new IslemParca
                            {
                                IslemId = anaIslem.Id,
                                ParcaId = parcaId,
                                KullanilanMiktar = 1
                            };
                            
                            _context.IslemParcalar.Add(islemParca);
                            
                            // Fiyatı güncelle
                            parcaToplamFiyat += parca.Fiyat;
                            
                            // Stok güncelle
                            parca.StokMiktari -= 1;
                            _context.Parcalar.Update(parca);
                        }
                    }
                    
                    // Ana işlemin fiyatına parça fiyatlarını ekle
                    anaIslem.Fiyat += parcaToplamFiyat;
                    toplamFiyat += parcaToplamFiyat;
                    _context.Islemler.Update(anaIslem);
                }
                
                _context.SaveChanges();
                
                return Json(new { 
                    success = true, 
                    message = "İşlem başarıyla kaydedildi", 
                    toplamFiyat = toplamFiyat.ToString("N2"), 
                    aracId = arac.Id, 
                    plaka = arac.Plaka 
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
                Console.WriteLine("StackTrace: " + ex.StackTrace);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                }
                
                return Json(new { success = false, message = "Hata oluştu: " + ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetIstatistikler()
        {
            try
            {
                // Bugünün başlangıcı ve sonu
                var bugun = DateTime.Today;
                var yarin = bugun.AddDays(1);
                
                // Bugün gelen araç sayısı
                var bugunGelenAracSayisi = _context.Arabalar
                    .Count(a => a.GirişTarihi >= bugun && a.GirişTarihi < yarin);
                
                // Tüm araç sayısı
                var toplamAracSayisi = _context.Arabalar.Count();
                
                // Bugünkü kazanç
                var bugunKazanc = _context.Islemler
                    .Where(i => i.IslemTarihi >= bugun && i.IslemTarihi < yarin)
                    .Sum(i => (decimal?)i.Fiyat) ?? 0;
                
                // Devam eden işlem sayısı (son 7 gün içinde başlayan işlemler)
                var devamEdenIslemSayisi = _context.Islemler
                    .Where(i => i.IslemTarihi >= bugun.AddDays(-7) && i.IslemTarihi < yarin)
                    .GroupBy(i => i.AracId)
                    .Count();
                
                // Tamamlanan işlem sayısı (bugün tamamlanan)
                var tamamlananIslemSayisi = _context.Islemler
                    .Count(i => i.IslemTarihi >= bugun && i.IslemTarihi < yarin);
                
                return Json(new 
                { 
                    bugunGelenAracSayisi, 
                    toplamAracSayisi, 
                    bugunKazanc, 
                    devamEdenIslemSayisi, 
                    tamamlananIslemSayisi 
                });
            }
            catch
            {
                // Hata durumunda varsayılan değerler
                return Json(new 
                { 
                    bugunGelenAracSayisi = 0, 
                    toplamAracSayisi = 0, 
                    bugunKazanc = 0, 
                    devamEdenIslemSayisi = 0, 
                    tamamlananIslemSayisi = 0 
                });
            }
        }
    }

    public class KaydetModel
    {
        public string Plaka { get; set; }
        public int[] IslemIdleri { get; set; }
        public int[] ParcaIdleri { get; set; }
        public string Notlar { get; set; }
    }

    public class YeniHazirIslemModel
    {
        public string IslemAdi { get; set; }
        public decimal Fiyat { get; set; }
    }

    public class YeniParcaModel
    {
        public string ParcaAdi { get; set; }
        public decimal Fiyat { get; set; }
    }

    public class BugunIslemModel
    {
        public string Plaka { get; set; }
        public string IslemTuru { get; set; }
        public decimal ToplamTutar { get; set; }
    }

    public class IslemEkleModel
    {
        public int AracId { get; set; }
        public string IslemAdi { get; set; }
        public decimal Fiyat { get; set; }
        public string IslemDetayi { get; set; }
        public int[] SecilenParcalar { get; set; }
    }
}
