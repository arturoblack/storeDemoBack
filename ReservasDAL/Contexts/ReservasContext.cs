using System;
using Microsoft.EntityFrameworkCore;
using ReservasDAL.Entities.ReservasDb.tables;

namespace ReservasDAL.Contexts
{
	public class ReservasContext: DbContext
	{
		public ReservasContext(
			DbContextOptions<ReservasContext> options
			) : base(options)
		{
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<MarcaTable> Marcas { get; set; }
        public DbSet<ProductoTable> Productos { get; set; }
        public DbSet<ImagenProductoTable> ImagenesProducto { get; set; }
    }
}

