using System.Collections.Generic;
using Physics;
using UnityEngine;

namespace VFX
{
    public class VFXProcessor : MonoBehaviour
    {
        [SerializeField] private LineRenderer connectionVFX = null;
        [SerializeField] private ParticleSystem activationVFX = null;

        public void ProcessActorVFX(bool isActive, List<PhysicsActorBehaviour> connectedActors)
        {
            if (activationVFX != null)
            {
                ParticleSystem.EmissionModule emission = activationVFX.emission;
                emission.enabled = isActive;
            }

            List<Vector3> points = new List<Vector3>();
            foreach (PhysicsActorBehaviour actor in connectedActors)
            {
                points.AddRange(new Vector3[]{this.transform.position, actor.transform.position});
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

            Vector3[] points = new Vector3[0];
            connectionVFX.positionCount = 0;
            connectionVFX.SetPositions(points);
        }
    }
}