using ERP_MaxysHC.Maxys.Model;

namespace ERP_MaxysHC.Maxys.Data.RepositoriesSQL
{
    public interface IOCDocumentosRepository
    {
        Task<IEnumerable<OCDocumentos>> GetAllOCDocumentos();

    }
}
