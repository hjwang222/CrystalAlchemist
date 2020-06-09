using Sirenix.OdinInspector;
using UnityEngine;

public class BossMechanicParent : BossMechanicProperty
{
    [SerializeField]
    [HideLabel]
    [BoxGroup("Main")]
    private SequenceProperty selfProperty;



    private void Start()
    {
        this.selfProperty.AddSpawnPoints(this.transform);
        this.transform.rotation = this.GetRotation(this.selfProperty.rotationType, this.selfProperty.rotationFactor, this.selfProperty.GetOffset());
        this.transform.position = GetSpawnPosition(this.selfProperty).transform.position;
    }
   
}
