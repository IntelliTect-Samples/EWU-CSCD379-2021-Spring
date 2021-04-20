using System.ComponentModel.DataAnnotations;

namespace SecretSanta.Web.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name="First Name")]
        public string FirstName { get; set; } = "";

        [Required]
        [Display(Name="Last Name")]
        public string LastName { get; set; } = "";
    }
}