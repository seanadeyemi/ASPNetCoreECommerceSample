using System.ComponentModel.DataAnnotations;

namespace ASPNetCoreECommerceSample.Models
{
    public class LoginModel
    {
        // [Required]
        public string UserName { get; set; }
        [Required]
        // [StringLength(50, MinimumLength = 6)]
        // [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
