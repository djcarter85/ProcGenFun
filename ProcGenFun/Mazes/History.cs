namespace ProcGenFun.Mazes;
using System.Collections.Immutable;

public record History<TState>(ImmutableList<TState> Previous, TState Current);
