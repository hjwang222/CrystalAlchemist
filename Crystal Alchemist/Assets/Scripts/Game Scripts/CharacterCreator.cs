using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

public enum Races
{
    human,
    elves,
    lamia,
    ponies
}

[System.Serializable]
public struct Race
{
    public Races race;
    public List<GameObject> parts;
}

public class CharacterCreator : MonoBehaviour
{
    [FoldoutGroup("Internal", Expanded = false)]
    [SerializeField]
    private List<SpriteRenderer> bodyParts = new List<SpriteRenderer>();

    [FoldoutGroup("Internal", Expanded = false)]
    [SerializeField]
    private List<SpriteRenderer> hairParts = new List<SpriteRenderer>();

    [FoldoutGroup("Internal", Expanded = false)]
    [SerializeField]
    private SpriteRenderer eyePart = new SpriteRenderer();

    [FoldoutGroup("Internal", Expanded = false)]
    [SerializeField]
    private List<Race> races = new List<Race>();

    [FoldoutGroup("Internal", Expanded = false)]
    [SerializeField]
    private List<GameObject> parts = new List<GameObject>();

    [FoldoutGroup("Internal", Expanded = false)]
    [SerializeField]
    private AnimationClip anim;

    [BoxGroup]
    [SerializeField]
    private Color hairColor;

    [BoxGroup]
    [SerializeField]
    private Color eyeColor;

    [BoxGroup]
    [SerializeField]
    private Color bodyColor;

    [BoxGroup]
    [SerializeField]
    private Races race;


#if UNITY_EDITOR
    [Button]
    public void setColor()
    {
        foreach(GameObject temp in this.parts)
        {
            temp.SetActive(false);
        }

        foreach(Race temp in this.races)
        {
            if(temp.race == this.race)
            {
                foreach (GameObject temp1 in temp.parts)
                {
                    temp1.SetActive(true);
                }
            }
        }

        foreach(SpriteRenderer temp in this.bodyParts)
        {
            temp.color = this.bodyColor;
        }

        foreach (SpriteRenderer temp in this.hairParts)
        {
            temp.color = this.hairColor;
        }

        this.eyePart.color = this.eyeColor;
    }

    [Button]
    public void test()
    {
        addProperties();
    }
#endif


    public void addProperties()
    {
        List<GameObject> temp = new List<GameObject>();

        foreach(Transform child in this.transform)
        {
            foreach (Transform child2 in child)
            {
                foreach (Transform child3 in child2)
                {
                    if (child3.GetComponent<SpriteRenderer>() != null) temp.Add(child3.gameObject);
                }
            }
        }

        Object[] sprite = AssetDatabase.LoadAllAssetsAtPath("Assets/Art/Graphics/Characters/Player Sprites/Legs.png");

        foreach (GameObject blub in temp)
        {
            EditorCurveBinding spriteBinding = new EditorCurveBinding();
            spriteBinding.type = typeof(SpriteRenderer);
            spriteBinding.path = GetGameObjectPath(blub.transform);
            spriteBinding.propertyName = "m_Sprite";

            ObjectReferenceKeyframe[] spriteKeyFrames = new ObjectReferenceKeyframe[sprite.Length];
            
            for (int i = 0; i < (sprite.Length); i++)
            {
                spriteKeyFrames[i] = new ObjectReferenceKeyframe();
                spriteKeyFrames[i].time = i;
                spriteKeyFrames[i].value = sprite[i];
            }

            AnimationUtility.SetObjectReferenceCurve(this.anim, spriteBinding, spriteKeyFrames);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    private string GetGameObjectPath(Transform transform)
    {
        string path = transform.name;
        while (transform.parent != null && transform.parent != this.transform)
        {
            transform = transform.parent;
            path = transform.name + "/" + path;
        }
        return path;
    }

}
