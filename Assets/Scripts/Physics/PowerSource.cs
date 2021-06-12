using System.Collections.Generic;
using UnityEngine;

namespace Physics
{
    public class PowerSource : PhysicsActorBehaviour
    {
        private HashSet<PhysicsActorBehaviour> allPhysicsActors = new HashSet<PhysicsActorBehaviour>();

        protected override void OnPhysicsPoll()
        {

        }

        [ContextMenu(nameof(DetermineConnectedSources))]
        public void DetermineConnectedSources()
        {
            foreach (PhysicsActorBehaviour actor in connectedNodes)
            {
                allPhysicsActors.UnionWith(actor.connectedNodes);
            }
        }
    }
}