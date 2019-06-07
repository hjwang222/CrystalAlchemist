using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnSystem : MonoBehaviour
{
    private List<RespawnObject> respawnObjects = new List<RespawnObject>();

    void Update()
    {        
        for(int i = 0; i < this.transform.childCount; i++)
        {
            Character character = this.transform.GetChild(i).gameObject.GetComponent<Character>();

            if (!character.gameObject.activeInHierarchy && character.currentState == CharacterState.dead)
            {
                this.respawnObjects.Add(new RespawnObject(character));
            }            
        }

        List<RespawnObject> temp = new List<RespawnObject>();

        foreach(RespawnObject respawnObject in this.respawnObjects)
        {
            if (respawnObject.spawnNow)
            {
                respawnObject.respawnCharacter();
            }
            else
            {
                respawnObject.countdown();
                temp.Add(respawnObject);
            }
        }

        this.respawnObjects = temp;
    }
}

public class RespawnObject
{
    private float respawnTimeLeft;
    private Character character;
    public bool spawnNow = false;
    
    public RespawnObject(Character character)
    {
        this.character = character;
        if(character.respawnTime < Utilities.maxFloatInfinite) this.respawnTimeLeft = character.respawnTime;
        this.character.currentState = CharacterState.respawning;
    }

    public void countdown()
    {
        if(this.respawnTimeLeft > 0)
        {
            this.respawnTimeLeft -= Time.deltaTime;
        }
        else
        {
            this.spawnNow = true;
        }
    }

    public void respawnCharacter()
    {
        this.character.gameObject.SetActive(true);
        this.character.spawn();
    }
}
