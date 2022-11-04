using System;
using Microsoft.EntityFrameworkCore;
using ReservasDAL.Contexts;
using ReservasDAL.Entities.ReservasDb.tables;
using ReservasDAL.Services.Products.Dtos;

namespace ReservasDAL.Services.Products
{
	public class BrandService
	{
		private readonly ReservasContext _db;

		public BrandService(ReservasContext db)
		{
			_db = db;
		}

		public async Task<List<MarcaTable>> GetAllAsync()
        {
			return await _db.Marcas.ToListAsync();
        }

		public async Task<MarcaTable> CreateAsync(BrandRequestBody body)
        {
			MarcaTable marca = new MarcaTable { nombre = body.name };
			_db.Marcas.Add(marca);
			await _db.SaveChangesAsync();
			return marca;
        }

		public async Task<bool> AddImageAsync(int id, string path)
		{
			MarcaTable? marca = await _db.Marcas.FindAsync(id);
			if (marca == null)
            {
				throw new Exception("No existe la marca");
            }
			marca.logoUrl = path;
			int res = await _db.SaveChangesAsync();

			// operador ternario
			return (res > 0)? true: false;
		}
	}
}

