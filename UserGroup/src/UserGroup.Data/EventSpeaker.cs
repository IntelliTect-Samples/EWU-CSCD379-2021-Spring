using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserGroup.Data
{
    public class EventSpeaker
    {
        public int EventId { get; set; }
        public int SpeakerId { get; set; }
        public List<Speaker> Speakers = new();
    }
}
