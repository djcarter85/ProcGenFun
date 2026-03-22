namespace ProcGenFun.Flags.Model;

public record Flag(FlagPattern Pattern, IReadOnlyList<FlagCharge> Charges);