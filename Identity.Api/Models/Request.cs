using Identity.Api.Services.Models;

namespace Identity.Api.Models
{
    public class Request
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public DateTime CreationTime { get; set; }
        public User User { get; set; }
    }
}
