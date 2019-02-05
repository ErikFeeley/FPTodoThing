using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.EntityFrameworkCore;
using static LanguageExt.Prelude;

namespace MegaFpCSharpTodoBackEndTotallyNotButtCheeks.Data
{
    public class TodoDto : Record<TodoDto>
    {
        public readonly int TodoId;
        public readonly string Description;
        public readonly bool IsActive;

        public TodoDto(int todoId, string description, bool isActive) =>
            (TodoId, Description, IsActive) = (todoId, description, isActive);

        public static Expression<Func<Todo, TodoDto>> Projection
        {
            get
            {
                return todoEntity => new TodoDto(todoEntity.TodoId, todoEntity.Description, todoEntity.IsActive);
            }
        }

        public static Func<Todo, TodoDto> FromTodoEntity =>
            Projection
                .Compile();
    }

    /// <summary>
    /// By no means is any of this a good idea.
    /// 
    /// I just wanted to see what I could get to work......
    /// 
    /// AKA what happens when a mediocre dev who happens to be a brand new dad
    /// tries to find the absolute most FUNC possible
    /// </summary>
    public static class Queries
    {
        /// <summary>
        /// This is the only kinda sane query in here
        /// </summary>
        /// <param name="todoDbSet"></param>
        /// <returns></returns>
        public static async Task<Either<string, Lst<TodoDto>>> GetAllTodos(DbSet<Todo> todoDbSet) =>
            await TryAsync(
                todoDbSet
                    .AsNoTracking()
                    .Select(TodoDto.Projection)
                    .ToListAsync()
            )
            .Map(Lst<TodoDto>.Empty.AddRange)
            .ToEither(
                exception => "There was a problem retrieving todos from the database"
            );

        /// <summary>
        /// Func
        /// </summary>
        /// <typeparam name="Dto"></typeparam>
        public static async Task<Either<Error, Lst<Dto>>> TryGetAll<Table, Dto, Error>(
            Expression<Func<Table, Dto>> projectExpr,
            Expression<Func<Table, bool>> filterExpr,
            Func<Exception, Error> errorFunc,
            DbSet<Table> dbSet
        ) where Table : class =>
            await TryAsync(
                dbSet
                    .AsNoTracking()
                    .Where(filterExpr)
                    .Select(projectExpr)
                    .ToListAsync()
            )
            .Map(Lst<Dto>.Empty.AddRange)
            .ToEither(errorFunc);

        /// <summary>
        /// FUNC
        /// </summary>
        /// <typeparam name="Dto"></typeparam>
        public static async Task<Either<Error, Option<Dto>>> TryGetOptional<Table, Dto, Error>(
            Expression<Func<Table, Dto>> projectExpr,
            Expression<Func<Table, bool>> filterExpr,
            Func<Exception, Error> errorFunc,
            DbSet<Table> dbSet
        ) where Table : class =>
            await TryOptionAsync(
                dbSet
                    .AsNoTracking()
                    .Where(filterExpr)
                    .Select(projectExpr)
                    .FirstOrDefaultAsync()
            )
            .ToEither(errorFunc);

        /// <summary>
        /// F U N C
        /// </summary>
        /// <typeparam name="Error"></typeparam>
        /// <typeparam name="OutDto"></typeparam>
        public static async Task<Either<Error, OutDto>> TryCreate<Table, InDto, OutDto, Error>(
            Func<Table, OutDto> projectFunc,
            Func<InDto, Table> createFunc,
            Func<Exception, Error> errorFunc,
            TodoDbContext dbContext,
            InDto dto
        ) where Table : class
        {
            var entityToAdd = createFunc(dto);
            dbContext.Set<Table>().Add(entityToAdd);

            return await TryAsync(
                dbContext
                    .SaveChangesAsync()
            )
            .Map(num => projectFunc(entityToAdd))
            .ToEither(errorFunc);
        }

        /// <summary>
        /// A S C E N D E D  F U N C
        /// </summary>
        /// <typeparam name="Error"></typeparam>
        /// <typeparam name="OutDto"></typeparam>
        public static async Task<Either<Error, OutDto>> TryUpdate<Table, InDto, OutDto, Error>(
            Func<Table, OutDto> projectFunc,
            Expression<Func<Table, bool>> filterExpr,
            Func<InDto, Table> toUpdateFunc,
            Func<Exception, Error> errorFunc,
            TodoDbContext dbContext,
            InDto dto,
            Error notFound
        ) where Table : class =>
            await TryOptionAsync(
                dbContext
                    .Set<Table>()
                    .AsNoTracking()
                    .Where(filterExpr)
                    .FirstOrDefaultAsync()
            )
            .ToEither(errorFunc)
            .BindAsync(
                maybeTable => maybeTable
                    .Match(
                        some => Right<Error, Table>(some),
                        () => Left<Error, Table>(notFound)
                    )
            )
            .BindAsync(
                async table =>
                {
                    var toUpdateWith = toUpdateFunc(dto);
                    dbContext
                        .Entry(toUpdateWith).State = EntityState.Modified;

                    return await TryAsync(
                        dbContext
                            .SaveChangesAsync()
                    )
                    .ToEither(errorFunc)
                    .BindAsync(
                        num => Right<Error, OutDto>(projectFunc(toUpdateWith))
                    );
                }
            );
    }
}