namespace Content.Shared._DV.Paper;

/// <summary>
/// 	Raised on the paper when a sign is successful.
/// </summary>
[ByRefEvent]
public record struct SignSuccessfulEvent(EntityUid Paper, EntityUid User);
