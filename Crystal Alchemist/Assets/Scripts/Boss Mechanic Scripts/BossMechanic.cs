using System.Collections.Generic;
using UnityEngine;


public class BossMechanic : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> variants = new List<GameObject>(); //for initialize and random

    [SerializeField]
    private float destroyAfter = 5f;

    private void Start()
    {
        int index = Random.Range(0, variants.Count);
        variants[index].SetActive(true);

        Destroy(this.gameObject, this.destroyAfter);
    }

    public void Initialize(Character sender, Character target)
    {
        List<BossMechanicProperty> properties = new List<BossMechanicProperty>();
        UnityUtil.GetChildObjects<BossMechanicProperty>(this.transform, properties);

        foreach(BossMechanicProperty property in properties)
        {
            property.Initialize(sender, target);
        }
    }

    #region Trigger

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }

    #endregion
}
