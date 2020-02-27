using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundImageSetter : MonoBehaviour
{
    [SerializeField]
    private GameObjectSignal signal;

    [SerializeField]
    private GameObject backgroundObject;

    void Start()
    {
        if (this.backgroundObject != null && this.signal != null) this.signal.Raise(this.backgroundObject);
        this.gameObject.SetActive(false);
    }
}
