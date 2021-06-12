using System;
using UnityEngine;

namespace Physics
{
    public class Puller : PhysicsActorBehaviour
    {
        [Header("Puller Properties")]
        [SerializeField] private float influenceRadius = 3f;
        [SerializeField] private float forceMagnitude = 3f;

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            Gizmos.DrawWireSphere(this.transform.position, influenceRadius);
        }

        protected override void OnPhysicsPoll()
        {
            Collider2D[] affectedColliders = Physics2D.OverlapCircleAll(this.transform.position, influenceRadius, affectedLayerMask);
            for (int i = 0; i < affectedColliders.Length; ++i)
            {
                affectedColliders[i].attachedRigidbody.AddForce(
                    forceMagnitude * (this.transform.position - affectedColliders[i].transform.position).normalized);
            }
        }

        protected override void ProcessConfigMode(Vector2 mousePosInWorldSpace)
        {
            this.transform.position= Vector2.Lerp(this.transform.position, mousePosInWorldSpace, 0.75f);
        }
    }
}