using ERP_MaxysHC.Maxys.Model;

namespace ERP_MaxysHC.Maxys.Data.Repositories
{
    public interface IAdmClientesRepository
    {
        Task<IEnumerable<adCarint_admClientes>> GetAllAdmClientes();
    }
}
