
namespace Content.Server._Erida.Atmos.ExplodeByPressure;

[RegisterComponent]
public sealed partial class ExplodeByPressureComponent : Component
{
    [DataField]
    public float PressureLimit = 15000f;
}
