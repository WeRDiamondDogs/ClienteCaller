using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static CallerPR.Models.ClienteModel;


namespace CallerPR.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class ClienteController : Controller
    {
        [ActionName("ListarClientes")]
        [HttpGet]
        public IActionResult ListarClientes()
        {
            try
            {
                DataTable dt = Instancia._listarClientes();
                return Ok(JsonConvert.SerializeObject(dt));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [ActionName("CrearCliente")]
        [HttpPost]
        public IActionResult CrearCliente([FromBody]dynamic obj)
        {
            try
            {
                string nombre = obj.Nombres;
                string apellidos = obj.Apellidos;
                string fechaNacimiento = obj.FechaNacimiento;//yyyy-mm-dd
                DataTable dt = Instancia._crearCliente(nombre,apellidos,fechaNacimiento);
                return Ok(JsonConvert.SerializeObject(dt));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [ActionName("ObtenerClientePorID")]
        [HttpGet]
        public IActionResult ObtenerClientePorID(int prmID)
        {
            try
            {
                DataTable dt = Instancia._obtenerClientePorID(prmID);
                return Ok(JsonConvert.SerializeObject(dt));
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

    }
}
