using System.ComponentModel.DataAnnotations;

namespace SmartMeetingHelper.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PhotoId { get; set; }
        public string Email { get; set; }
        public string LastVisit { get; set; }

    }
}