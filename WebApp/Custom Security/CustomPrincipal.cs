using System.Security.Principal;


namespace WebApp.Custom_Security
{
    public class CustomPrincipal : IPrincipal
    {
        public CustomPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
        }

        public IIdentity Identity
        {
            get;
            private set;
        }

        public bool IsInRole(string role)
        {
            if (role == RoleName)
                return true;
            else
                return false;

        }


        public int Id { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
    }
}