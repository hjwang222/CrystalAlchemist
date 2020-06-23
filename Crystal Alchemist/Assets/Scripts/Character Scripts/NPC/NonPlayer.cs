using UnityEngine;

public class NonPlayer : Character
{
    [HideInInspector]
    public bool IsSummoned = false;

    public override void Start()
    {
        if (this.IsSummoned)
        {
            SetCharacterSprites(false);
            SpawnOut();
        }
        
        base.Start();

        if (this.stats.showAnalyse)
        {
            AnalyseInfo analyse = Instantiate(MasterManager.analyseInfo, this.transform.position, Quaternion.identity, this.transform);
            analyse.SetTarget(this.gameObject);
        }

        if (this.IsSummoned)
        {
            SetCharacterSprites(true);
            PlayRespawnAnimation();
            SpawnIn();
        }
    }
}
