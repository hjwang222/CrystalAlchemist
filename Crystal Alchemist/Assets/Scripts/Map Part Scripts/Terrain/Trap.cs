using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Trap : Terrain
{
    [SerializeField]
    private List<UnityEvent> events = new List<UnityEvent>();

    [SerializeField]
    private bool repeatEvents;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(UnityEvent ev in this.events)
        {
            ev.Invoke();
        }

        if(!this.repeatEvents) this.enabled = false;
    }
}
