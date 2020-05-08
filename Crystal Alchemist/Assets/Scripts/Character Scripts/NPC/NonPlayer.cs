using UnityEngine;

public class NonPlayer : Character
{
    public override void Start()
    {
        //TODO: Analyse Protection
        AnalyseInfo analyse = Instantiate(MasterManager.analyseInfo, this.transform.position, Quaternion.identity, this.transform);
        analyse.SetTarget(this.gameObject);
        analyse.gameObject.SetActive(false);
    }
}
