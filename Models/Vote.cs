namespace OnlineVoting.Models
{
    public class Vote
    {
        public int VoteId { get; set; } // Primary Key
        public int VoterId { get; set; } // Foreign Key
        public int CandidateId { get; set; } // Foreign Key
        public int ElectionId { get; set; } // Foreign Key
        public DateTime CastDateTime { get; set; } // Date and Time of the vote
        public int? UserId { get; internal set; }
        public DateTime VotedAt { get; internal set; }
    }
}
