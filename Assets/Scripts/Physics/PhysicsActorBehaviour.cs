using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Physics
{
    public abstract class PhysicsActorBehaviour : MonoBehaviour
    {

        [Header("Physics Actor Properties")]
        public bool isActive = true;
        public List<PhysicsActorBehaviour> connectedNodes = new List<PhysicsActorBehaviour>();
        public LayerMask affectedLayerMask = 1 << 6;

        private const float physicsPollInterval = 2f;

        protected virtual void Awake()
        {
            StartCoroutine(nameof(PollPhysicsInteraction));
        }

        protected virtual void OnDrawGizmosSelected()
        {
            if (isActive) Gizmos.color = Color.green;
            if (connectedNodes.Count <= 0) return;
            foreach (PhysicsActorBehaviour actor in connectedNodes)
            {
                Gizmos.DrawLine(this.transform.position, actor.transform.position);
            }
        }

        private IEnumerator PollPhysicsInteraction()
        {
            while (true)
            {
                yield return new WaitForSeconds(physicsPollInterval);
                if (!isActive) continue;
                OnPhysicsPoll();
            }
        }

        protected abstract void OnPhysicsPoll();
    }
}