using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace WebApplication1.Domain
{
    public class Todo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
