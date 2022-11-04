using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservasDAL.Entities.ReservasDb.tables
{
	[Table("Marca")]
	public class MarcaTable
	{
		[Key]
		public int id { get; set; }
		public string nombre { get; set; }
		public string? logoUrl { get; set; }
	}
}
