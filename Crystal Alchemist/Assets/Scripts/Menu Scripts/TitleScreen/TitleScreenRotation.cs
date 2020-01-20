using UnityEngine;

public class TitleScreenRotation : MonoBehaviour
{
    [SerializeField]
    private GameObject parent;

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.identity;
    }
}
