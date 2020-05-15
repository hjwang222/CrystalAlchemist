using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectedArea : MonoBehaviour
{
    [SerializeField]
    private List<AI> protectingNPCs = new List<AI>();

    [SerializeField]
    [Range(0, 120)]
    private float aggroIncreaseFactor = 25;

    [SerializeField]
    [Range(-120, 0)]
    private float aggroDecreaseFactor = -25f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        setAggro(collision, false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        setAggro(collision, true);
    }

    private void setAggro(Collider2D collision, bool decrease)
    {
        Character character = collision.GetComponent<Character>();

        if (character != null)
        {
            foreach (AI enemy in this.protectingNPCs)
            {
                if (enemy.gameObject.activeInHierarchy && enemy.aggroGameObject != null)
                {
                    if(!decrease) enemy.aggroGameObject.increaseAggro(character, this.aggroIncreaseFactor);
                    else enemy.aggroGameObject.decreaseAggro(character, this.aggroDecreaseFactor);
                }
            }
        }
    }
}