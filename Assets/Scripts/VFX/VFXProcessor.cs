using System.Collections.Generic;
using Physics;
using UnityEngine;

namespace VFX
{
    public class VFXProcessor : MonoBehaviour
    {
        [SerializeField] private LineRenderer connectionVFX = null;
        [SerializeField] private ParticleSystem activationVFX = null;
        [SerializeField] private ParticleSystem pollingVFX = null;

        private PhysicsActorBehaviour actor = null;

        public void ProcessActorVFX(bool isActive, PhysicsActorBehaviour theActor, List<PhysicsActorBehaviour> connectedActors)
        {
            actor = theActor;
            if (activationVFX != null)
            {
                ParticleSystem.EmissionModule emission = activationVFX.emission;
                emission.enabled = isActive;
                activationVFX.Play();
            }

            if (pollingVFX != null)
            {
                ParticleSystem.EmissionModule emission = pollingVFX.emission;
                emission.enabled = isActive;
                actor.pollPhysicsAction += OnPhysicsPoll;
            }

            List<Vector3> points = new List<Vector3>();
            foreach (PhysicsActorBehaviour branchActor in connectedActors)
            {
                points.AddRange(new Vector3[]{this.transform.position, branchActor.transform.position});
            }

            connectionVFX.positionCount = points.Count;
            connectionVFX.SetPositions(points.ToArray());
        }

        public void ResetVFX()
        {
            if (activationVFX != null)
            {
                ParticleSystem.EmissionModule emission = activationVFX.emission;
                emission.enabled = false;
            }

            if (pollingVFX != null)
            {
                ParticleSystem.EmissionModule emission = pollingVFX.emission;
                emission.enabled = false;
                actor.pollPhysicsAction -= OnPhysicsPoll;
            }

            Vector3[] points = new Vector3[0];
            connectionVFX.positionCount = 0;
            connectionVFX.SetPositions(points);
        }

        private void OnPhysicsPoll()
        {
            if (pollingVFX != null)
            {
                pollingVFX.Play();
            }
        }
    }
}