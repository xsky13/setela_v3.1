using SetelaServerV3._1.Domain.Entities;
using SetelaServerV3._1.Domain.Enums;
using SetelaServerV3._1.Shared.Common.DTO;

namespace SetelaServerV3._1.Application.Features.GradeFeature.DTO
{
    public class GradeDTO
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public GradeParentType ParentType { get; set; }
        public int ParentId { get; set; }
        public int SysUserId { get; set; }
        public UserSimpleDTO SysUser { get; set; }
    }
}
