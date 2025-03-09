using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace APP.UserWorks.Domain
{
    public class UserWork : Entity
    {
        public int UserId { get; set; }

        public int WorkId { get; set; }

        [StringLength(400)]
        public string Url { get; set; }

        public double Percentage { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
