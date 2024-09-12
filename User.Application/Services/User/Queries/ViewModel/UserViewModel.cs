namespace User.Application.Services.User.Queries.ViewModel
{
    public class UserViewModel
    {
        public Guid GuidId { get; set; }
        public string? PersonName { get; set; }
        public string? PersonEmail { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
