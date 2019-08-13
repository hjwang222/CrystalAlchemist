using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    [SerializeField]
    private bool leftToRight = false;

    [SerializeField]
    private float degree = 0.75f;

    private float stepValue = 0;

    private void Start()
    {
        this.stepValue = degree;
        if (leftToRight) this.stepValue = -degree;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (character != null)
        {
            character.steps = this.stepValue;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (character != null)
        {
            character.steps = 0;
        }
    }
}
