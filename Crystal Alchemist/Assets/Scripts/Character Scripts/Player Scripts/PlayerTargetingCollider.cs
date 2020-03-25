using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingCollider : MonoBehaviour
{
    [SerializeField]
    private PlayerTargetingSystem system;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        system.addTarget(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        system.addTarget(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        system.removeTarget(collision);
    }
}
