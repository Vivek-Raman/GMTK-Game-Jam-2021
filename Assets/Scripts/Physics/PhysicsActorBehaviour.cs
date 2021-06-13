using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Physics
{
    public abstract class PhysicsActorBehaviour : MonoBehaviour
    {
        private const float PHYSICS_POLL_INTERVAL = 2f;

        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }

        public PhysicsActorBehaviour Parent { get; set; } = null;

        [Header("Activation Properties")]
        [SerializeField] private bool isActive = false;
        [SerializeField] private float activationRadius = 3f;

        private List<PhysicsActorBehaviour> connectedActors = new List<PhysicsActorBehaviour>();
        protected LayerMask affectedLayerMask = 1 << 6;
        private LayerMask actorLayerMask = 1 << 8;
        private Camera cam = null;
        private bool isInConfigMode = false;

        #region Process Physics

        protected virtual void Awake()
        {
            StartCoroutine(nameof(PollPhysicsInteraction));
            cam = Camera.main;
        }

        private IEnumerator PollPhysicsInteraction()
        {
            while (true)
            {
                yield return new WaitForSeconds(PHYSICS_POLL_INTERVAL);
                if (!isActive) continue;
                OnPhysicsPoll();
            }
        }

        #endregion

        #region Linking Power

        public void DetermineLinkedActors()
        {
            if (!isActive) return;
            Collider2D[] nearbyActors = Physics2D.OverlapCircleAll(this.transform.position, activationRadius, actorLayerMask);
            if (nearbyActors.Length < 1) return;
            foreach (Collider2D actorCollider in nearbyActors)
            {
                PhysicsActorBehaviour actor = actorCollider.GetComponent<PhysicsActorBehaviour>();
                if (actor == this || actor == Parent) continue;
                actor.IsActive = true;
                actor.Parent = this;
                connectedActors.Add(actor);
                actor.DetermineLinkedActors();
            }
        }

        #endregion

        #region Configuration

        public void OnMouseDrag()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector2 mousePosInWorldSpace = Physics2D.Raycast(ray.origin, ray.direction, 50f, 1 << 7).point;

            if (!isInConfigMode)
            {
                isInConfigMode = true;
                EnterConfigMode(mousePosInWorldSpace);
            }

            ProcessConfigMode(mousePosInWorldSpace);
        }

        private void OnMouseUp()
        {
            if (!isInConfigMode) return;
            ExitConfigMode(Input.mousePosition);
            isInConfigMode = false;
        }

        #endregion

        #region OVERRIDES

        protected abstract void OnPhysicsPoll();

        protected virtual void EnterConfigMode(Vector2 mousePosInWorldSpace)
        {
        }

        protected abstract void ProcessConfigMode(Vector2 mousePosInWorldSpace);

        protected virtual void ExitConfigMode(Vector2 mousePosInWorldSpace)
        {
        }

        #endregion

        #region Editor Only - Visualize Connections

        #if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            if (isActive) Gizmos.color = Color.green;
            if (connectedActors.Count <= 0) return;
            foreach (PhysicsActorBehaviour actor in connectedActors)
            {
                if (actor == Parent) continue;
                Gizmos.DrawLine(this.transform.position, actor.transform.position);
            }
        }
        #endif

        #endregion
    }
}