namespace ProcGenFun.Mazes;

using ProcGenFun.Distributions;
using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class BinaryTree
{
    public static IDistribution<Maze> MazeDistribution(Grid grid)
    {
        var initialState = new State(Maze.WithAllWalls(grid));

        IDistribution<State> stateDist = Singleton.New(initialState);

        foreach (var cell in grid.Cells)
        {
            stateDist = NextStepDistribution(stateDist, grid, cell);
        }

        return stateDist.Select(s => s.Maze);
    }

    private static IDistribution<State> NextStepDistribution(
        IDistribution<State> stateDist, Grid grid, Cell cell)
    {
        var validDirections = GetValidDirections(cell, grid);

        return UniformDistribution.TryCreate(validDirections, out var directionDist)
            ? (from state in stateDist
               from direction in directionDist
               select new State(Maze: state.Maze.RemoveWall(cell, direction)))
            : stateDist;
    }

    private static IEnumerable<Direction> GetValidDirections(Cell cell, Grid grid) =>
        from dir in new[] { Direction.North, Direction.East }
        where grid.AdjacentCellOrNull(cell, dir) != null
        select dir;

    private record State(Maze Maze);
}
