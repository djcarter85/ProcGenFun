namespace ProcGenFun.Mazes;

public static class ColouredMazeCreator
{
    public static IEnumerable<ColouredMaze> FromBinaryTreeHistory(BinaryTreeHistory history) =>
        history.Steps.Select(s => new ColouredMaze(Maze: s.Maze, CellColours.Highlighted([s.Cell])));

    public static IEnumerable<ColouredMaze> FromSidewinderHistory(SidewinderHistory history)
    {
        var previousMaze = history.Initial;
        foreach (var step in history.Steps)
        {
            yield return new ColouredMaze(Maze: previousMaze, CellColours.Highlighted(step.RunBeforeWallRemoved));
            yield return new ColouredMaze(Maze: step.Maze, CellColours.Highlighted(step.Run));
            previousMaze = step.Maze;
        }
    }

    public static IEnumerable<ColouredMaze> FromRecursiveBacktrackerHistory(
        IReadOnlyList<RecursiveBacktrackerState> history) =>
        history.Select(step => new ColouredMaze(
            Maze: step.Maze,
            CellColours.RB(currentCell: step.CurrentCell, visitedCells: step.Visited, path: step.Path)));
}
