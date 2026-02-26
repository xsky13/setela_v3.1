namespace SetelaServerV3._1.Application.Features.OrderingFeature.DTO
{
    public class ReorderRequestDTO
    {
        public int CourseId { get; set; }
        public List<OrderItem>? TopicSeparators { get; set; }
        public List<OrderItem>? Modules { get; set; }
        public List<OrderItem>? Exams { get; set; }
        public List<OrderItem>? Assignments { get; set; }
        public List<OrderItem>? Resources { get; set; }
    }
}
