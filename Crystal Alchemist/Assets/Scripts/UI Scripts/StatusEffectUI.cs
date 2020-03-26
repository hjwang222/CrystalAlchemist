﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusEffectUI : MonoBehaviour
{
    [SerializeField]
    private Image iconUI;

    [SerializeField]
    private TextMeshProUGUI textfieldUI;

    [SerializeField]
    private SpriteRenderer icon;

    [SerializeField]
    private TextMeshPro textfield;

    private StatusEffect statusEffect;

    public void setUI(StatusEffect statusEffect)
    {
        this.statusEffect = statusEffect;
        if (this.icon != null) this.icon.sprite = this.statusEffect.iconSprite;
        if (this.iconUI != null) this.iconUI.sprite = this.statusEffect.iconSprite;
    }

    public void updateUI()
    {
        string seconds = FormatUtil.setDurationToString(statusEffect.getTimeLeft());
        if (statusEffect.getTimeLeft() <= 0 || !statusEffect.hasDuration) seconds = "";
        if (this.textfield != null) this.textfield.text = seconds;
        if (this.textfieldUI != null) this.textfieldUI.text = seconds;
    }

    private void LateUpdate()
    {
        if (this.statusEffect == null) Destroy(this.gameObject);
    }
}
