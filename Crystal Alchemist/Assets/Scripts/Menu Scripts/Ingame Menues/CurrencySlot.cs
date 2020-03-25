using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencySlot : MonoBehaviour
{
    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private ItemGroup item;
    [SerializeField]
    private int maxValue;
    [SerializeField]
    private TextMeshProUGUI textField;
    [SerializeField]
    private AudioClip raiseSoundEffect;
    [SerializeField]
    private FloatSignal hideSignal;



    private Player player;

    //private bool playOnce = false;
    private int currentValue;
    private bool isRunning = false;
    private bool playSound = false;
    private int newValue;

    private void Start()
    {
        this.player = this.playerStats.player;

        updateCurrency();
        playSound = true;
    }

    public bool isItRunning()
    {
        return this.isRunning;
    }

    public void updateCurrency()
    {
        this.newValue = this.player.GetComponent<PlayerItems>().GetAmount(this.item);

        if (this.playSound) AudioUtil.playSoundEffect(this.raiseSoundEffect);       

        if(!this.isRunning) StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        int change = Mathf.Abs(this.currentValue - this.newValue);
        float counterDelay = 1;
        if (change != 0) counterDelay = 0.01f / (float)change;
        this.isRunning = true;

        while (this.currentValue != this.newValue)
        {
            int rate = -1;
            if (this.newValue - this.currentValue > 0) rate = 1;

            this.currentValue += rate;
            if (((rate > 0 && this.currentValue >= this.newValue)
                || (rate < 0 && this.currentValue <= 0)))
            {
                this.currentValue = this.newValue;
                this.isRunning = false;
                this.hideSignal.Raise(3f);
                this.textField.text = FormatUtil.formatString(this.currentValue, this.maxValue);
                break;
            }   

            this.textField.text = FormatUtil.formatString(this.currentValue, this.maxValue);
            
            yield return new WaitForSeconds(counterDelay);
        }

        this.isRunning = false;
    }
}
