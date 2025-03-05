using CORE.APP.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APP.Projects.Domain
{
    public class Project : Entity
    {
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Name { get; set; }

        public string Description { get; set; }

        [StringLength(400)]
        public string Url { get; set; }

        public double? Version { get; set; }

        [NotMapped]
        public List<int> TagIds
        {
            get => ProjectTags.Select(pt => pt.TagId).ToList();
            set => ProjectTags = value.Select(v => new ProjectTag() { TagId = v }).ToList();
        }

        public List<Work> Works { get; set; } = new List<Work>();

        public List<ProjectTag> ProjectTags { get; set; } = new List<ProjectTag>();
    }
}
