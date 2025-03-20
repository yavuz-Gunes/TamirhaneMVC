using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TamirhaneMVC.Models
{
    public class IslemParca
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Islem")]
        public int IslemId { get; set; }
        public Islem Islem { get; set; }

        [ForeignKey("Parca")]
        public int ParcaId { get; set; }
        public Parca Parca { get; set; }

        [Required]
        [Range(1, 1000)]
        public int KullanilanMiktar { get; set; } = 1;
    }
} 