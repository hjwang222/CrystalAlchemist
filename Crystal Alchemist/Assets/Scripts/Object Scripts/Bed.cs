using UnityEngine;

public class Bed : Interactable
{
    [SerializeField]
    private TimeValue time;

    [SerializeField]
    private float newValue;

    [SerializeField]
    private float offset = 0.35f;

    [SerializeField]
    private GameObject decke;

    private Vector2 position;
    private bool isSleeping;

    public override void Start()
    {
        base.Start();
        this.decke.SetActive(false);
    }

    //Stop music, Play other music

    public override void DoOnSubmit()
    {
        if (!this.isSleeping)
        {
            this.position = this.player.transform.position;
            Vector2 position = new Vector2(this.transform.position.x, this.transform.position.y + offset);

            GameEvents.current.DoSleep(position, () => this.decke.SetActive(true), () => { this.time.SetFactor(this.newValue); this.isSleeping = true; });
        }
    }

    public override void DoOnUpdate()
    {
        if (Input.anyKeyDown && this.isSleeping)
        {
            GameEvents.current.DoWakeUp(this.position, () => this.time.Reset(), () => { this.decke.SetActive(false); this.isSleeping = false; });            
        }
    }
}
