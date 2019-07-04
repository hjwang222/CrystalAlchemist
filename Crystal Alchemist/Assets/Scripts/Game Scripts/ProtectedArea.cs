using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectedArea : MonoBehaviour
{
    [SerializeField]
    private List<Enemy> protectingNPCs = new List<Enemy>();

    [SerializeField]
    [Range(0, 10)]
    private float aggroIncreaseFactor = 0.25f;

    [SerializeField]
    [Range(-10, 0)]
    private float aggroDecreaseFactor = -0.25f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (Enemy enemy in this.protectingNPCs)
        {
            enemy.increaseAggro(collision.GetComponent<Character>(), this.aggroIncreaseFactor);
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (Enemy enemy in this.protectingNPCs)
        {
            enemy.decreaseAggro(collision.GetComponent<Character>(), this.aggroIncreaseFactor);
        }
    }
}
