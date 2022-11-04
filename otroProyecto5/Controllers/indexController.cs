using System;
using Microsoft.AspNetCore.Mvc;
using ReservasDAL.Entities.ReservasDb.tables;
using ReservasDAL.Services.Products;

namespace otroProyecto5.Controllers
{
    [Route("/")]
    public class indexController: ControllerBase
    {
        public indexController()
        {
        }

        [HttpGet]
        [Route("")]
        public ActionResult GetAllAsync()
        {
            return Ok(new {status = "Ok"});
        }
    }
}

