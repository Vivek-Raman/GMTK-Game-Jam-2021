using System;
using UnityEngine;

namespace Physics
{
    public class Puller : PhysicsActorBehaviour
    {
        [Header("Puller Properties")]
        [SerializeField] private float influenceRadius = 3f;
        [SerializeField] private float forceMagnitude = 3f;
        
        private Collider2D[] affectedColliders = new Collider2D[5];

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            Gizmos.DrawWireSphere(this.transform.position, influenceRadius);
        }

        protected override void OnPhysicsPoll()
        {
            int colliderCount = Physics2D.OverlapCircleNonAlloc(this.transform.position, influenceRadius,
                affectedColliders, affectedLayerMask);
            for (int i = 0; i < colliderCount; ++i)
            {
                affectedColliders[i].attachedRigidbody.AddForce(
                    forceMagnitude * (this.transform.position - affectedColliders[i].transform.position).normalized);
            }
        }
    }
}