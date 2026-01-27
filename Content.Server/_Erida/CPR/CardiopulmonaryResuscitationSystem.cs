using System.Runtime.CompilerServices;
using System.Threading;
using Content.Server.DoAfter;
using Content.Shared.CRP;
using Content.Shared.Damage;
using Content.Shared.Damage.Components;
using Content.Shared.DoAfter;
using Content.Shared.FixedPoint;
using Content.Shared.Interaction.Components;
using Content.Shared.Interaction.Events;
using Content.Shared.Mobs.Components;
using Content.Shared.Mobs.Systems;
using Content.Shared.Verbs;
using Robust.Shared.Serialization;
using DependencyAttribute = Robust.Shared.IoC.DependencyAttribute;

namespace Content.Shared.Interaction;

public sealed class CardiopulmonaryResuscitationSystem : EntitySystem
{
    [Dependency] protected readonly EntityManager EntityManager = default!;
    [Dependency] private readonly InteractionPopupSystem _interactionPopupSystem = default!;
    [Dependency] private readonly MobStateSystem _mobStateSystem = default!;
    [Dependency] private readonly DoAfterSystem _doAfterSystem = default!;
    [Dependency] private readonly Damage.Systems.DamageableSystem _damageable = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<CardiopulmonaryResuscitationComponent, GetVerbsEvent<AlternativeVerb>>(AddCRPVerb);
        SubscribeLocalEvent<CardiopulmonaryResuscitationComponent, InteractHandEvent>(OnInteractHand); // InteractUsingEvent InteractHandEvent
        SubscribeLocalEvent<CardiopulmonaryResuscitationComponent, CRPDoAfterEvent>(CRPDoAfter);
    }

    private void AddCRPVerb(EntityUid uid, CardiopulmonaryResuscitationComponent component, GetVerbsEvent<AlternativeVerb> args)
    {
        if (!args.CanInteract || !args.CanAccess)
            return;

        if (!_mobStateSystem.IsCritical(args.Target))
            return;

        if (!HasComp<CardiopulmonaryResuscitationComponent>(args.Target))
        {
            return;
        }

        AlternativeVerb verb = new()
        {
            Act = () =>
            {
                StartCRP(args.User, uid, component);
            },
            Text = Loc.GetString("CRP-verb"),
            Priority = 2
        };

        args.Verbs.Add(verb);
    }

    public void OnInteractHand(EntityUid uid, CardiopulmonaryResuscitationComponent component, InteractHandEvent args)
    {
        if (args.Handled)
            return;

        if (!(TryComp<MobStateComponent>(args.Target, out var mobState)
            && _mobStateSystem.IsCritical(args.Target, mobState)))
        {
            return;
        }

        StartCRP(uid, args.Target, component);
    }

    private void StartCRP(EntityUid CRPer, EntityUid CRPied, CardiopulmonaryResuscitationComponent component)
    {
        TimeSpan length = TimeSpan.FromSeconds(3);

        //component.CancelToken = new CancellationTokenSource();

        var ev = new CRPDoAfterEvent();
        var args = new DoAfterArgs(EntityManager, CRPer, length, ev, CRPied, target: CRPied)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            //NeedHand = true
        };

        _doAfterSystem.TryStartDoAfter(args);
    }
    private void CRPDoAfter(EntityUid uid, CardiopulmonaryResuscitationComponent component, CRPDoAfterEvent args)
    {
        //component.CancelToken = null;
        if (args.Handled || args.Cancelled)
            return;

        CRP(args.Args.User, uid);
        args.Repeat = true;
        args.Args.Event.Repeat = args.Repeat;
        args.Handled = true;
    }

    private void CRP(EntityUid CRPer, EntityUid CRPied)
    {
        if (!TryComp<DamageableComponent>(CRPied, out var DCcomp))
            return;

        var damageSpec = new DamageSpecifier();
        damageSpec.DamageDict = new Dictionary<string, FixedPoint2>()
        {
            { "Asphyxiation", FixedPoint2.New(-5) }
        };

        _damageable.TryChangeDamage(
            (CRPied, DCcomp),
            damageSpec,
            ignoreResistances: true,
            interruptsDoAfters: true
        );
    }
}
