using System.ComponentModel.DataAnnotations;

namespace UserGroup.Web.ViewModels
{
    public class SpeakerViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name="First Name")]
        public string FirstName { get; set; } = "";

        [Required]
        [Display(Name="Last Name")]
        public string LastName { get; set; } = "";
        [Required]
        [EmailAddress]
        [Display(Name="Email")]
        public string EmailAddress  { get; set; } = "";
        public string FullName { get => $"{FirstName} {LastName}"; }
    }
}