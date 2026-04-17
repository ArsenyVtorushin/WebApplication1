using Microsoft.EntityFrameworkCore;
using WebApplication1.Domain;

namespace WebApplication1.Repository
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Todo> Todos { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

                entity.Property(u => u.Login)
                .HasColumnName("login")
                .HasMaxLength(100)
                .IsRequired();

                entity.Property(u => u.Password)
                .HasColumnName("password")
                .IsRequired();

                entity.HasIndex(u => u.Login)
                .IsUnique()
                .HasDatabaseName("users_unique");
            });

            modelBuilder.Entity<Todo>(entity =>
            {
                entity.ToTable("todos");
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

                entity.Property(t => t.Name)
                .HasColumnName("name")
                .HasMaxLength(225)
                .IsRequired();

                entity.Property(t => t.Description)
                .HasColumnName("description");

                entity.Property(t => t.IsCompleted)
                .HasColumnName("iscompleted")
                .HasDefaultValue(false)
                .IsRequired();

                entity.Property(t => t.UserId)
                .HasColumnName("userid")
                .IsRequired();

                entity.HasOne(t => t.User)
                .WithMany(u => u.todos)
                .HasForeignKey(t => t.UserId)
                .HasConstraintName("todos_user_fk")
                .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
