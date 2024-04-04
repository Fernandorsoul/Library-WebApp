namespace Library_API.Models
{
    public class User
    {
        public int Id { get;set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string EmailConfirmed { get; set; } = string.Empty;
    }
}
