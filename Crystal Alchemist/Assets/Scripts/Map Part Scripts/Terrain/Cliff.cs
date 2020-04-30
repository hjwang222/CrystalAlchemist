using System.Collections.Generic;
using UnityEngine;

public class Cliff : Terrain
{
    private List<Character> characters = new List<Character>();    
    private Vector2 direction;

    [SerializeField]
    private float speed = 32;

    [SerializeField]
    private Collider2D collider;
    

    private void Start()
    {
        this.direction = RotationUtil.DegreeToVector2(this.transform.rotation.z-90);
    }

    private void Update()
    {
        foreach(Character character in this.characters)
        {
            if (character.myRigidbody != null)
            {
                character.myRigidbody.AddForce(this.direction.normalized * this.speed * character.GetSpeedFactor() * character.values.timeDistortion, ForceMode2D.Force);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.collider.enabled = false;

        Character character = collision.GetComponent<Character>();
        if (character != null && !this.characters.Contains(character))
        {
            this.characters.Add(character);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        this.collider.enabled = true;

        Character character = collision.GetComponent<Character>();
        if (character != null && this.characters.Contains(character))
        {
            this.characters.Remove(character);
            character.myRigidbody.velocity = Vector2.zero;
        }
    }

}
