using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Physics
{
    public class PowerSource : PhysicsActorBehaviour
    {
        public UnityAction physicsPollAction;

        private PowerSource me = null;

        protected override void Awake()
        {
            base.Awake();
            me = this;
        }

        private void Start()
        {
            DetermineConnectedActors();
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
            IsActive = true;
            DetermineLinkedActors(ref me);
            ProcessVFX();
        }

        public void ResetActorConnections()
        {
            ResetVFX();
            ResetConnections();
        }
    }
}