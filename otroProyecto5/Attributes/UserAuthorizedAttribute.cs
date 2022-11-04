using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ReservasDAL.Services.Authentication.DTOS;

namespace otroProyecto5.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class UserAuthorizedAttribute: Attribute, IAuthorizationFilter
	{
        public void OnAuthorization(AuthorizationFilterContext context)
        {
			UserModel? user = (UserModel?)context.HttpContext.Items["LoggedUser"];
			if (user == null)
            {
				context.Result = new JsonResult(new {message = "Usuario no autorizado."}) {
					StatusCode = StatusCodes.Status401Unauthorized
				};
            }
		}
    }
}

