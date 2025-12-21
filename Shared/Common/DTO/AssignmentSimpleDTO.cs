using SetelaServerV3._1.Shared.Common.Interfaces;

namespace SetelaServerV3._1.Shared.Common.DTO
{
    public class AssignmentSimpleDTO : IOrderable
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int DisplayOrder { get; set; }
    }
}
