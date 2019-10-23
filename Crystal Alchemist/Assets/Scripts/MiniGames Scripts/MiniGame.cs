using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame : MenuControls
{
    [SerializeField]
    private float maxDuration = 60;

    private float elapsed = 0;
    private List<Item> rewards = new List<Item>();

    private void Start()
    {
        this.elapsed = this.maxDuration;
    }

    public void setRewards(List<Item> loot)
    {
        this.rewards.AddRange(loot);
    }

    public override void Update()
    {
        base.Update();

        if (this.elapsed > 0) this.elapsed -= Time.deltaTime;
        else DestroyIt(); //Time is over!
    }

    public void DestroyIt()
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        this.exitMenu();
    }

}
