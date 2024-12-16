using RandN.Distributions;
using RandN;

namespace ProcGenFun.Mazes;

using RandN.Distributions;
using RandN;
using System;

public static class Sidewinder
{
    public static IDistribution<Maze> MazeDistribution(Grid grid)
    {
        var initialState = Maze.WithAllWalls(grid);

        IDistribution<Maze> mazeDist = Singleton.New(initialState);

        foreach (var y in grid.RowIndices)
        {
            mazeDist = RowDistribution(mazeDist, grid, y);
        }

        return mazeDist;
    }

    private static IDistribution<Maze> RowDistribution(IDistribution<Maze> mazeDist, Grid grid, int y)
    {
        throw new NotImplementedException();
    }
}
