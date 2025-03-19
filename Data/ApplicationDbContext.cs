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
    }
}