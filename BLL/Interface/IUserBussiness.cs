

using DL;

namespace BLL.Interface
{
    public interface IUserBussiness
    {
        /// <summary>
        /// ...Retrieve User Detail from db...
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        UserDomainModel GetUser(UserDomainModel model);

        UserDomainModel GetUserById(int id);

        bool IsEmailExist(string email);

        UserDomainModel SaveUser(UserDomainModel model);

        bool EditUser(UserDomainModel model);

        UserDomainModel GetUserByEmail(string email);

    }
}
