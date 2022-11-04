using System;
namespace ReservasDAL.Services.Products.Dtos
{
	public class ProductRequestBody
	{
        public int id { get; set; }
        public int marcaId { get; set; }
        public string name { get; set; }
        public string codigo { get; set; }
        public decimal price { get; set; }
        public string sku { get; set; }
    }
}

