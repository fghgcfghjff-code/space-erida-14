using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;

namespace Content.Shared._Erida.Language.Components;

/// <summary>
/// Component, which adding to entity languages on spawn
/// </summary>
[RegisterComponent]
public sealed partial class LanguageOnSpawnComponent : Component
{
    [DataField("languages", customTypeSerializer: typeof(PrototypeIdListSerializer<LanguagePrototype>), required: true)]
    public List<string> Languages = new();
}
