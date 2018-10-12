using System.ComponentModel.DataAnnotations;

namespace BlogDemo.Web.Models
{
    public class LoginViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
