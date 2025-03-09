using CORE.APP.Features;

namespace APP.UserWorks.Features.Works
{
    public class WorkQueryResponse : QueryResponse
    {
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; }
    }
}
