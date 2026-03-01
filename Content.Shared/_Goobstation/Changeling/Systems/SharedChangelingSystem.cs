using Content.Shared._Goobstation.Overlays;
using Content.Shared.Changeling.Components;
using Content.Shared.Eye.Blinding.Components;

namespace Content.Shared._Goobstation.Changeling.Systems;

public abstract class SharedChangelingSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ChangelingIdentityComponent, SwitchableOverlayToggledEvent>(OnVisionToggle);
    }

    private void OnVisionToggle(Entity<ChangelingIdentityComponent> ent, ref SwitchableOverlayToggledEvent args)
    {
        if (args.User != ent.Owner)
            return;

        if (TryComp(ent, out EyeProtectionComponent? eyeProtection))
            eyeProtection.ProtectionTime = args.Activated ? TimeSpan.Zero : TimeSpan.FromSeconds(10);

        UpdateFlashImmunity(ent, !args.Activated);
    }

    protected virtual void UpdateFlashImmunity(EntityUid uid, bool active) { }
}
