using Gyomei.Models;

namespace Gyomei.Repositories
{
    public class RepositoryBase
    {
        GyomeiDbContext dbContext;

        public RepositoryBase(GyomeiDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
    }
}
