using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameRound : MonoBehaviour
{
    public List<GameObject> buttons = new List<GameObject>();

    private int roundNumber = 1;
    private float maxDuration;
    private float elapsed;
    [HideInInspector]
    public int difficulty = 1;
    private MiniGameUI ui;
    

    public void setParameters(float time, int round, int difficulty, myCursor cursor, MiniGameUI ui)
    {
        this.maxDuration = time;
        this.roundNumber = round;
        this.difficulty = difficulty;
        this.ui = ui;

        foreach(GameObject button in this.buttons)
        {
            if (button.GetComponent<ButtonExtension>() != null)
                button.GetComponent<ButtonExtension>().setCursor(cursor);
        }
    }

    public virtual void Start()
    {
        this.elapsed = this.maxDuration;
    }

    private void Update()
    {
        if(elapsed <= 0)
        {
            setMarkAndEndRound(false);
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

    public void setMarkAndEndRound(bool value) 
    {
        this.ui.setMarkAndEndRound(value);
    }

    public virtual void checkIfWon()
    {
 
    }
}
