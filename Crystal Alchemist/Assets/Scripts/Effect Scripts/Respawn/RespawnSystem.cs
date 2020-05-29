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
        public GameObject gameObject;
        public bool spawnIt = false;        

        public RespawnTimer(GameObject gameObject)
        {
            this.gameObject = gameObject;

            Character character = this.gameObject.GetComponent<Character>();

            if (character != null)
            {
                if (character.values == null) character.values = new CharacterValues();
                character.values.currentState = CharacterState.respawning;
                this.timeElapsed = character.stats.respawnTime;
            }
        }

        public void Updating(float time)
        {
            if (!this.spawnIt)
            {
                if (this.timeElapsed > 0) this.timeElapsed -= time;
                else
                {
                    Character character = this.gameObject.GetComponent<Character>();

                    if (character != null)
                    {
                        if (Random.Range(1, 100) <= character.stats.respawnChance) this.spawnIt = true;
                        else this.timeElapsed = character.stats.respawnTime;
                    }
                    else this.spawnIt = true;
                }
            }
        }

        public void SetSpawnImmediately()
        {
            Character character = this.gameObject.GetComponent<Character>();
            this.timeElapsed = 0;

            if (character != null)
            {
                if (Random.Range(1, 100) <= character.stats.respawnChance) this.spawnIt = true;
                else this.timeElapsed = character.stats.respawnTime;
            }
            else this.spawnIt = true;
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

    private bool isActive = false;

    private void Start() => InvokeRepeating("Updating", 0f, this.updateTime);
    
    private void SetIsActive()
    {
        bool result = ((this.spawnType == SpawnType.none)
                    || (this.spawnType == SpawnType.day && !time.night)
                    || (this.spawnType == SpawnType.night && time.night)
                    || (this.spawnType == SpawnType.time && this.from >= time.getHour() && this.to <= time.getHour()));

        if (result != this.isActive)
        {
            this.isActive = result;
            RespawnAll();
        }
    }

    private bool MustDespawn(GameObject child)
    {
        return (child != null && child.gameObject.activeInHierarchy);
    }

    private void DisableGameObjects()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (MustDespawn(this.transform.GetChild(i).gameObject)) DespawnCharacter(this.transform.GetChild(i).gameObject);            
        }          
    }

    private void Updating()
    {
        if (this.gameObject.activeInHierarchy) //stops system when not active
        {
            SetIsActive();
            if (!this.isActive) DisableGameObjects(); //set characters inactive
            SetRespawnObjects(); //Add inactive characters to list
            UpdateRespawnObjects(); //update timer of listed characters
            if (this.isActive) SpawnObjects(); //spawn characters    
        }
    }

    private void SetRespawnObjects()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            GameObject child = this.transform.GetChild(i).gameObject;

            if (!child.activeInHierarchy
                && !this.Contains(child)
                && ((child.GetComponent<Character>() != null && child.GetComponent<Character>().stats.hasRespawn)
                || child.GetComponent<Character>() == null)) this.respawnObjects.Add(new RespawnTimer(child));
        }
    }

    private void UpdateRespawnObjects()
    {
        for (int i = 0; i < this.respawnObjects.Count; i++)
        {
            if (!this.respawnObjects[i].spawnIt) respawnObjects[i].Updating(this.updateTime);
        }
    }

    private void RespawnAll()
    {
        for (int i = 0; i < this.respawnObjects.Count; i++)
        {
            if (!this.respawnObjects[i].spawnIt) respawnObjects[i].SetSpawnImmediately();
        }
    }

    private void SpawnObjects()
    {
        for (int i = 0; i < this.respawnObjects.Count; i++)
        {
            if (this.respawnObjects[i].spawnIt)
            {
                respawnCharacter(this.respawnObjects[i].gameObject);
                this.respawnObjects[i] = null;
            }
        }
        this.respawnObjects.RemoveAll(x => x == null);
    }

    private bool Contains(GameObject gameObject)
    {
        for(int i = 0; i < this.respawnObjects.Count; i++)
        {
            if(this.respawnObjects[i].gameObject == gameObject) return true;            
        }
        return false;
    }

    private void DespawnCharacter(GameObject gameObject)
    {
        Character character = gameObject.GetComponent<Character>();

        if (character != null)
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
        else
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator InactiveCo(float seconds, GameObject gameObject)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }

    private void respawnCharacter(GameObject gameObject)
    {
        Character character = gameObject.GetComponent<Character>();

        if (character != null)
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
        else
        {
            gameObject.SetActive(true);
        }
    }
}

