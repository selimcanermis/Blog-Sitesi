namespace MvcBlogSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Makaleler")]
    public partial class Makaleler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Makaleler()
        {
            Yorums = new HashSet<Yorum>();
            Etikets = new HashSet<Etiket>();
        }

        [Key]
        public int Makaleid { get; set; }

        [StringLength(500)]
        public string Baslik { get; set; }

        public string icerik { get; set; }

        [StringLength(500)]
        public string Foto { get; set; }

        public DateTime? Tarih { get; set; }

        public int? Kategoriid { get; set; }

        public int? Uyeid { get; set; }

        public int? Okunma { get; set; }

        public virtual Kategori Kategori { get; set; }

        public virtual Uye Uye { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Yorum> Yorums { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Etiket> Etikets { get; set; }
    }
}
