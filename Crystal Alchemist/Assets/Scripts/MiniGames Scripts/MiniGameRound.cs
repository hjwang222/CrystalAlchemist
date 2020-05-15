using System.Collections.Generic;
using UnityEngine;

public class MiniGameRound : MonoBehaviour
{
    public List<GameObject> buttons = new List<GameObject>();

    [HideInInspector]
    public CustomCursor cursor;

    private int roundNumber = 1;
    private float maxDuration;
    private float elapsed;
    [HideInInspector]
    public int difficulty = 1;
    private MiniGameUI ui;
    private bool isTimerStopped = true;

    public void setParameters(float time, int round, int difficulty, CustomCursor cursor, MiniGameUI ui)
    {
        this.maxDuration = time;
        this.roundNumber = round;
        this.difficulty = difficulty;
        this.ui = ui;
        this.cursor = cursor;

        foreach(GameObject button in this.buttons)
        {
            if (button.GetComponent<ButtonExtension>() != null)
                button.GetComponent<ButtonExtension>().SetCursor(this.cursor);
        }
    }

    public virtual string getDifficulty(string text, int difficulty)
    {
        return text;
    }

    public void enableInputs(bool value)
    {
        foreach (GameObject button in this.buttons)
        {
            button.SetActive(value);
        }
        this.cursor.gameObject.SetActive(value);
        this.isTimerStopped = !value;
    }


    public virtual void Start()
    {
        enableInputs(false);
        this.elapsed = this.maxDuration;
    }

    private void Update()
    {
        if (this.isTimerStopped) return;

        if(elapsed <= 0)
        {
            setMarkAndEndRound(false);
        }
        else
        {
            this.elapsed -= Time.deltaTime;
        }
    }

    public void stopTimer()
    {
        this.isTimerStopped = true;
    }

    public float getSeconds()
    {
        return this.elapsed;
    }

    public void setMarkAndEndRound(bool value) 
    {
        this.ui.setMarkAndEndRound(value);
    }

    public virtual void checkIfWon()
    {
 
    }
}
