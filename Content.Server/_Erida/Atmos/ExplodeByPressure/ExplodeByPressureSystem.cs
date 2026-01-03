using Content.Server.Atmos.Piping.Components;
using Content.Server.NodeContainer.Nodes;
using Content.Shared.NodeContainer;
using Content.Server.Explosion.EntitySystems;

namespace Content.Server._Erida.Atmos.ExplodeByPressure;

public sealed partial class ExplodeByPressureSystem : EntitySystem
{
    [Dependency] private readonly ExplosionSystem _explosionSystem = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ExplodeByPressureComponent, AtmosDeviceUpdateEvent>(OnPressureChanged);
    }

    private void OnPressureChanged(EntityUid uid, ExplodeByPressureComponent component, ref AtmosDeviceUpdateEvent args)
    {
        if (!TryComp<NodeContainerComponent>(uid, out var nodeContainer))
            return;

        foreach (var node in nodeContainer.Nodes.Values)
        {
            if (node is not PipeNode)
                continue;
            var pipe = (PipeNode)node;

            if (pipe.Air.Pressure >= component.PressureLimit)
            {
                _explosionSystem.QueueExplosion(uid,
                    ExplosionSystem.DefaultExplosionPrototypeId,
                    10f,
                    1f,
                    3f);

                QueueDel(uid);
                return;
            }
        }
    }
}
