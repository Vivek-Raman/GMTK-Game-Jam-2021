using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VFX;

namespace Physics
{
    public abstract class PhysicsActorBehaviour : MonoBehaviour
    {
        public UnityAction pollPhysicsAction;

        public bool IsActive
        {
            get => isActive;
            set => isActive = value;
        }
        public PhysicsActorBehaviour Parent { get; set; } = null;

        [Header("Activation Properties")]
        [SerializeField] private float activationRadius = 3f;

        [HideInInspector] public List<PhysicsActorBehaviour> connectedActors = new List<PhysicsActorBehaviour>();
        private bool isActive = false;
        protected PowerSource connectedPowerSource = null;
        protected bool isInConfigMode = false;
        private Camera cam = null;

        protected readonly LayerMask affectedLayerMask = 1 << 6;
        private readonly LayerMask actorLayerMask = 1 << 8;

        protected virtual void Awake()
        {
            cam = Camera.main;
        }

        #region Linking Power

        protected void DetermineLinkedActors(ref PowerSource powerSource)
        {
            if (!isActive) return;
            connectedPowerSource = powerSource;
            Collider2D[] nearbyActors = Physics2D.OverlapCircleAll(this.transform.position, activationRadius, actorLayerMask);
            if (nearbyActors.Length < 1) return;
            foreach (Collider2D actorCollider in nearbyActors)
            {
                PhysicsActorBehaviour actor = actorCollider.GetComponent<PhysicsActorBehaviour>();
                if (actor == this || actor == Parent) continue;
                if (actor.Parent != null) continue;
                if (actor is PowerSource) continue;
                actor.IsActive = true;
                actor.Parent = this;
                connectedActors.Add(actor);
                connectedPowerSource.physicsPollAction += actor.OnPhysicsPoll;
                actor.DetermineLinkedActors(ref powerSource);
                actor.ProcessVFX();
            }
        }

        protected void ResetConnections()
        {
            if (connectedActors.Count > 0)
            {
                foreach (PhysicsActorBehaviour actor in connectedActors)
                {
                    if (actor == Parent) continue;
                    if (actor == this) continue;
                    actor.ResetConnections();
                }
            }

            IsActive = false;
            Parent = null;
            connectedPowerSource.physicsPollAction -= OnPhysicsPoll;
            connectedActors.Clear();
            connectedPowerSource = null;
            ResetVFX();
        }

        #endregion
        
        #region Configuration

        public void OnMouseDrag()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            Vector2 mousePosInWorldSpace = Physics2D.Raycast
                (ray.origin, ray.direction, 50f, 1 << 7).point;

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

        protected virtual void OnPhysicsPoll()
        {
            pollPhysicsAction?.Invoke();
        }

        protected virtual void EnterConfigMode(Vector2 mousePosInWorldSpace)
        {
        }

        protected abstract void ProcessConfigMode(Vector2 mousePosInWorldSpace);

        protected virtual void ExitConfigMode(Vector2 mousePosInWorldSpace)
        {
        }

        #endregion

        #region VFX

        public void ProcessVFX()
        {
            VFXProcessor vfx = this.GetComponentInChildren<VFXProcessor>();
            if (vfx == null) return;
            vfx.ProcessActorVFX(IsActive, this, connectedActors);
        }

        public void ResetVFX()
        {
            VFXProcessor vfx = this.GetComponentInChildren<VFXProcessor>();
            if (vfx == null) return;
            vfx.ResetVFX();
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