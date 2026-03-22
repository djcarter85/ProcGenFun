namespace ProcGenFun.Flags.Model;

public record FlagCharge(
    FlagChargeShape Shape,
    float Size,
    FlagChargeHorizontalLocation HorizontalLocation,
    FlagChargeVerticalLocation VerticalLocation);