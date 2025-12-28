namespace ProcGenFun.Mazes;

public static class ColouredMazeCreator
{
    public static IEnumerable<ColouredMaze<RectCell>> FromBinaryTreeHistory(BinaryTreeHistory history) =>
        history.Steps.Select(s => new ColouredMaze<RectCell>(Maze: s.Maze, CellColours.Highlighted([s.Cell])));

    public static IEnumerable<ColouredMaze<RectCell>> FromSidewinderHistory(SidewinderHistory history)
    {
        var previousMaze = history.Initial;
        foreach (var step in history.Steps)
        {
            yield return new ColouredMaze<RectCell>(Maze: previousMaze, CellColours.Highlighted(step.RunBeforeWallRemoved));
            yield return new ColouredMaze<RectCell>(Maze: step.Maze, CellColours.Highlighted(step.Run));
            previousMaze = step.Maze;
        }
    }

    public static IEnumerable<ColouredMaze<TCell>> FromRecursiveBacktrackerHistory<TCell>(
        IReadOnlyList<RecursiveBacktrackerState<TCell>> history)
        where TCell : notnull =>
        history.Select(step => new ColouredMaze<TCell>(
            Maze: step.Maze,
            CellColours.RB(currentCell: step.CurrentVertex, visitedCells: step.Visited, path: step.Path)));
}
