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


    #region setInfo

    private void setInfo(Skill skill)
    {
        this.additionalInfo.SetActive(false);

        this.nameField.text = CustomUtilities.Format.getLanguageDialogText(skill.skillName, skill.skillNameEnglish);

        if (skill.GetComponent<SkillBookModule>() != null)
        {
            this.previewImage.sprite = skill.GetComponent<SkillBookModule>().icon;
            this.descriptionField.text = CustomUtilities.Format.getLanguageDialogText(skill.GetComponent<SkillBookModule>().skillDescription, skill.GetComponent<SkillBookModule>().skillDescriptionEnglish);
        }

        if (skill.GetComponent<SkillTargetModule>() != null && skill.GetComponent<SkillTargetModule>().statusEffects.Count > 0)
        {
            this.additionalInfo.SetActive(true);

            StatusEffect statusEffect = skill.GetComponent<SkillTargetModule>().statusEffects[0];

            this.statusEffectPreviewImage.sprite = statusEffect.iconSprite;
            this.statusEffectNameField.text = CustomUtilities.Format.getLanguageDialogText(statusEffect.statusEffectName, statusEffect.statusEffectNameEnglish);
            this.statusEffectDescriptionField.text = CustomUtilities.Format.getLanguageDialogText(statusEffect.statusEffectDescription, statusEffect.statusEffectDescriptionEnglish);
        }
    }


    private void setInfo(Item item)
    {
        this.additionalInfo.SetActive(false);

        CustomUtilities.Items.setItemImage(this.previewImage, item);

        this.nameField.text = CustomUtilities.Format.getLanguageDialogText(item.itemName, item.itemNameEnglish);
        this.descriptionField.text = CustomUtilities.Format.getLanguageDialogText(item.itemDescription, item.itemDescriptionEnglish);
    }

    private void setInfo(CharacterAttributeStats stats)
    {
        this.additionalInfo.SetActive(false);

        this.previewImage.sprite = stats.icon.sprite;
        this.nameField.text = CustomUtilities.Format.getLanguageDialogText(stats.attributeName, stats.nameEnglish);
        this.descriptionField.text = CustomUtilities.Format.getLanguageDialogText(stats.description, stats.descriptionEnglish);
    }

    private void setInfo(MapPagePoint mapPoint)
    {
        this.additionalInfo.SetActive(false);

        this.previewImage.sprite = mapPoint.areaSprite;
        this.nameField.text = CustomUtilities.Format.getLanguageDialogText(mapPoint.areaName, mapPoint.areaNameEnglish);
        this.descriptionField.text = CustomUtilities.Format.getLanguageDialogText(mapPoint.areaDescription, mapPoint.areaDescriptionEnglish);
    }

    #endregion


    #region show and hide

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Show(Skill skill)
    {
        this.gameObject.SetActive(true);
        setInfo(skill);
    }

    public void Show(Item item)
    {
        this.gameObject.SetActive(true);
        setInfo(item);
    }

    public void Show(CharacterAttributeStats stats)
    {
        this.gameObject.SetActive(true);
        setInfo(stats);
    }

    public void Show(MapPagePoint mapPoint)
    {
        this.gameObject.SetActive(true);
        setInfo(mapPoint);
    }
    #endregion
}
