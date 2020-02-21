using UnityEngine;

public class SignalLauncher : MonoBehaviour
{
    [SerializeField]
    private SimpleSignal signalOnStart;

    private void Start()
    {
        this.signalOnStart.Raise();
    }
}
