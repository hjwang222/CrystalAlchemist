using UnityEngine;
using Sirenix.OdinInspector;

public class NonPlayer : Character
{
    [SerializeField]
    [BoxGroup("Analyse")]
    private bool addAnalyse = true;

    private void Start()
    {
        AnalyseInfo analyse = Instantiate(GlobalGameObjects.analyseInfo, this.transform.position, Quaternion.identity, this.transform);
        analyse.SetTarget(this.gameObject);
    }
}
