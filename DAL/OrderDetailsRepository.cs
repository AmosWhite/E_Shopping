using DAL.Infrastructure;
using DAL.Infrastructure.Contract;


namespace DAL
{
    public class OrderDetailRepository : BaseRepository<OrderDetail>
    {
        public OrderDetailRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
