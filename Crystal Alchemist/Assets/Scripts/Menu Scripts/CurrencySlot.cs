using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencySlot : MonoBehaviour
{
    [SerializeField]
    private string itemGroup;
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

    private void Awake()
    {
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
        this.audioSource = this.transform.gameObject.AddComponent<AudioSource>();
        this.audioSource.loop = false;
        this.audioSource.playOnAwake = false;
    }

    public void updateCurrency()
    {
        int newValue = Utilities.getAmountFromInventory(this.itemGroup, this.player.inventory, false);

        if (this.currentValue != newValue && this.playOnce)
        {            
            Utilities.playSoundEffect(this.audioSource, this.raiseSoundEffect);
        }

        StartCoroutine(Countdown(newValue));
    }

    private IEnumerator Countdown(int newValue)
    {
        while (this.currentValue != newValue)
        {
            int rate = -1;
            if (newValue - this.currentValue > 0) rate = 1;

            this.currentValue += rate;
            if ((rate > 0 && this.currentValue >= newValue)
                || (rate < 0 && this.currentValue <= 0)) this.currentValue = newValue;

            this.textField.text = Utilities.formatString(this.currentValue, this.maxValue);
            
            yield return new WaitForSeconds(this.counterDelay);
        }

        this.playOnce = true;
    }


}
