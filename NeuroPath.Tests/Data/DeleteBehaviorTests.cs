using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NeuroPath.Core.Models.Domain.Exercises;
using NeuroPath.Core.Models.Domain.Quizzes;
using NeuroPath.Core.Models.Domain.Training;
using NeuroPath.Data.Context;

namespace NeuroPath.Tests.Data;

public class DeleteBehaviorTests
{
    private static IModel BuildSqlServerModel()
    {
        var options = new DbContextOptionsBuilder<NeuroPathDbContext>()
            .UseSqlServer("Server=(localdb)\\unused;Database=ModelOnly;Trusted_Connection=true")
            .Options;
        using var context = new NeuroPathDbContext(options);
        return context.Model;
    }

    [Fact]
    public void Model_HasNoMultipleCascadePathsOrCycles()
    {
        var model = BuildSqlServerModel();

        var edges = model.GetEntityTypes()
            .Where(e => !e.IsOwned())
            .SelectMany(e => e.GetForeignKeys())
            .Where(fk => fk.DeleteBehavior is DeleteBehavior.Cascade or DeleteBehavior.SetNull)
            .Select(fk => (From: fk.PrincipalEntityType.GetTableName()!, To: fk.DeclaringEntityType.GetTableName()!))
            .ToList();

        var adjacency = edges.GroupBy(e => e.From).ToDictionary(g => g.Key, g => g.Select(e => e.To).ToList());
        var problems = new List<string>();

        foreach (var root in adjacency.Keys)
        {
            var pathCounts = new Dictionary<string, int>();
            var stack = new Stack<(string Node, HashSet<string> Visited)>();
            stack.Push((root, new HashSet<string> { root }));

            while (stack.Count > 0)
            {
                var (node, visited) = stack.Pop();
                if (!adjacency.TryGetValue(node, out var targets)) continue;

                foreach (var target in targets)
                {
                    if (visited.Contains(target))
                    {
                        problems.Add($"Cascade cycle: {root} -> ... -> {node} -> {target}");
                        continue;
                    }

                    pathCounts[target] = pathCounts.GetValueOrDefault(target) + 1;
                    stack.Push((target, new HashSet<string>(visited) { target }));
                }
            }

            problems.AddRange(pathCounts.Where(p => p.Value > 1).Select(p => $"{p.Key} is reachable from {root} through {p.Value} cascade paths"));
        }

        Assert.True(problems.Count == 0, string.Join(Environment.NewLine, problems.Distinct()));
    }

    [Theory]
    [InlineData(typeof(Quiz), typeof(QuizQuestion), DeleteBehavior.Cascade)]
    [InlineData(typeof(QuizQuestion), typeof(QuizAnswer), DeleteBehavior.Cascade)]
    [InlineData(typeof(Exercise), typeof(UserExerciseLog), DeleteBehavior.Restrict)]
    [InlineData(typeof(Quiz), typeof(UserQuizAttempt), DeleteBehavior.Restrict)]
    [InlineData(typeof(DifficultyLevel), typeof(Exercise), DeleteBehavior.Restrict)]
    [InlineData(typeof(TrainingPlan), typeof(DailySession), DeleteBehavior.SetNull)]
    public void Relationship_HasExpectedDeleteBehavior(Type principal, Type dependent, DeleteBehavior expected)
    {
        var model = BuildSqlServerModel();

        var foreignKey = model.FindEntityType(dependent)!
            .GetForeignKeys()
            .Single(fk => fk.PrincipalEntityType.ClrType == principal);

        Assert.Equal(expected, foreignKey.DeleteBehavior);
    }
}
