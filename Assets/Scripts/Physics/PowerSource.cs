using System.Collections.Generic;
using UnityEngine;

namespace Physics
{
    public class PowerSource : PhysicsActorBehaviour
    {
        private HashSet<PhysicsActorBehaviour> allPhysicsActors = new HashSet<PhysicsActorBehaviour>();

        protected override void OnPhysicsPoll()
        {
            ;
        }

        protected override void ProcessConfigMode(Vector2 mousePosInWorldSpace)
        {
            ;
        }

        [ContextMenu(nameof(DetermineConnectedActors))]
        public void DetermineConnectedActors()
        {
            DetermineLinkedActors();
            // foreach (PhysicsActorBehaviour actor in connectedActors)
            // {
            //     allPhysicsActors.UnionWith(actor.connectedActors);
            // }
        }
    }
}