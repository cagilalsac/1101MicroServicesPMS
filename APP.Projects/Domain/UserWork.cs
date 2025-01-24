using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace APP.Projects.Domain
{
    public class UserWork : Entity
    {
        [Required]
        [StringLength(75)]
        public string UserName { get; set; }

        [StringLength(250)]
        public string Url { get; set; }

        public decimal Percentage { get; set; }

        public DateTime? EndDate { get; set; }

        public int WorkId { get; set; }

        public Work _Work { get; set; }
    }
}
