using ERP_MaxysHC.Maxys.Data;
using ERP_MaxysHC.Maxys.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ERP_MaxysHC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly MaxysContext _context;

        public ClientesController(MaxysContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetAllClientes")]
        public async Task<ActionResult<IEnumerable<adCarint_admClientes>>> GetAllClientes()
        {
            return Ok(await _context.admClientes.ToListAsync());
        }
    }
}
