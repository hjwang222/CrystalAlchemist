using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcedArea : MonoBehaviour
{
    private List<Character> characters = new List<Character>();

    [SerializeField]
    [Range(-100,0)]
    private int startSpeed = -50;

    private void Update()
    {
        foreach (Character character in this.characters)
        {
            if (character.currentState == CharacterState.walk && character.speed < character.stats.startSpeed)
            {
                character.myRigidbody.AddForce(character.direction.normalized * character.speed * character.timeDistortion, ForceMode2D.Force);
            }                       
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (character != null)
        {
            character.isOnIce = true;
            character.myRigidbody.velocity = Vector2.zero;
            character.updateSpeed(this.startSpeed, false);

            if (!this.characters.Contains(character)) this.characters.Add(character);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Character character = collision.GetComponent<Character>();
        if (character != null)
        {
            character.isOnIce = false;
            character.myRigidbody.velocity = Vector2.zero;
            character.updateSpeed(0, false);

            if (this.characters.Contains(character)) this.characters.Remove(character);
        }
    }
}
