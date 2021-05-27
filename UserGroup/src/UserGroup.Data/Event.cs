using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace UserGroup.Data
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; } = "";
        public DateTime? Date { get; set; }
        public string? Location { get; set; } = "";

        [NotMapped]
        public IEnumerable<Speaker> Speakers 
        { 
            get
            {
                return EventSpeaker.SelectMany(item => item.Speakers);
            }
        } 


        public List<EventSpeaker> EventSpeaker = new();
    }
}
