namespace ProcGenFun.Mazes;

public static class HighlightedMazesCreator
{
    public static IEnumerable<HighlightedMaze> FromBinaryTreeHistory(BinaryTreeHistory history) =>
        history.Steps.Select(s => new HighlightedMaze(Maze: s.Maze, HighlightedCells: [s.Cell]));

    public static IEnumerable<HighlightedMaze> FromSidewinderHistory(SidewinderHistory history)
    {
        var previousMaze = history.Initial;
        foreach (var step in history.Steps)
        {
            yield return new HighlightedMaze(Maze: previousMaze, HighlightedCells: step.RunBeforeWallRemoved);
            yield return new HighlightedMaze(Maze: step.Maze, HighlightedCells: step.Run);
            previousMaze = step.Maze;
        }
    }
}
