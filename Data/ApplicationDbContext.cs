using Microsoft.EntityFrameworkCore;
using TamirhaneMVC.Models;
namespace TamirhaneMVC.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
       

        public DbSet<Arac> Arabalar { get; set; }
        public DbSet<Islem> Islemler { get; set; }
        public DbSet<HazirIslem> HazirIslemler { get; set; }
        public DbSet<Parca> Parcalar { get; set; }
        public DbSet<IslemParca> IslemParcalar { get; set; }
        public DbSet<IslemDetay> IslemDetaylar { get; set; }
    }
}