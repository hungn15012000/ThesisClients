using System.ComponentModel.DataAnnotations;

namespace MiddleWare.Models.User
{
    public class UserDto :LoginUserDto
    {
      
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string LastName { get; set; }
       
        public string Role { get; set; }

    }
   
}
