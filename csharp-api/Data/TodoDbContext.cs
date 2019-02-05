using Microsoft.EntityFrameworkCore;

namespace MegaFpCSharpTodoBackEndTotallyNotButtCheeks.Data
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options)
            : base(options)
        { }

        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Todo>(entity =>
                {
                    entity
                        .Property(todo => todo.Description)
                        .IsRequired()
                        .HasMaxLength(1000);

                    entity
                        .HasData(new Todo(1, "dummy", true));
                });
        }
    }
}