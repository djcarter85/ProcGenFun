namespace ProcGenFun.Mazes;

public static class HighlightedMazesCreator
{
    public static IEnumerable<HighlightedMaze> FromBinaryTreeHistory(BinaryTreeHistory history) =>
        history.Steps.Select(s => new HighlightedMaze(Maze: s.Maze, HighlightedCells: [s.Cell]));
}
