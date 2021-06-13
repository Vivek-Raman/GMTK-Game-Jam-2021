using System;
using System.Collections;
using System.Collections.Generic;
using Physics;
using UnityEngine;

namespace VFX
{
    public class LineVisualizer : MonoBehaviour
    {
        private LineRenderer lineRenderer = null;

        private void Awake()
        {
            lineRenderer = this.GetComponent<LineRenderer>();
        }

        public void DetermineLinePoints(List<PhysicsActorBehaviour> connectedActors)
        {
            List<Vector3> points = new List<Vector3>();
            foreach (PhysicsActorBehaviour actor in connectedActors)
            {
                points.AddRange(new Vector3[]{this.transform.position, actor.transform.position});
            }

            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ToArray());
        }

        public void ClearLinePoints()
        {
            Vector3[] points = new Vector3[0];
            lineRenderer.positionCount = 0;
            lineRenderer.SetPositions(points);
        }
    }
}