using UnityEngine;

public class BackgroundImageHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject background;

    private void Awake()
    {
        if(background != null) background.transform.SetParent(this.transform);
    }    
}
