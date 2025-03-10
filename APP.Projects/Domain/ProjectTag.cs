﻿using CORE.APP.Domain;

namespace APP.Projects.Domain
{
    public class ProjectTag : Entity
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
