using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SingleColor : UnityEvent<Color>
{
}

public class ColorSignalListener : MonoBehaviour
{
    public ColorSignal signal;
    public SingleColor signalEventColor;

    public void OnSignalRaised(Color color)
    {
        this.signalEventColor.Invoke(color);
    }

    private void OnEnable()
    {
        signal.RegisterListener(this);
    }

    private void OnDisable()
    {
        signal.DeRegisterListener(this);
    }
}
