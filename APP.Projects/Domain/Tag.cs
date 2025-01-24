using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace APP.Projects.Domain
{
    public class Tag : Entity
    {
        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        public List<ProjectTag> _ProjectTags { get; set; } = new List<ProjectTag>();
    }
}
