using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Physics
{
    public class PowerSource : PhysicsActorBehaviour
    {
        public UnityAction physicsPollAction;

        protected override void Awake()
        {
            base.Awake();
            IsActive = true;
        }

        public override void OnPhysicsPoll()
        {
            Debug.Log($"{this.name} polls physics");
        }

        protected override void ProcessConfigMode(Vector2 mousePosInWorldSpace)
        {
            ;
        }

        private void OnMouseDown()
        {
            if (isInConfigMode) return;
            physicsPollAction?.Invoke();
        }

        [ContextMenu(nameof(DetermineConnectedActors))]
        public void DetermineConnectedActors()
        {
            DetermineLinkedActors(ref physicsPollAction);
        }
    }
}