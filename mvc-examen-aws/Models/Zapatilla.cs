using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc_examen_aws.Models
{
    [Table("ZAPATILLAS")]
    public class Zapatilla
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("IDPRODUCTO")]
        public int IdProducto { get; set; }

        [Column("NOMBRE")]
        public string? Nombre { get; set; }

        [Column("DESCRIPCION")]
        public string? Descripcion { get; set; }

        [Column("IMAGEN")]
        public string? Imagen { get; set; }
    }
}
