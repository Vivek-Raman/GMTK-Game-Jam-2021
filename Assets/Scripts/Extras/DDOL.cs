using UnityEngine;

namespace Extras
{
public class DDOL : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
}
