using System.ComponentModel.DataAnnotations;

namespace OnlineVoting.Models
{
    public class Administrator
    {
        [Key]
        public int AdminId { get; set; }

        public int UserId { get; set; }
    }
}
