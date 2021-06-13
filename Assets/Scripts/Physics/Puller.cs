using System;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            base.OnPhysicsPoll();
            Collider2D[] affectedColliders = Physics2D.OverlapCircleAll(this.transform.position, influenceRadius, affectedLayerMask);
            for (int i = 0; i < affectedColliders.Length; ++i)
            {
                affectedColliders[i].attachedRigidbody.AddForce(
                    forceMagnitude * (this.transform.position - affectedColliders[i].transform.position).normalized);
            }
        }

        protected override void ProcessConfigMode(Vector2 mousePosInWorldSpace)
        {
            this.transform.position = Vector2.Lerp(this.transform.position, mousePosInWorldSpace, 0.75f);
        }

        protected override void ExitConfigMode(Vector2 mousePosInWorldSpace)
        {
            try
            {
                base.ExitConfigMode(mousePosInWorldSpace);
                PowerSource[] sources = FindObjectsOfType<PowerSource>();
                if (sources.Length <= 0) return;
                foreach (PowerSource source in sources)
                {
                    source.ResetActorConnections();
                    source.DetermineConnectedActors();
                }
            }
            catch (StackOverflowException e)
            {
                Debug.LogError(e);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}