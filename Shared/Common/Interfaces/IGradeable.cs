using SetelaServerV3._1.Shared.Common.DTO;

namespace SetelaServerV3._1.Shared.Common.Interfaces
{
    public interface IGradeable
    {
        public int Id { get; set; }
        public GradeSimpleDTO? Grade { get; set; }
        public int MaxGrade { get; set; }
    }
}
