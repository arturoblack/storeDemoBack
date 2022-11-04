using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ReservasDAL.Helpers;
using ReservasDAL.Services.Authentication.DTOS;

namespace otroProyecto5.Controllers.v1.Auth
{
	[Route("/api/v1/auth")]
	public class AuthController: ControllerBase
	{
		private readonly IConfiguration _configuration;
		public AuthController(
			IConfiguration configuration
		)
		{
			_configuration = configuration;
		}

		[HttpPost]
		[Produces("application/json")]
		[Route("login")]
		public async Task<ActionResult> LoginAsync([FromBody] LoginRequest model)
        {
			// Asumimos que usuario y password son correctos
			UserModel user = new UserModel
			{
				username = model.username,
				names = "Pepito",
				photoUrl = "sadsad",
				rol = "client"
			};

			string token = GenerateToken(user);
			// Generar el token
			return Ok(new {token});
        }

		private string GenerateToken(UserModel model)
		{
			AppSettings settings = new();
			_configuration.GetSection("AppSettings").Bind(settings);
			string keyString = settings.JwtSecret;
			byte[] key = Encoding.ASCII.GetBytes(keyString);

			ClaimsIdentity claims = new ClaimsIdentity();
			claims.AddClaim(
				new Claim("username", model.username)
				);
			claims.AddClaim(
				new Claim("names", model.names)
				);
			claims.AddClaim(
				new Claim("role", model.rol)
				);
			claims.AddClaim(
				new Claim("photoUrl", model.photoUrl)
				);

			var tokenDescriptor = new SecurityTokenDescriptor {
				Subject = claims,
				Expires = DateTime.UtcNow.AddHours(2), // 2*100*60*60

				SigningCredentials = new SigningCredentials(
					new SymmetricSecurityKey(key),
					SecurityAlgorithms.HmacSha256Signature
					)
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var createdToken = tokenHandler.CreateToken(tokenDescriptor);
			string token = tokenHandler.WriteToken(createdToken);

			return token;
		}
	}
	
}

