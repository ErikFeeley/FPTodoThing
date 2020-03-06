using System.Threading.Tasks;
using MegaFpCSharpTodoBackEndTotallyNotButtCheeks.Data;
using Microsoft.AspNetCore.Mvc;
using LanguageExt;
using static LanguageExt.Prelude;
using FluentValidation;
using System.Collections.Generic;
using FluentValidation.Results;

namespace MegaFpCSharpTodoBackEndTotallyNotButtCheeks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly TodoDbContext _todoDbContext;

        public TodosController(TodoDbContext todoDbContext) =>
            _todoDbContext = todoDbContext;

        public class CreateTodoDto
        {
            public string Description { get; set; }
        }

        public class UpdateTodoDto
        {
            public string Description { get; set; }

            public bool IsActive { get; set; }

            public class UpdateTodoDtoValidator : AbstractValidator<UpdateTodoDto>
            {
                public UpdateTodoDtoValidator()
                {
                    RuleFor(x => x.Description)
                        .NotEmpty()
                        .MaximumLength(1000)
                        .WithMessage("The description is required and has a max length of 1000 characters");

                    RuleFor(x => x.IsActive)
                        .NotEmpty()
                        .WithMessage("A boolean for active state is required");
                }
            }

            public static Validation<IEnumerable<ValidationFailure>, UpdateTodoDto>
            ValidateUpdateTodo(UpdateTodoDto updateTodoDto)
            {
                var todoValidator = new UpdateTodoDtoValidator();

                var result = todoValidator.Validate(updateTodoDto);

                return result.IsValid
                    ? Success<IEnumerable<ValidationFailure>, UpdateTodoDto>(updateTodoDto)
                    : Fail<IEnumerable<ValidationFailure>, UpdateTodoDto>(result.Errors);
            }
        }


        [HttpGet]
        public async Task<ActionResult<Lst<TodoDto>>> Get([FromHeader] string authKey) =>
            (await Queries
                .TryGetAll(
                    projectExpr: TodoDto.Projection,
                    filterExpr: todo => true,
                    errorFunc: exception => $"Failed retrieving todos",
                    dbSet: _todoDbContext.Todos
                )
            )
            .Match<ActionResult<Lst<TodoDto>>>(
                Right: todos => Ok(todos),
                Left: failMessage => BadRequest(failMessage)
            );

        // has to be a normal int could not just
        // plug in the new todoId type.
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TodoDto>> Get([FromRoute] int id) =>
            (await Queries
                .TryGetOptional(
                    projectExpr: TodoDto.Projection,
                    filterExpr: todo => todo.TodoId == new TodoId(id), // crap this doesnt quite work either falls back to in memory
                    errorFunc: exception => $"Failed to retrieve todo",
                    dbSet: _todoDbContext.Todos
                )
            )
            .Match<ActionResult<TodoDto>>(
                Right: todo => todo
                    .Match<ActionResult<TodoDto>>(
                        Some: some => Ok(some),
                        None: () => NotFound()
                    ),
                Left: failMessage => BadRequest(failMessage)
            );

        [HttpPost]
        public async Task<ActionResult<TodoDto>> Create([FromBody] CreateTodoDto createTodoDto) =>
            (await Queries
                .TryCreate(
                    projectFunc: TodoDto.FromTodoEntity,
                    createFunc: Todo.FromCreateTodoDto,
                    errorFunc: exception => $"ded",
                    dbContext: _todoDbContext,
                    dto: createTodoDto
                )
            )
            .Match<ActionResult<TodoDto>>(
                Right: todo => Created($"/todos/{todo.TodoId}", todo),
                Left: failMessage => BadRequest(failMessage)
            );

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TodoDto>> Update([FromRoute] int id, [FromBody] UpdateTodoDto updateTodoDto) =>
            await UpdateTodoDto.ValidateUpdateTodo(updateTodoDto)
                .MatchAsync<ActionResult<TodoDto>>(
                    SuccAsync: async validatedUpdateTodo =>
                        (await Queries
                            .TryUpdate(
                                projectFunc: TodoDto.FromTodoEntity,
                                filterExpr: todo => todo.TodoId == new TodoId(id),
                                toUpdateFunc: update => new Todo(new TodoId(id), update.Description, update.IsActive),
                                errorFunc: exception => "shit went awry",
                                dbContext: _todoDbContext,
                                dto: updateTodoDto,
                                notFound: "not found"
                            )
                        )
                        .Match<ActionResult<TodoDto>>(
                            Right: todo => Ok(todo),
                            Left: failMessage => BadRequest(failMessage)
                        ),
                    Fail: fail => BadRequest(fail)
                );
    }
}
