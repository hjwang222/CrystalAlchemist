using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CastBar : MonoBehaviour
{
    public Character target;
    public Image charging;
    public Image full;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI percentage;
    public Skill skill;

    private void Start()
    {
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 1f);
        this.skillName.text = CustomUtilities.Format.getLanguageDialogText(this.skill.skillName, this.skill.skillNameEnglish);
        this.percentage.text = "0%";
    }

    public void showCastBar()
    {
        if (this.skill.cast > 0)
        {
            float percent = skill.holdTimer / this.skill.cast;
            this.charging.fillAmount = (percent);

            string text = (int)(percent * 100) + "%";
            if (percent * 100 >= 100) text = "BEREIT!";

            this.percentage.text = text;

            if (skill.holdTimer >= this.skill.cast) this.full.enabled = true;
            else this.full.enabled = false;            
        }
    }

    public void destroyIt()
    {
        target.activeCastbar = null;
        Destroy(this.gameObject, 0.1f);
    }
}
