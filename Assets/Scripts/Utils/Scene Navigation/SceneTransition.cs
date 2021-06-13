using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utils.SceneNavigation
{
    public class SceneTransition : MonoBehaviour
    {
        public SceneName nextScene = SceneName.NULL;

        public void MoveToNextScene()
        {
            SceneManager.LoadScene((int) nextScene);
        }
    }
}