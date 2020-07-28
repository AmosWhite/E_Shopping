using System.ComponentModel.DataAnnotations;
using DL;

namespace WebApp.Models.ViewModels.Account
{
    public class UserProfileVM
    {
        public UserProfileVM()
        {
        }

        public UserProfileVM(UserDomainModel row)
        {
            Id = row.Id;
            FirstName = row.FirstName;
            LastName = row.LastName;
            EmailAddress = row.Email;
            Password = row.Password;
        }

        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        [Required]
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}