using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMinimap : MonoBehaviour
{
    private Camera cam;
    public Shader shader;

    [Button]
    private void Awake()
    {
        this.cam = this.GetComponent<Camera>();
        this.cam.SetReplacementShader(shader, null);
    }
}
