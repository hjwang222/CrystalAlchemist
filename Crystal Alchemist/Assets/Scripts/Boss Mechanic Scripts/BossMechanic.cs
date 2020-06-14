using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class BossMechanic : MonoBehaviour
{
    [SerializeField]
    [BoxGroup("Properties")]
    private float destroyAfter = 5f;

    [SerializeField]
    [BoxGroup("Properties")]
    private List<int> patterns = new List<int>();

    [BoxGroup("Debug")]
    [SerializeField]
    private Character sender;

    [BoxGroup("Debug")]
    [SerializeField]
    private Character target;

    private List<GameObject> variants = new List<GameObject>();

    [Button]
    private void AddCharacters()
    {
        this.sender = FindObjectOfType<AI>();
        this.target = FindObjectOfType<Player>();
        this.gameObject.SetActive(false);
    }

    private int GetCount()
    {
        return this.transform.childCount-1;
    }

    public void SetPattern(List<int> pattern)
    {
        this.patterns = pattern;
    }

    private void Awake()
    {
        for(int i = 0; i < this.transform.childCount; i++)
        {
            GameObject child = this.transform.GetChild(i).gameObject;
            this.variants.Add(child);
            child.SetActive(false);            
        }        
    }

    private void Start()
    {
        int index = 0;
        if (this.patterns.Count == 1) index = this.patterns[0];
        else if (this.patterns.Count > 1) index = this.patterns[Random.Range(0, this.patterns.Count)];

        if (index > GetCount()) index = GetCount();
        if (index < 0) index = 0;

        GameObject variant = variants[index];

        foreach(BossMechanicProperty property in variant.GetComponentsInChildren<BossMechanicProperty>(true)) property.Initialize(this.sender, this.target);

        variant.SetActive(true);

        Destroy(this.gameObject, this.destroyAfter);
    }

    public void Initialize(Character sender, Character target)
    {
        this.sender = sender;
        this.target = target;
    }

    #region Trigger

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }

    #endregion
}
