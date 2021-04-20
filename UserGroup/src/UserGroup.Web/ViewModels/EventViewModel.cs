using System;
using System.ComponentModel.DataAnnotations;

namespace UserGroup.Web.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime Date { get; set; }
        public string Location { get; set; } = "";
        [Display(Name = "Speaker")]
        public int SpeakerId { get; set; }
    }
}