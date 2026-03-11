namespace OnlineVoting.Models
{
    public class Candidate
    {
        internal bool IsApproved;


        public int CandidateId { get; set; } // Primary Key
        public required string Name { get; set; }
        public int Age { get; set; }
        public required string Education { get; set; }
        public required string  CITNo{ get; set; } // Unique
        public required string Party { get; set; }
        public string ? Logo { get; set; } // Nullable
        public string? ProfilePicture { get; set; } // Nullable
        public int VoteCount { get; set; } = 0; // Default Value
        public string ? Manifesto { get; set; } // Nullable
        public int ElectionId { get; set; } // Foreign Key
        public bool IsAccepted { get; internal set; }

    }
}
