namespace EduSecure.Api.Models;
public class Enrollment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public Guid ClassRoomId { get; set; }
    public ClassRoom ClassRoom { get; set; } = null!;
    public Grade? Grade { get; set; }
}
