using UnityEngine;
using DG.Tweening;
using System.Collections;

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

    [SerializeField]
    private Collider2D coll;

    private bool isRunning = false;
    private Vector2 position;

    public override void Start()
    {
        base.Start();
        this.decke.SetActive(false);
    }

    public override void DoOnSubmit()
    {
        if (!this.isRunning)
        {           
            this.position = this.player.transform.position;
            Vector2 position = new Vector2(this.transform.position.x, this.transform.position.y+offset);
            StartCoroutine(MoveCo(position, 1f));

            this.player.updateSpeed(-100, true);
            this.time.factor = this.newValue;
            this.isRunning = true;            
        }
        else
        {
            this.player.updateSpeed(0, true);
            this.time.factor = this.time.normalFactor;
            this.isRunning = false;
            StartCoroutine(MoveCo(this.position, 1f));
        }

        this.decke.SetActive(isRunning);
    }

    private IEnumerator MoveCo(Vector2 position, float delay)
    {
        this.coll.enabled = false;
        this.player.values.currentState = CharacterState.respawning;
        this.player.transform.DOMove(position, delay);
        yield return new WaitForSeconds(delay);
        this.player.values.currentState = CharacterState.interact;
        this.coll.enabled = true;
    }
}
