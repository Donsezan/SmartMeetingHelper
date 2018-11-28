using System.Collections.Generic;
using Microsoft.Graph;

namespace SmartMeetingHelper.Models
{
    public class CalendarEventModel
    {
        public string Subject { get; set; }
        public string MeetingTime { get; set; }
        public IEnumerable<Attendee> Participants { get; set; }
    }
}