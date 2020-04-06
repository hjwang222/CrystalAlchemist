using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

#region Attributes

public enum modificationType
{
    none,
    target,
    sender,
    point,
    randomArea,
    randomPoints,
    normalized
}

[System.Serializable]
public class customSequence
{
    public GameObject gameObject;

    public modificationType position = modificationType.none;
    public float spawnDelay = 0f;

    [ShowIf("position", modificationType.point)]
    public GameObject spawnPoint;
    [ShowIf("position", modificationType.randomArea)]
    public GameObject min;
    [ShowIf("position", modificationType.randomArea)]
    public GameObject max;
    [ShowIf("position", modificationType.randomPoints)]
    public List<GameObject> spawnPoints = new List<GameObject>();

    public modificationType rotation = modificationType.none;
    [ShowIf("rotation", modificationType.randomPoints)]
    [Range(1, 8)]
    public int randomRotations;
}

#endregion

public class SkillSequence : MonoBehaviour
{
    [SerializeField]
    private Character sender;

    [SerializeField]
    private Character target;

    [SerializeField]
    private List<customSequence> modifcations = new List<customSequence>();

    private bool useDurationTime = false;
    private float duration = 0;
    private float timeElapsed = 0;

    private void Start()
    {
        setChildObjects();
        initModification();
    }

    private void Update()
    {
        this.timeElapsed += Time.deltaTime;

        if ((this.useDurationTime && this.timeElapsed >= this.duration) || this.noActiveGameObjects())
        {
            this.DestroyIt();
        }

        activateGameObject();
    }

    private bool noActiveGameObjects()
    {
        foreach(customSequence custom in this.modifcations)
        {
            if (custom.gameObject != null) return false;
        }

        return true;
    }

    private void activateGameObject()
    {
        foreach(customSequence mod in this.modifcations)
        {
            if (mod.gameObject != null && this.timeElapsed >= mod.spawnDelay)
            {
                //Instantiate Skill
            }
        }
    }

    /////////////////////////////////////////

    #region Init

    private void initModification()
    {
        foreach (customSequence modification in this.modifcations)
        {
            setPosition(modification);
            setRotation(modification);
        }
    }

    private void setPosition(customSequence modification)
    {
        switch (modification.position)
        {
            case modificationType.point: modification.gameObject.transform.position = modification.spawnPoint.transform.position; break;
            case modificationType.sender: setPositionAtCharacter(modification, this.sender); break;
            case modificationType.target: setPositionAtCharacter(modification, this.target); break;
            case modificationType.randomArea: modification.gameObject.transform.position = getRandomPosition(modification.min.transform.position, modification.max.transform.position); break;
            case modificationType.randomPoints: modification.gameObject.transform.position = getRandomPosition(modification.spawnPoints); break;
        }
    }

    private void setPositionAtCharacter(customSequence modification, Character character)
    {
        modification.gameObject.transform.position = character.transform.position;
        if (character.shadowRenderer != null) modification.gameObject.transform.position = character.shadowRenderer.transform.position;
    }

    private void setRotation(customSequence modification)
    {
        if (modification.rotation == modificationType.randomPoints)
        {
            modification.gameObject.transform.rotation = Quaternion.Euler(0, 0, getRandomRotation(modification.randomRotations));
        }
        else if (modification.rotation == modificationType.target)
        {
            Vector2 direction = (this.target.transform.position - modification.gameObject.transform.position).normalized;
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

            modification.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else if (modification.rotation == modificationType.normalized)
        {
            modification.gameObject.transform.rotation = Quaternion.identity;
        }
    }

    private void setChildObjects()
    {
        if (this.sender != null)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                Skill childSkill = this.transform.GetChild(i).GetComponent<Skill>();
                if (childSkill != null)
                {
                    childSkill.sender = this.sender;
                    childSkill.target = this.target;
                    childSkill.overridePosition = false;
                }
            }
        }
    }

    #endregion

    /////////////////////////////////////////

    #region Trigger

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }

    #endregion

    /////////////////////////////////////////

    #region Random Functions

    public void setSender(Character sender)
    {
        this.sender = sender;
    }

    public void setTarget(Character target)
    {
        this.target = target;
    }

    private int getRandomRotation(int randomRotations)
    {
        int rng = Random.Range(1, randomRotations);
        int result = (360 / randomRotations) * rng;
        return result;
    }

    private Vector2 getRandomPosition(List<GameObject> positions)
    {
        int rng = Random.Range(0, positions.Count - 1);
        return positions[rng].transform.position;
    }

    private Vector2 getRandomPosition(Vector2 min, Vector2 max)
    {
        float x = Random.Range(min.x, max.x);
        float y = Random.Range(min.y, max.y);

        return new Vector2(x, y);
    }

    #endregion

}
