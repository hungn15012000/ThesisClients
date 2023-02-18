using Microsoft.AspNetCore.Identity;

namespace MiddleWare.Data
{
    public class ApiUser:IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
