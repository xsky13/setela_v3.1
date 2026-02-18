namespace SetelaServerV3._1.Shared.Common.DTO
{
    public class CourseListingDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public List<UserSimpleDTO> Professors { get; set; }
    }
}
