using System;
namespace ReservasDAL.Services.Authentication.DTOS
{
	public class LoginRequest
	{
		public string username { get; set; }
		public string password { get; set; }
	}
}

