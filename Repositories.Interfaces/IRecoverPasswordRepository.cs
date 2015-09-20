using Domain.Data.Core.MongoDb;
using Domain.Data.Identity;

namespace Repositories.Interfaces
{
    public interface IRecoverPasswordRepository : IRepository<RecoverPassword>
    {
    }
}
