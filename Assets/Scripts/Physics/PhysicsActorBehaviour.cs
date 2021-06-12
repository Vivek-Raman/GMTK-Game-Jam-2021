using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Physics
{
    public abstract class PhysicsActorBehaviour : MonoBehaviour
    {
        private const float PHYSICS_POLL_INTERVAL = 2f;

        [Header("Physics Actor Properties")]
        public bool isActive = true;
        public List<PhysicsActorBehaviour> connectedNodes = new List<PhysicsActorBehaviour>();
        public LayerMask affectedLayerMask = 1 << 6;

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

        protected abstract void OnPhysicsPoll();

        protected virtual void EnterConfigMode(Vector2 mousePosInWorldSpace)
        {
        }

        protected abstract void ProcessConfigMode(Vector2 mousePosInWorldSpace);

        protected virtual void ExitConfigMode(Vector2 mousePosInWorldSpace)
        {
        }

        #region Editor Only

        #if UNITY_EDITOR
        protected virtual void OnDrawGizmosSelected()
        {
            if (isActive) Gizmos.color = Color.green;
            if (connectedNodes.Count <= 0) return;
            foreach (PhysicsActorBehaviour actor in connectedNodes)
            {
                Gizmos.DrawLine(this.transform.position, actor.transform.position);
            }
        }
        #endif

        #endregion
    }
}