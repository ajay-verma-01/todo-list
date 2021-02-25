using Microsoft.EntityFrameworkCore;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace TodoListService.Models
{
    public partial class TodoListDBContext : DbContext
    {
        public TodoListDBContext()
        {
        }

        public TodoListDBContext(DbContextOptions<TodoListDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Todo> ToDo { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                //optionsBuilder.UseSqlite("DataSource=D:\\invw\\demo\\ToDoList\\TodoListService\\TodoListService\\App_Data\\ToDoListDB.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>(entity =>
            {
                entity.Property(e => e.id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.description)
                    .IsRequired()
                    .HasColumnName("description");

                entity.Property(e => e.isActive)
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.isCompleted).HasColumnName("isCompleted");

                entity.Property(e => e.userId)
                    .IsRequired()
                    .HasColumnName("userId");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.userId).HasColumnName("userId");

                entity.Property(e => e.emailId)
                    .IsRequired()
                    .HasColumnName("emailId");

                entity.Property(e => e.firstName).HasColumnName("firstName");

                entity.Property(e => e.isActive)
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.lastName).HasColumnName("lastName");

                entity.Property(e => e.password)
                    .IsRequired()
                    .HasColumnName("password");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
