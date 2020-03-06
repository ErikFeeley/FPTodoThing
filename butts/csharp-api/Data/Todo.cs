using System;
using System.Linq.Expressions;
using LanguageExt;
using static MegaFpCSharpTodoBackEndTotallyNotButtCheeks.Controllers.TodosController;

namespace MegaFpCSharpTodoBackEndTotallyNotButtCheeks.Data
{
    public class TodoId : NewType<TodoId, int> { public TodoId(int x) : base(x) { } }

    public class Todo
    {
        public Todo(string description, bool isActive) => (Description, IsActive) = (description, isActive);

        public Todo(TodoId todoId, string description, bool isActive) => (TodoId, Description, IsActive) = (todoId, description, isActive);

        public TodoId TodoId { get; set; }

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
