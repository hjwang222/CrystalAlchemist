using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencySlot : MonoBehaviour
{
    [SerializeField]
    private PlayerStats playerStats;

    [SerializeField]
    private Item item;
    [SerializeField]
    private int maxValue;
    [SerializeField]
    private TextMeshProUGUI textField;
    [SerializeField]
    private AudioClip raiseSoundEffect;
    [SerializeField]
    private FloatSignal hideSignal;
    [SerializeField]
    private float hideDelay = 3f;


    private Player player;

    private AudioSource audioSource;
    //private bool playOnce = false;
    private int currentValue;
    private bool isRunning = false;
    private bool playSound = false;
    private int newValue;

    private void Start()
    {
        this.player = this.playerStats.player;
        this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;

        updateCurrency();

        playSound = true;
    }

    public bool isItRunning()
    {
        return this.isRunning;
    }

    public void updateCurrency()
    {
        this.newValue = CustomUtilities.Items.getAmountFromInventory(this.item, this.player.inventory);

        if (this.playSound)
        {
            //this.playOnce = true;
            CustomUtilities.Audio.playSoundEffect(this.audioSource, this.raiseSoundEffect);
        }

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
                this.hideSignal.Raise(this.hideDelay);
                //this.playOnce = false;
                this.textField.text = CustomUtilities.Format.formatString(this.currentValue, this.maxValue);
                break;
            }   

            this.textField.text = CustomUtilities.Format.formatString(this.currentValue, this.maxValue);
            
            yield return new WaitForSeconds(counterDelay);
        }

        this.isRunning = false;
        
        //this.playOnce = false;
    }
}
