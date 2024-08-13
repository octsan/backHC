using ERP_MaxysHC.Maxys.Model;

namespace ERP_MaxysHC.Maxys.Data.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UsersModel>> GetAllUsers();
        Task<bool> UpdateUser(UsersModel userModel);
    }
}
