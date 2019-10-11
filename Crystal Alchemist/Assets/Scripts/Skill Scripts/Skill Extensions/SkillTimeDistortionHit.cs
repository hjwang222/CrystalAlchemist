using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillTimeDistortionHit : SkillExtension
{
    #region Attributes
    private List<Character> affectedCharacters = new List<Character>();
    private List<Skill> affectedSkills = new List<Skill>();

    [Header("Zeitverzerrungs-Info")]
    [Tooltip("Wert, für Zeitkrümmung. 0 = Stop, 1 = Normal, 2 = Hast")]
    [Range(Utilities.minFloatPercent, Utilities.maxFloatPercent)]
    public float TimeDistortion = 100;
    [Tooltip("StatusEffekt für Zeitverzerrung. Wichtig für Charactere")]
    public StatusEffect timeEffect;

    public float invertColor = 0f;    
    private bool isDestroyed = false;

    #endregion


    #region Overrides
    private void OnDestroy()
    {      
        //Zeit normalisieren wenn Skill zerstört/beendet wird
        isDestroyed = true; 

        foreach(Character character in this.affectedCharacters)
        {
            Utilities.StatusEffectUtil.RemoveStatusEffect(this.timeEffect, true, character);
        }

        //Skills wieder normalisieren
        foreach (Skill skill in this.affectedSkills)
        {
            skill.updateTimeDistortion(0);
        }

        affectedCharacters.Clear();
        affectedSkills.Clear();
    }

    private void OnTriggerExit2D(Collider2D hittedCharacter)
    {
        //Normalisiere Zeit beim Austritt aus dem Feld
        if (Utilities.Collisions.checkCollision(hittedCharacter, this.skill))
            removeTimeDistorion(hittedCharacter.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        //Setze Zeit beim Eintritt in das Feld
        if (Utilities.Collisions.checkCollision(hittedCharacter, this.skill))
            setTimeDistorion(hittedCharacter.gameObject, this.TimeDistortion);
    }

    private void OnTriggerStay2D(Collider2D hittedCharacter)
    {
        //Setze Zeit beim Eintritt in das Feld
        if (Utilities.Collisions.checkCollision(hittedCharacter, this.skill))
            setTimeDistorion(hittedCharacter.gameObject, this.TimeDistortion);
    }

    #endregion


    #region Functions (private)
    private void removeTimeDistorion(GameObject hittedCharacter)
    {
        Skill skill = Utilities.Skills.getSkillByCollision(hittedCharacter);

        if (skill != null)
        {
            skill.updateTimeDistortion(0);
            this.affectedSkills.Remove(skill);
        }

        Character character = hittedCharacter.GetComponent<Character>();

        if (character != null)
        {
            Utilities.StatusEffectUtil.RemoveStatusEffect(this.timeEffect, true, character);
            this.affectedCharacters.Remove(character);
        }
    }

    private void setTimeDistorion(GameObject hittedCharacter, float destortion)
    {
        Character character = hittedCharacter.GetComponent<Character>();
        Skill skill = Utilities.Skills.getSkillByCollision(hittedCharacter);

        if (character != null)
        {
            Utilities.StatusEffectUtil.AddStatusEffect(this.timeEffect, character);
            if (!this.affectedCharacters.Contains(character)) this.affectedCharacters.Add(character);
        }

        if (skill != null)
        {
            skill.updateTimeDistortion(destortion);
            if(!this.affectedSkills.Contains(skill)) this.affectedSkills.Add(skill);
        }
    }
    #endregion
}
