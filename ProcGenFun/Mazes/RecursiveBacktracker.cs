namespace ProcGenFun.Mazes;

using ProcGenFun.Distributions;
using RandN;
using RandN.Extensions;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

public static class RecursiveBacktracker
{
    public static IDistribution<Maze> MazeDist(Grid grid)
    {
        var initialCellDistribution = UniformDistribution.CreateOrThrow(grid.Cells);

        return initialCellDistribution.SelectMany(initialCell => new RecursiveBacktrackerMazeDist(grid, initialCell));
    }

    private class RecursiveBacktrackerMazeDist : IDistribution<Maze>
    {
        private Grid grid;
        private readonly Cell initialCell;

        public RecursiveBacktrackerMazeDist(Grid grid, Cell initialCell)
        {
            this.grid = grid;
            this.initialCell = initialCell;
        }

        public Maze Sample<TRng>(TRng rng) where TRng : notnull, IRng
        {
            var state = new State(
                Maze: Maze.WithAllWalls(this.grid),
                CurrentCell: initialCell,
                Stack: [initialCell],
                Visited: [initialCell]);

            while (!state.Stack.IsEmpty)
            {
                var neighbouringDirections = this.grid.NeighbouringDirections(state.CurrentCell)
                    .Where(d => !state.Visited.Contains(this.grid.AdjacentCellOrNull(state.CurrentCell, d)!));

                if (UniformDistribution.TryCreate(neighbouringDirections, out var directionDist))
                {
                    var direction = directionDist.Sample(rng);
                    var ccc = this.grid.AdjacentCellOrNull(state.CurrentCell, direction)!;

                    state = new State(
                        Maze: state.Maze.RemoveWall(state.CurrentCell, direction),
                        CurrentCell: ccc,
                        Stack: state.Stack.Push(ccc),
                        Visited: state.Visited.Add(ccc));
                }
                else
                {
                    var foo = state.Stack.Pop(out var bar);
                    state = state with { CurrentCell = bar, Stack = foo };
                }
            }

            return state.Maze;
        }

        public bool TrySample<TRng>(TRng rng, [MaybeNullWhen(false)] out Maze result) where TRng : notnull, IRng
        {
            result = Sample(rng);
            return true;
        }

        private record State(Maze Maze, Cell CurrentCell, ImmutableStack<Cell> Stack, ImmutableList<Cell> Visited);
    }
}
