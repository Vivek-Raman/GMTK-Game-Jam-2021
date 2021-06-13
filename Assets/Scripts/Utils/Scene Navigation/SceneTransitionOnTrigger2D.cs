using System.Collections;
using UnityEngine;

namespace Utils.SceneNavigation
{
    [RequireComponent(typeof(Collider2D))]
    public class SceneTransitionOnTrigger2D : SceneTransition
    {
        private IEnumerator OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) yield break;
            yield return new WaitForSeconds(0.3f);
            MoveToNextScene();
        }
    }
}