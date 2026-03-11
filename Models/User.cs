namespace OnlineVoting.Models
{
    public class User
    {
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string CITNo { get; set; }
        public DateTime DOB { get; set; }
        public required string Role { get; set; } 
        public bool IsCandidate {  get; set; }
        public bool IsAdmin { get; internal set; }

    }
}
