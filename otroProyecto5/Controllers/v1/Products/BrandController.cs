using System;
using Microsoft.AspNetCore.Mvc;
using otroProyecto5.Attributes;
using otroProyecto5.ResponseData;
using ReservasDAL.Contexts;
using ReservasDAL.Entities.ReservasDb.tables;
using ReservasDAL.Services.Authentication.DTOS;
using ReservasDAL.Services.Products;
using ReservasDAL.Services.Products.Dtos;

namespace otroProyecto5.Controllers.v1.Products
{
	[Route("/api/v1/marcas")]
    // [UserAuthorized]
	public class BrandController: ControllerBase
	{
		private readonly ILogger<BrandController> _logger;
		private readonly IWebHostEnvironment _webHostEnvironment;
		//private readonly ReservasContext _db;
		private readonly BrandService _brandService;

		public BrandController(
			ILogger<BrandController> logger,
			IWebHostEnvironment webHostEnvironment,
		ReservasContext context
		)
		{
			_logger = logger;
			_webHostEnvironment = webHostEnvironment;
			//_db = context;
			_brandService = new BrandService(context);
		}

		[HttpGet]
		[Produces("application/json")]
		[Route("")]
		public async Task<ActionResult<List<MarcaTable>>> GetAllAsync()
        {
			List<MarcaTable> marcas = await _brandService.GetAllAsync();
			return Ok(marcas);
        }

		[HttpPost]
		[Produces("application/json")]
		[Route("")]
		//[UserAuthorized]
		public async Task<ActionResult<MarcaTable>> CreateAsync(
			[FromBody] BrandRequestBody body)
		{
			MarcaTable marca = await _brandService.CreateAsync(body);
			return Ok(marca);
		}

		[HttpPut]
		[Produces("application/json")]
		[Route("{marcaId}/agregar-imagen")]
		[ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		//[UserAuthorized]
		public async Task<ActionResult<object>> AddImageAsync(
			[FromRoute] int marcaId, [FromForm] FileImage body)
		{
			//copiar la imagen y generar el path
			try
            {
				string path = await CopyImageAsync(body.imageFile, marcaId);

				bool isOk = await _brandService.AddImageAsync(marcaId, path);
				return isOk? Ok(new {imageUrl = path}): NotFound();
            } catch(Exception ex)
            {
				var err = new ErrorResponse {
					status = 20001,
					message = ex.Message
				};
				return Ok(err);
            }
		}

		private async Task<string> CopyImageAsync(IFormFile image, int id)
		{

			List<string> aceptedMimeTypes = new List<string> {
				"image/jpg", "image/jpeg", "image/png" };
			string mimetype = image.ContentType;
			if (aceptedMimeTypes.Where(m => m == mimetype).Count() <= 0)
			{
				throw new Exception("Tipo de archivo invalido");
			}
			long size = image.Length;
			if (size > 2048000)//2MB
            {
				throw new Exception("No archivos mayores a 2 MB");
			}
			else if (size > 0)
			{
				string ext = image.FileName.Split('.').Last();
				string wwwroot = _webHostEnvironment.WebRootPath;
				string nameImage = $"marca-{id}.{ext}";
				string file = Path.Combine(wwwroot, "uploads", "brands", nameImage);
				//System.IO.File.Delete(file);
				using (var stream = System.IO.File.Create(file))
				{
					await image.CopyToAsync(stream);
				}
				return $"/uploads/brands/{nameImage}";
			}
			throw new Exception("archivo invalido");
		}
	}

	public class FileImage
    {
		public IFormFile imageFile { get; set; }
    }
}

