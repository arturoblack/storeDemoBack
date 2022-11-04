using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasDAL.Entities.ReservasDb.tables
{
    [Table("Producto")]
    public class ProductoTable
	{
		public int id { get; set; }
        public int marcaId { get; set; }
        public string nombre { get; set; }
        public string codigo { get; set; }
		public decimal precioVenta { get; set; }
        public string sku { get; set; }

        [ForeignKey("productoId")]
        public List<ImagenProductoTable> imagenes { get; set; }
    }
}

