using UnityEngine;
using DG.Tweening;

public class TestScript : MonoBehaviour
{
    public float rotation;
    public float time;

    private void FixedUpdate()
    {
        this.transform.Rotate(0, 0, Time.fixedDeltaTime*this.rotation);
    }
}
