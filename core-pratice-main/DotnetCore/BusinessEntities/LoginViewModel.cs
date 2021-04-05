using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class LoginViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Email Id required")]
        [Display(Name = "Email Id")]
        public string emailid { get; set; }
        [Required(ErrorMessage = "Password required")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
