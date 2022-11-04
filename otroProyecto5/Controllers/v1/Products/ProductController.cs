using System;
using Microsoft.AspNetCore.Mvc;
using otroProyecto5.Attributes;
using otroProyecto5.ResponseData;
using otroProyecto5.Utils;
using ReservasDAL.Contexts;
using ReservasDAL.Entities.ReservasDb.tables;
using ReservasDAL.Services.Products;
using ReservasDAL.Services.Products.Dtos;

namespace otroProyecto5.Controllers.v1.Products
{
	[Route("/api/v1/products")]
	public class ProductController : ControllerBase
	{

		private readonly ILogger<ProductController> _logger;
		private readonly IWebHostEnvironment _webHostEnvironment;
		//private readonly ReservasContext _db;
		private readonly ProductService _productService;

		public ProductController(
			ILogger<ProductController> logger,
			IWebHostEnvironment webHostEnvironment,
			ReservasContext context
		)
		{
			_logger = logger;
			_webHostEnvironment = webHostEnvironment;
			//_db = context;
			_productService = new ProductService(context);
		}

        [HttpGet]
        [Produces("application/json")]
        [Route("")]
        public async Task<ActionResult<List<ProductoTable>>> GetAllAsync()
        {
            List<ProductoTable> marcas = await _productService.GetAllAsync();
            return Ok(marcas);
        }

        [HttpPost]
        [Produces("application/json")]
        [Route("")]
        //[UserAuthorized]
        public async Task<ActionResult<ProductRequestBody>> CreateAsync(
            [FromBody] ProductRequestBody body)
        {
            ProductoTable product = await _productService.CreateAsync(body);
            return Ok(product);
        }

        [HttpPut]
        [Produces("application/json")]
        [Route("{productId}/agregar-imagen")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[UserAuthorized]
        public async Task<ActionResult<object>> AddImageAsync(
            [FromRoute] int productId, [FromForm] FileImage body)
        {
            //copiar la imagen y generar el path
            try
            {
                string path = await CopyTools.CopyImageAsync(
                    body.imageFile,
                    "products",
                    _webHostEnvironment.WebRootPath
                    );
                   
                bool isOk = await _productService.AddImageAsync(productId, path);
                return isOk ? Ok(new { imageUrl = path }) : NotFound();
            }
            catch (Exception ex)
            {
                var err = new ErrorResponse
                {
                    status = 20001,
                    message = ex.Message
                };
                return Ok(err);
            }
        }
    }
}

