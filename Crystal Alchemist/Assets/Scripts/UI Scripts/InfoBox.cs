using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InfoBox : MonoBehaviour
{
    [SerializeField]
    private Image previewImage;

    [SerializeField]
    private TextMeshProUGUI nameField;

    [SerializeField]
    private TextMeshProUGUI descriptionField;

    [SerializeField]
    private GameObject additionalInfo;

    [SerializeField]
    private Image statusEffectPreviewImage;

    [SerializeField]
    private TextMeshProUGUI statusEffectNameField;

    [SerializeField]
    private TextMeshProUGUI statusEffectDescriptionField;


    private void setInfo(StandardSkill skill)
    {
        this.additionalInfo.SetActive(false);

        this.previewImage.sprite = skill.icon;
        this.nameField.text = Utilities.Format.getLanguageDialogText(skill.skillName, skill.skillNameEnglish);
        this.descriptionField.text = Utilities.Format.getLanguageDialogText(skill.skillDescription, skill.skillDescriptionEnglish);

        if(skill.statusEffects.Count > 0)
        {
            this.additionalInfo.SetActive(true);

            StatusEffect statusEffect = skill.statusEffects[0];

            this.statusEffectPreviewImage.sprite = statusEffect.iconSprite;
            this.statusEffectNameField.text = Utilities.Format.getLanguageDialogText(statusEffect.statusEffectName, statusEffect.statusEffectNameEnglish);
            this.statusEffectDescriptionField.text = Utilities.Format.getLanguageDialogText(statusEffect.statusEffectDescription, statusEffect.statusEffectDescriptionEnglish);
        }
    }


    private void setInfo(Item item)
    {
        this.additionalInfo.SetActive(false);

        this.previewImage.sprite = item.itemSpriteInventory;
        this.nameField.text = Utilities.Format.getLanguageDialogText(item.itemName, item.itemNameEnglish);
        this.descriptionField.text = Utilities.Format.getLanguageDialogText(item.itemDescription, item.itemDescriptionEnglish);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show(StandardSkill skill)
    {
        this.gameObject.SetActive(true);
        setInfo(skill);
    }

    public void Show(Item item)
    {
        this.gameObject.SetActive(true);
        setInfo(item);
    }


}
