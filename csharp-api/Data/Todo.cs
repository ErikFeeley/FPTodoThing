using System;
using System.Linq.Expressions;
using static MegaFpCSharpTodoBackEndTotallyNotButtCheeks.Controllers.TodosController;

namespace MegaFpCSharpTodoBackEndTotallyNotButtCheeks.Data
{
    public class Todo
    {
        public Todo(string description, bool isActive) => (Description, IsActive) = (description, isActive);

        public Todo(int todoId, string description, bool isActive) => (TodoId, Description, IsActive) = (todoId, description, isActive);

        public int TodoId { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public static Expression<Func<CreateTodoDto, Todo>> Projection
        {
            get
            {
                return createTodoDto => new Todo(createTodoDto.Description, true); // always create active
            }
        }

        public static Func<CreateTodoDto, Todo> FromCreateTodoDto =>
            Projection
                .Compile();
    }
}