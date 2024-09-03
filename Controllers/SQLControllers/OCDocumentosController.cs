using ERP_MaxysHC.Maxys.Data.RepositoriesSQL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ERP_MaxysHC.Controllers.SQLControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OCDocumentosController : ControllerBase
    {
        private readonly IOCDocumentosRepository _repository;

        public OCDocumentosController(IOCDocumentosRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [Route("GetAllOCDocumentos")]
        public async Task<IActionResult> GetAllOCDocumentos()
        {
            return Ok(await _repository.GetAllOCDocumentos());
        }
    }
}
