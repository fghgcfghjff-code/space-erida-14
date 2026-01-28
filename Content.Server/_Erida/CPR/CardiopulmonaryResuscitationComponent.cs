namespace Content.Shared.Interaction.Components;

[RegisterComponent, Access(typeof(CardiopulmonaryResuscitationSystem))]
public sealed partial class CardiopulmonaryResuscitationComponent : Component
{
    [DataField]
    public float CRPAsphyxiationAmount = -3f;

    [DataField]
    public int CRPHowLong = 3;
}
