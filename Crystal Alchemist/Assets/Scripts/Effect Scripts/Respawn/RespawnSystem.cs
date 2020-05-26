using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class RespawnSystem : MonoBehaviour
{
    private enum SpawnType
    {
        none,
        day,
        night,
        time
    }

    [System.Serializable]
    private class RespawnTimer
    {
        public float timeElapsed;
        public Character character;
        public bool spawnIt = false;        

        public RespawnTimer(Character character)
        {
            this.character = character;
            if (this.character.values == null) this.character.values = new CharacterValues();
            this.character.values.currentState = CharacterState.respawning;
            this.timeElapsed = character.stats.respawnTime;
        }

        public void Updating(float time)
        {
            if (!this.spawnIt)
            {
                if (this.timeElapsed > 0) this.timeElapsed -= time;
                else
                {
                    if (Random.Range(1, 100) <= character.stats.respawnChance) this.spawnIt = true;
                    else this.timeElapsed = character.stats.respawnTime;
                }
            }
        }
    }

    [SerializeField]
    private TimeValue time;

    [SerializeField]
    private float updateTime = 1f;

    [SerializeField]
    private SpawnType spawnType = SpawnType.none;

    [SerializeField]
    [ShowIf("spawnType", SpawnType.time)]
    private int from;

    [SerializeField]
    [ShowIf("spawnType", SpawnType.time)]
    private int to;

    [BoxGroup("Debug")]
    [SerializeField]
    private List<RespawnTimer> respawnObjects = new List<RespawnTimer>();

    private void Start() => InvokeRepeating("Updating", 0f, this.updateTime);
    
    private bool NotActive()
    {
        return (this.spawnType == SpawnType.day && time.night)
            || (this.spawnType == SpawnType.night && !time.night)
            || (this.spawnType == SpawnType.time && this.from >= time.getHour() && this.to <= time.getHour());
    }

    private bool MustDespawn(Character child)
    {
        return (child != null
                && child.gameObject.activeInHierarchy
                && child.values.currentState != CharacterState.respawning);
    }

    private void DisableGameObjects()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Character character = this.transform.GetChild(i).gameObject.GetComponent<Character>();
            if (MustDespawn(character)) DespawnCharacter(character);            
        }          
    }

    private void Updating()
    {
        if (this.gameObject.activeInHierarchy) //stops system when not active
        {
            if (NotActive()) DisableGameObjects(); //set characters inactive
            SetRespawnObjects(); //Add inactive characters to list
            UpdateRespawnObjects(); //update timer of listed characters
            if (!NotActive()) SpawnObjects(); //spawn characters    
        }
    }

    private void SetRespawnObjects()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject child = this.transform.GetChild(i).gameObject;
            Character character = child.gameObject.GetComponent<Character>();

            if (character != null 
                && !character.gameObject.activeInHierarchy
                && !this.Contains(character)
                && character.stats.hasRespawn) this.respawnObjects.Add(new RespawnTimer(character));
        }
    }

    private void UpdateRespawnObjects()
    {
        for (int i = 0; i < this.respawnObjects.Count; i++)
        {
            if (!this.respawnObjects[i].spawnIt) respawnObjects[i].Updating(this.updateTime);
        }
    }

    private void SpawnObjects()
    {
        for (int i = 0; i < this.respawnObjects.Count; i++)
        {
            if (this.respawnObjects[i].spawnIt)
            {
                respawnCharacter(this.respawnObjects[i].character);
                this.respawnObjects[i] = null;
            }
        }
        this.respawnObjects.RemoveAll(x => x == null);
    }

    private bool Contains(Character character)
    {
        for(int i = 0; i < this.respawnObjects.Count; i++)
        {
            if(this.respawnObjects[i].character == character) return true;            
        }
        return false;
    }

    private void DespawnCharacter(Character character)
    {
        if (character.respawnAnimation != null)
        {            
            RespawnAnimation respawnObject = Instantiate(character.respawnAnimation, character.GetSpawnPosition(), Quaternion.identity);
            respawnObject.Reverse(character);
            character.SetCharacterSprites(true);

            StartCoroutine(InactiveCo(respawnObject.getAnimationLength(), character.gameObject));
            StartCoroutine(InactiveCo(respawnObject.getAnimationLength(), respawnObject.gameObject));
        }
        else
        {
            character.PlayDespawnAnimation();
            character.SpawnOut();
            StartCoroutine(InactiveCo(character.GetDespawnLength(), character.gameObject));
        }

        character.values.currentState = CharacterState.respawning;        
    }

    private IEnumerator InactiveCo(float seconds, GameObject character)
    {
        yield return new WaitForSeconds(seconds);
        character.SetActive(false);
    }

    private void respawnCharacter(Character character)
    {
        character.gameObject.SetActive(true);
        character.values.currentState = CharacterState.respawning;

        if (character.respawnAnimation != null)
        {
            //spawn character after animation
            RespawnAnimation respawnObject = Instantiate(character.respawnAnimation, character.GetSpawnPosition(), Quaternion.identity);
            respawnObject.Initialize(character);
            character.SetCharacterSprites(false);
        }
        else
        {
            //spawn character immediately
            character.SetCharacterSprites(true);
            character.PlayRespawnAnimation();
            character.SpawnIn();
        }
    }
}

