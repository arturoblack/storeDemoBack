using System;
using Microsoft.EntityFrameworkCore;
using ReservasDAL.Contexts;
using ReservasDAL.Entities.ReservasDb.tables;
using ReservasDAL.Services.Products.Dtos;

namespace ReservasDAL.Services.Products
{
	public class ProductService
	{
        private readonly ReservasContext _db;

        public ProductService(ReservasContext db)
        {
            _db = db;
        }
        public async Task<List<ProductoTable>> GetAllAsync()
        {
            return await _db.Productos.Include(p => p.imagenes)
                .ToListAsync();
        }

        public async Task<ProductoTable> CreateAsync(ProductRequestBody body)
        {
            ProductoTable product = new ProductoTable {
                nombre = body.name,
                marcaId = body.marcaId,
                precioVenta = body.price,
                codigo = body.codigo,
                sku = body.sku
            };
            await _db.Productos.AddAsync(product);
            int res = await _db.SaveChangesAsync();
            if (res > 0)
                return product;
            throw new Exception("No fue posible agregar el producto");
        }

        public async Task<bool> AddImageAsync(int id, string path)
        {
            ProductoTable? product = await _db.Productos.FindAsync(id);
            if (product == null)
            {
                throw new Exception("No existe el producto");
            }
            ImagenProductoTable image = new ImagenProductoTable
            {
                imagenUrl = path,
                productoId = product.id
            };
            await _db.ImagenesProducto.AddAsync(image);
            int res = await _db.SaveChangesAsync();

            // operador ternario
            return (res > 0) ? true : false;
        }
    }
}

