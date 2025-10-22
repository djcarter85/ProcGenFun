namespace ProcGenFun.Mazes;

public static class HighlightedMazesCreator
{
    public static IEnumerable<HighlightedMaze> FromBinaryTreeHistory(BinaryTreeHistory history) =>
        history.Steps.Select(s => new HighlightedMaze(Maze: s.Maze, CellColours.Highlighted([s.Cell])));

    public static IEnumerable<HighlightedMaze> FromSidewinderHistory(SidewinderHistory history)
    {
        var previousMaze = history.Initial;
        foreach (var step in history.Steps)
        {
            yield return new HighlightedMaze(Maze: previousMaze, CellColours.Highlighted(step.RunBeforeWallRemoved));
            yield return new HighlightedMaze(Maze: step.Maze, CellColours.Highlighted(step.Run));
            previousMaze = step.Maze;
        }
    }

    public static IEnumerable<HighlightedMaze> FromRecursiveBacktrackerHistory(
        IReadOnlyList<RecursiveBacktrackerState> history) =>
        history.Select(step => new HighlightedMaze(
            Maze: step.Maze,
            CellColours.RB(currentCell: step.CurrentCell, visitedCells: step.Visited, path: step.Path)));
}
