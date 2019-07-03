using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectedArea : MonoBehaviour
{
    [SerializeField]
    private List<Enemy> protectingNPCs = new List<Enemy>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (Enemy enemy in this.protectingNPCs)
        {
            enemy.increaseAggro(collision);
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (Enemy enemy in this.protectingNPCs)
        {
            enemy.decreaseAggro(collision);
        }
    }
}
