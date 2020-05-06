using UnityEngine;

public class TitleScreenRotation : MonoBehaviour
{
    [SerializeField]
    private GameObject parent;

    void LateUpdate() => this.transform.rotation = Quaternion.identity;    
}
