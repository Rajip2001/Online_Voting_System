namespace OnlineVoting.Models.ViewModels
{
    public class ResultViewModel
    {
        public required Election ElectionDetails { get; set; }
        public required Candidate Winner {  get; set; }
        public int TotalVotes {  get; set; }
        public double TurnoutPercentage { get; set; }
    }
}
