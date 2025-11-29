namespace ProcGenFun.Mazes;

using ProcGenFun.Distributions;
using RandN;
using RandN.Extensions;
using System;

public static class AldousBroder
{
    public static IDistribution<Maze> MazeDist(Grid grid) =>
        from x in HistoryDist(grid)
        select x.Last().Maze;

    public static IDistribution<IReadOnlyList<AldousBroderState>> HistoryDist(Grid grid) =>
        from initial in InitialStateDist(grid)
        from randomWalk in RandomWalk.New(initial, s => NextStateDist(grid, s))
        select randomWalk.TakeWhile(s => !StopIteration(s, grid)).ToReadOnly();

    private static bool StopIteration(AldousBroderState state, Grid grid) =>
        state.Visited.Count == grid.CellCount;

    private static IDistribution<AldousBroderState> InitialStateDist(Grid grid) =>
        from cell in UniformDistribution.Create(grid.Cells)
        select new AldousBroderState(
            Maze: Maze.WithAllWalls(grid),
            CurrentCell: cell,
            Visited: [cell]);

    private static IDistribution<AldousBroderState> NextStateDist(Grid grid, AldousBroderState state)
    {
        var neighbours = grid.GetNeighbours(state.CurrentCell);
        return
            from neighbour in UniformDistribution.Create(neighbours)
            select new AldousBroderState(
                Maze: state.Visited.Contains(neighbour.Cell) ?
                    state.Maze :
                    state.Maze.RemoveWall(state.CurrentCell, neighbour.Direction),
                CurrentCell: neighbour.Cell,
                Visited: state.Visited.Add(neighbour.Cell));
    }
}
