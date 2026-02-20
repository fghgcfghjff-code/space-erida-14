using Robust.Shared.Serialization;

namespace Content.Shared._Goobstation.SpecialAnimation;

/// <summary>
/// Raised on some client to play a spell card animation.
/// </summary>
[ImplicitDataDefinitionForInheritors]
[Serializable, NetSerializable]
public sealed partial class SpecialAnimationEvent : EntityEventArgs
{
    public SpecialAnimationEvent(SpecialAnimationData animationData)
    {
        AnimationData = animationData;
    }

    public SpecialAnimationData AnimationData;
}
