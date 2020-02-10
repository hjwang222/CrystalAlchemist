using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnSystem : MonoBehaviour
{
    private List<Character> respawnObjects = new List<Character>();

    void FixedUpdate()
    {
        setRespawnObjects(this.transform.gameObject);
    }

    private void setRespawnObjects(GameObject parent)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject child = parent.transform.GetChild(i).gameObject;

            Character character = child.gameObject.GetComponent<Character>();

            if (character != null)
            {
                if (!character.gameObject.activeInHierarchy
                 && character.currentState == CharacterState.dead
                 && !this.respawnObjects.Contains(character))
                {
                    setRespawn(character);
                }
            }
            else
            {
                setRespawnObjects(child);
            }
        }
    }

    private void setRespawn(Character character)
    {
        if (character.stats.respawnTime < CustomUtilities.maxFloatInfinite)
        {
            StartCoroutine(respawnCo(character));
            character.currentState = CharacterState.respawning;
        }
    }

    private IEnumerator respawnCo(Character character)
    {
        yield return new WaitForSeconds(character.stats.respawnTime);
        respawnCharacter(character);
    }

    private void respawnCharacter(Character character)
    {
        if (character.stats.respawnAnimation != null)
        {
            //spawn character after animation
            RespawnAnimation respawnObject = Instantiate(character.stats.respawnAnimation, character.spawnPosition, Quaternion.identity);
            respawnObject.setCharacter(character);
        }
        else
        {
            //spawn character immediately
            character.gameObject.SetActive(true);
            character.PlayRespawnAnimation();
            character.initSpawn(true);
        }
    }
}

