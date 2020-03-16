using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusEffectUI : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private TextMeshProUGUI textfield;

    private StatusEffect statusEffect;

    public void setUI(StatusEffect statusEffect)
    {
        this.statusEffect = statusEffect;
        this.icon.sprite = this.statusEffect.iconSprite;
    }

    public void updateUI()
    {
        string seconds = CustomUtilities.Format.setDurationToString(statusEffect.getTimeLeft());
        if (statusEffect.getTimeLeft() <= 0 || statusEffect.maxDuration == CustomUtilities.maxFloatInfinite) seconds = "";
        this.textfield.text = seconds;        
    }

    private void LateUpdate()
    {
        if (this.statusEffect == null) Destroy(this.gameObject);
    }
}
