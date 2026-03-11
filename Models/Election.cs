
namespace OnlineVoting.Models
{
    public class Election
    {
        public int ElectionId { get; set; }
        public required string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public required string Status { get; set; }
        public List<Candidate>? Candidates { get; internal set; }
    }
}
