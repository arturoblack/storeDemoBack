using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasDAL.Entities.ReservasDb.tables
{
    [Table("ImagenProducto")]
    public class ImagenProductoTable
	{
        [Key]
		public int id { get; set; }
		public int productoId { get; set; }
		public string imagenUrl { get; set; }
	}
}

