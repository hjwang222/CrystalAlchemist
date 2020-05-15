using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class CameraStartUp : MonoBehaviour
{
    private float delay = 0.2f;
    private Camera cam;

    private void Awake()
    {
        this.cam = this.GetComponent<Camera>();
        this.cam.enabled = false;
        StartCoroutine(delayCo());
    }
    
    private IEnumerator delayCo()
    {
        yield return new WaitForSeconds(this.delay);
        this.cam.enabled = true;
    }
}
