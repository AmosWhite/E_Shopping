

using BLL.Interface;
using DAL;
using DAL.Infrastructure.Contract;
using DL;

namespace BLL
{
    public class UserBussiness : IUserBussiness
    {
        private readonly UserRepository userRepository;
        private readonly IUnitOfWork unitOfWork;
        public UserBussiness(IUnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;
            this.userRepository = new UserRepository(unitOfWork);
        }

        public UserDomainModel GetUser(UserDomainModel model)
        {
            User userDTO = userRepository.SingleOrDefault(x => x.Email == model.Email && x.Password == model.Password);
            UserDomainModel userDM = new UserDomainModel();

            if (userDTO != null)
            {
                userDM.Id = userDTO.Id;
                userDM.PhoneNumber = userDTO.PhoneNumber;
                userDM.Email = userDTO.Email;
                userDM.RoleId = userDTO.RoleId;
                userDM.Address = userDTO.Address;
                userDM.UserRoleName = userDTO.tblRole.Name;
                userDM.FirstName = userDTO.FirstName;
                userDM.LastName = userDTO.LastName;
            }
           
            return userDM;
        }

        public UserDomainModel GetUserById(int id)
        {
            User userDTO = userRepository.SingleOrDefault(x => x.Id == id);

            UserDomainModel userDM = new UserDomainModel();
            userDM.Id = userDTO.Id;
            userDM.FirstName = userDTO.FirstName;
            userDM.PhoneNumber = userDTO.PhoneNumber;
            userDM.Email = userDTO.Email;
            userDM.RoleId = userDTO.RoleId;
            userDM.Address = userDTO.Address;
            userDM.UserRoleName = userDTO.tblRole.Name;
            return userDM;
        }

        public bool IsEmailExist(string email)
        {
            if (userRepository.Exists(x => x.Email == email))
                return true;

            else
                return false;
        }

        public UserDomainModel SaveUser(UserDomainModel model)
        {
            User userDTO = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber

            };
            userDTO = userRepository.Insert(userDTO);

            UserDomainModel UserDM = new UserDomainModel()
            {
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                Address = userDTO.Address,
                PhoneNumber = userDTO.PhoneNumber

            };

            return UserDM;
        }

        public bool EditUser(UserDomainModel model)
        {
            if(userRepository.IsEmailUnique(model.Id, model.Email))
            {
                User userDTO = userRepository.SingleOrDefault(x => x.Id == model.Id);
                userDTO.FirstName = model.FirstName;
                userDTO.LastName = model.LastName;
                userDTO.Email = model.Email;

                if (!string.IsNullOrWhiteSpace(model.Password))
                    userDTO.Password = model.Password;

                userRepository.Update(userDTO);

                return true;
            }
            else
            {
                return false;
            }
            
}

        public UserDomainModel GetUserByEmail(string email)
        {
            User userDTO = userRepository.SingleOrDefault(x => x.Email == email);

            UserDomainModel userDM = new UserDomainModel();
            userDM.Id = userDTO.Id;
            userDM.FirstName = userDTO.FirstName;
            userDM.PhoneNumber = userDTO.PhoneNumber;
            userDM.Email = userDTO.Email;
            userDM.RoleId = userDTO.RoleId;
            userDM.Address = userDTO.Address;
            userDM.UserRoleName = userDTO.tblRole.Name;
            return userDM;
        }
    }
}
