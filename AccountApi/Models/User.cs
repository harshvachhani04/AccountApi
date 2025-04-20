using System.ComponentModel.DataAnnotations;

namespace AccountApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Username { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Password and Confirm Password not match")]
        public string ConfirmPassword { get; set; }
        [AllowedValues("user", "admin")]
        public string Role { get; set; }
    }
}
