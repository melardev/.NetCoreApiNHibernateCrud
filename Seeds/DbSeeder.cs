using System;
using System.Threading.Tasks;
using ApiCoreNHibernateCrud.Data;
using ApiCoreNHibernateCrud.Entities;
using Bogus;
using NHibernate.Linq;

namespace ApiCoreNHibernateCrud.Seeds
{
    public class DbSeeder
    {
        public static async Task Seed(ISessionFactoryBuilder context)
        {
            await SeedTodos(context);
        }


        public static async Task SeedTodos(ISessionFactoryBuilder context)
        {
            using (var session = context.GetSessionFactory().OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var todosCount = await session.Query<Todo>().CountAsync();
                    var todosToSeed = 32;
                    todosToSeed -= todosCount;
                    if (todosToSeed > 0)
                    {
                        Console.WriteLine($"[+] Seeding {todosToSeed} Todos");
                        var faker = new Faker<Todo>()
                            .RuleFor(a => a.Title, f => string.Join(" ", f.Lorem.Words(f.Random.Int(2, 5))))
                            .RuleFor(a => a.Description, f => f.Lorem.Sentences(f.Random.Int(1, 10)))
                            .RuleFor(t => t.Completed, f => f.Random.Bool(0.4f))
                            .RuleFor(a => a.CreatedAt,
                                f => f.Date.Between(DateTime.Now.AddYears(-5), DateTime.Now.AddDays(-1)))
                            .FinishWith((f, todoInstance) =>
                            {
                                todoInstance.UpdatedAt =
                                    f.Date.Between(todoInstance.CreatedAt, DateTime.Now);
                            });

                        var todos = faker.Generate(todosToSeed);
                        foreach (var todo in todos) await session.SaveAsync(todo);

                        await transaction.CommitAsync();
                    }
                }
            }
        }
    }
}