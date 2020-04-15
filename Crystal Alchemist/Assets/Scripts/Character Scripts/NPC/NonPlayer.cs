using UnityEngine;
using Sirenix.OdinInspector;

public class NonPlayer : Character
{
    private void Start()
    {
        //TODO: Analyse Protection
        AnalyseInfo analyse = Instantiate(GlobalGameObjects.analyseInfo, this.transform.position, Quaternion.identity, this.transform);
        analyse.SetTarget(this.gameObject);
        analyse.gameObject.SetActive(false);
    }
}
