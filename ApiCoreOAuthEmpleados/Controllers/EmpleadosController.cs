using ApiCoreOAuthEmpleados.Models;
using ApiCoreOAuthEmpleados.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ApiCoreOAuthEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private RepositoryEmpleados repo;

        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Empleado>>>
            GetEmpleados()
        {
            return await this.repo.GetEmpleadosAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Empleado>>
            FindEmpleado(int id)
        {
            return await this.repo.FindEmpleadoAsync(id);
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<Empleado>>
            PerfilEmpleado()
        {
            //INTERNAMENTE, CUANDO RECIBIMOS EL TOKEN 
            //EL USUARIO ES VALIDADO Y ALMACENA DATOS 
            //COMO HttpContext.User.Identity.IsAuthenticated
            //COMO HEMOS INCLUIDO LA KEY DE LOS Claims, 
            //AUTOMATICAMENTE TAMBIEN TENEMOS DICHOS CLAIMS
            //COMO EN LAS APLICACIONES MVC
            Claim claim = HttpContext.User
                .FindFirst(x => x.Type == "UserData");
            //RECUPERAMOS EL JSON DEL EMPLEADO
            string jsonEmpleado = claim.Value;
            Empleado empleado =
                JsonConvert.DeserializeObject<Empleado>(jsonEmpleado);
            return empleado;
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Empleado>>>
            CompisCurro()
        {
            string jsonEmpleado =
                HttpContext.User.FindFirst(x => x.Type == "UserData")
                .Value;
            Empleado empleado =
                JsonConvert.DeserializeObject<Empleado>(jsonEmpleado);
            List<Empleado> compis =
                await this.repo.GetCompisDepartamentoAsync
                (empleado.IdDepartamento);
            return compis;
        }
    }
}
