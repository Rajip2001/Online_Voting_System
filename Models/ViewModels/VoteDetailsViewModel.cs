namespace OnlineVoting.Models.ViewModels
{
    public class VoteDetailsViewModel
    {
        public required string ElectionName { get; set; }
        public required string CandidateName { get; set; }
        public required string CandidatePhotoUrl { get; set; }
        public required string VoterName { get; set; }
        public required string VoterEmail { get; set; }
        public DateTime CastDateTime { get; set; }
    }

}
