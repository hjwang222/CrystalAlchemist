using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnSystem : MonoBehaviour
{
    private List<Character> respawnObjects = new List<Character>();

    void Update()
    {        
        for(int i = 0; i < this.transform.childCount; i++)
        {
            Character character = this.transform.GetChild(i).gameObject.GetComponent<Character>();

            if (!character.gameObject.activeInHierarchy 
                && character.currentState == CharacterState.dead
                && !this.respawnObjects.Contains(character))
            {                
                setRespawn(character);
            }            
        }       
    }

    private void setRespawn(Character character)
    {
        if (character.respawnTime < Utilities.maxFloatInfinite)
        {
            StartCoroutine(respawnCo(character));
            character.currentState = CharacterState.respawning;
        }
    }

    private IEnumerator respawnCo(Character character)
    {
        yield return new WaitForSeconds(character.respawnTime);
        respawnCharacter(character);
    }

    private void respawnCharacter(Character character)
    {
        character.gameObject.SetActive(true);
        character.spawn();
    }

}

