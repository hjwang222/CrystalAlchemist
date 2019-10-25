using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameRound : MonoBehaviour
{
    [SerializeField]
    private BoolSignal successSignal;

    [SerializeField]
    private SimpleSignal endSignal;

    private int roundNumber = 1;
    private float maxDuration;
    private float elapsed;
    private int difficulty = 1;
    private MiniGameUI ui;

    public void setParameters(float time, int round, int difficulty)
    {
        this.maxDuration = time;
        this.roundNumber = round;
        this.difficulty = difficulty;
    }

    public virtual void Start()
    {
        this.elapsed = this.maxDuration;
    }

    private void Update()
    {
        if(elapsed <= 0)
        {
            endRound();
        }
        else
        {
            this.elapsed -= Time.deltaTime;
        }
    }

    public int getSeconds()
    {
        return (int)this.elapsed;
    }

    public void setSuccess(bool value) //SIGNAL
    {
        this.successSignal.Raise(value);
        endRound();
    }

    public void endRound()
    {
        this.endSignal.Raise();
    }
}
