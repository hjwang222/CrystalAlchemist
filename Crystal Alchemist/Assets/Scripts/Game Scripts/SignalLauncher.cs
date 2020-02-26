using System.Collections;
using UnityEngine;

public class SignalLauncher : MonoBehaviour
{
    [SerializeField]
    private SimpleSignal signalOnStart;

    [SerializeField]
    private float delay = 0f;

    private void Start()
    {
        StartCoroutine(delayCo());
    }

    private IEnumerator delayCo()
    {
        yield return new WaitForSeconds(this.delay);
        this.signalOnStart.Raise();
    }
}
