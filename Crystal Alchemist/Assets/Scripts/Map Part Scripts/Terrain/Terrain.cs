using UnityEngine;

public class Terrain : MonoBehaviour
{
    [SerializeField]
    private GameObject unityIndicator;

    private void Awake()
    {
        if(this.unityIndicator != null) this.unityIndicator.SetActive(false);
    }
}
