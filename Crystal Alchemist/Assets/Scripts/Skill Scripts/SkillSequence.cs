using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

#region Attributes

public enum modificationType
{
    none,
    target, 
    sender,
    randomArea,
    random
}

[System.Serializable]
public class customSequence
{
    public GameObject gameObject;

    public modificationType position = modificationType.none;
    [ShowIf("position", modificationType.randomArea)]
    public GameObject max;
    [ShowIf("position", modificationType.randomArea)]
    public GameObject min;
    [ShowIf("position", modificationType.random)]
    public List<GameObject> randomSpawnPositions = new List<GameObject>();

    public modificationType rotation = modificationType.none;
    [ShowIf("rotation", modificationType.random)]
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


    private void Start()
    {
        setChildObjects();
        initModification();
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
        if (modification.position == modificationType.randomArea)
        {
            //set Position in Area
            modification.gameObject.transform.position = getRandomPosition(modification.min.transform.position, modification.max.transform.position);
        }
        else if (modification.position == modificationType.random)
        {
            //set Position of a set of Spawn-Points
            modification.gameObject.transform.position = getRandomPosition(modification.randomSpawnPositions);
        }
        else if (modification.position == modificationType.target)
        {
            //set Position on Target
            modification.gameObject.transform.position = this.target.transform.position;
        }
        else if (modification.position == modificationType.sender)
        {
            //set Position on Sender
            modification.gameObject.transform.position = this.sender.transform.position;
        }
    }

    private void setRotation(customSequence modification)
    {
        if (modification.rotation == modificationType.random)
        {
            modification.gameObject.transform.rotation = Quaternion.Euler(0, 0, getRandomRotation(modification.randomRotations));
        }
        else if (modification.rotation == modificationType.target)
        {
            Vector2 direction = (this.target.transform.position - modification.gameObject.transform.position).normalized;
            float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

            modification.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private void setChildObjects()
    {
        if (this.sender != null)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                StandardSkill childSkill = this.transform.GetChild(i).GetComponent<StandardSkill>();
                if (childSkill != null)
                {
                    childSkill.sender = this.sender;
                    childSkill.setPositionAtStart = false;
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
