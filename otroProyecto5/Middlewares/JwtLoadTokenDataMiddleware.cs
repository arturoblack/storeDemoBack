using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ReservasDAL.Helpers;
using ReservasDAL.Services.Authentication.DTOS;

namespace otroProyecto5.Middlewares
{
	public class JwtLoadTokenDataMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IConfiguration _configuration;

		public JwtLoadTokenDataMiddleware(RequestDelegate next, IConfiguration configuration)
		{
			_next = next;
			_configuration = configuration;
		}

		public async Task Invoke(HttpContext context) {
			// leer el token (cadena)
			string? authorization = context.Request.Headers["Authorization"].FirstOrDefault(); // bearer GREFDS#%RESDFDSFDSF
			if (authorization != null)
            {
				string? token = authorization.Split(" ").LastOrDefault();
				if (token != null)
                {
					JwtSecurityToken? jwt = DecodeToken(token);
					if (jwt != null)
                    {
						UserModel? user = GetUserByJWT(jwt);
						if (user != null)
						{
							context.Items["LoggedUser"] = user;
						}
                    }
				}
            }
			// agregar ub objeto de clase usuario al contexto http

			await _next(context);
		}


		private JwtSecurityToken? DecodeToken(string token) {
			AppSettings settings = new();
			_configuration.GetSection("AppSettings").Bind(settings);
			byte[] key = Encoding.ASCII.GetBytes(settings.JwtSecret);

			try
			{
				JwtSecurityTokenHandler tokenHandler = new();
				TokenValidationParameters config = new TokenValidationParameters {
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					// tiempo extra para la validacion del token
					ClockSkew = TimeSpan.Zero,
				};
				tokenHandler.ValidateToken(token, config, out SecurityToken validateToken );
				JwtSecurityToken jwt = (JwtSecurityToken)validateToken;
				return jwt;
			}
			catch
            {
				return null;
            }
		}

		private UserModel? GetUserByJWT(JwtSecurityToken jwt)
        {
			// Claim? xusername = jwt.Claims.FirstOrDefault(v => v.Type == "username");
			// string usernameString = xusername != null ? xusername.Value : null;

			string? username = jwt.Claims.FirstOrDefault(v => v.Type == "username")?.Value;
			string? names = jwt.Claims.FirstOrDefault(v => v.Type == "names")?.Value;
			string? role = jwt.Claims.FirstOrDefault(v => v.Type == "username")?.Value;
			string? photoUrl = jwt.Claims.FirstOrDefault(v => v.Type == "photoUrl")?.Value;

			if (username != null && names != null && role != null)
            {
				return new UserModel { username = username, names = names, rol = role, photoUrl = photoUrl ?? "" };
            }

			return null;
		}
	}
}

