using Robust.Shared.Prototypes;

namespace Content.Shared._Goobstation.SpecialAnimation;

/// <summary>
/// Prototype for custom SpecialAnimationData.
/// </summary>
[Prototype]
public sealed partial class SpecialAnimationPrototype : IPrototype
{
    /// <inheritdoc/>
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField]
    public SpecialAnimationData Animation = SpecialAnimationData.DefaultAnimation;
}
