
using DAL.Infrastructure;
using DAL.Infrastructure.Contract;
using System.Linq;

namespace DAL
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork){}

        public bool IsEmailUnique(int id, string email)
        {
            if (dbSet.Where(x => x.Id != id).Any(x => x.Email == email))
               
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
