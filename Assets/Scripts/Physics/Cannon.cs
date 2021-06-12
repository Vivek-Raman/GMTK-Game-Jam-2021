using System;
using Extras;
using Physics;
using UnityEngine;

namespace Physics
{
    public class Cannon : PhysicsActorBehaviour
    {
        [Header("Cannon Properties")]
        [SerializeField] private float launchForceMagnitude = 50f;

        private Transform cannonHead = null;
        private Rigidbody2D playerRigidbody = null;
        private bool isStoringPlayer = false;

        protected override void Awake()
        {
            base.Awake();
            cannonHead = this.transform.GetChild(1);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            playerRigidbody = other.GetComponent<Rigidbody2D>();
            playerRigidbody.isKinematic = true;
            playerRigidbody.velocity = Vector2.zero;
            // TODO: lerp / tween
            playerRigidbody.MovePosition(this.transform.position);
            isStoringPlayer = true;
        }

        protected override void OnPhysicsPoll()
        {
            if (!isStoringPlayer) return;
            playerRigidbody.isKinematic = false;
            playerRigidbody.AddForce(launchForceMagnitude * cannonHead.up);
            isStoringPlayer = false;
        }

        protected override void ProcessConfigMode(Vector2 mousePosInWorldSpace)
        {
            Vector3 newUp = mousePosInWorldSpace.ToVector3() - cannonHead.transform.position;
            cannonHead.up = newUp.SetAxis('z', 0f);
        }
    }
}