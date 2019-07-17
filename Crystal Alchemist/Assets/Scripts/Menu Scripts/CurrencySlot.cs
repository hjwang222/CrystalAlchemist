using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencySlot : MonoBehaviour
{
    [SerializeField]
    private Item item;
    [SerializeField]
    private int maxValue;
    [SerializeField]
    private float counterDelay = 0.05f;
    [SerializeField]
    private TextMeshProUGUI textField;
    [SerializeField]
    private AudioClip raiseSoundEffect;
    private Player player;

    private AudioSource audioSource;
    private bool playOnce = false;
    private int currentValue;
    private bool isRunning = false;

    private int newValue;

    private void Awake()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
        this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;
    }

    public void updateCurrency()
    {        
        this.newValue = Utilities.Items.getAmountFromInventory(this.item, this.player.inventory, false);

        if (!this.playOnce)
        {
            this.playOnce = true;
            Utilities.Audio.playSoundEffect(this.audioSource, this.raiseSoundEffect);
        }

        if(!this.isRunning) StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {        
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
                this.playOnce = false;
                this.textField.text = Utilities.Format.formatString(this.currentValue, this.maxValue);
                break;
            }   

            this.textField.text = Utilities.Format.formatString(this.currentValue, this.maxValue);
            
            yield return new WaitForSeconds(this.counterDelay);
        }

        this.isRunning = false;
        this.playOnce = false;
    }
}
