namespace Identity.Api.Models.ViewModel
{
    public class RequestViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
