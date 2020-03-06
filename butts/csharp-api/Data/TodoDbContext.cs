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
                        .Property(todo => todo.TodoId)
                        .HasConversion
                        (
                            todoId => todoId.Value,
                            todoId => new TodoId(todoId)
                        );

                    entity
                        .Property(todo => todo.Description)
                        .IsRequired()
                        .HasMaxLength(1000);

                    entity
                        .HasData(
                            new Todo(new TodoId(1), "dummy", true),
                            new Todo(new TodoId(2), "another dummy", true),
                            new Todo(new TodoId(3), "A third dummy", false)
                        );
                });
        }
    }
}
