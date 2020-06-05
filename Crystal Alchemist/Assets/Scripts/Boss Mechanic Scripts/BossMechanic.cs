using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class BossMechanic : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> variants = new List<GameObject>(); //for initialize and random

    [SerializeField]
    private float destroyAfter = 5f;

    [BoxGroup("Debug")]
    [SerializeField]
    private Character sender;

    [BoxGroup("Debug")]
    [SerializeField]
    private Character target;

    private void Awake()
    {
        for (int i = 0; i < this.variants.Count; i++) variants[i].SetActive(false);
    }

    private void Start()
    {     
        int index = Random.Range(0, variants.Count);
        variants[index].SetActive(true);

        List<BossMechanicProperty> properties = new List<BossMechanicProperty>();
        UnityUtil.GetChildObjects<BossMechanicProperty>(this.variants[index].transform, properties);

        foreach (BossMechanicProperty property in properties) property.Initialize(this.sender, this.target);

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
