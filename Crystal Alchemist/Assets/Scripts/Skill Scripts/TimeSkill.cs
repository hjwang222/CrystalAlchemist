using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSkill : StandardSkill
{
    #region Attributes
    private List<Character> affectedCharacters = new List<Character>();
    private List<StandardSkill> affectedSkills = new List<StandardSkill>();

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
    public override void DestroyIt()
    {        
        //Zeit normalisieren wenn Skill zerstört/beendet wird
        isDestroyed = true; 

        foreach(Character character in this.affectedCharacters)
        {
            Utilities.StatusEffectUtil.RemoveStatusEffect(this.timeEffect, true, character);
        }

        //Skills wieder normalisieren
        foreach (StandardSkill skill in this.affectedSkills)
        {
            skill.updateTimeDistortion(0);
        }

        affectedCharacters.Clear();
        affectedSkills.Clear();
        base.DestroyIt();
    }

    public override void init()
    {
        base.init();
        foreach (TimeDistortion effect in this.statusEffects)
        {
            effect.time = this.TimeDistortion;            
        }
    }

    public override void OnTriggerExit2D(Collider2D hittedCharacter)
    {
        base.OnTriggerExit2D(hittedCharacter);
        //Normalisiere Zeit beim Austritt aus dem Feld
        if (Utilities.Collisions.checkCollision(hittedCharacter, this))
            removeTimeDistorion(hittedCharacter.gameObject);
    }

    public override void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        base.OnTriggerEnter2D(hittedCharacter);
        //Setze Zeit beim Eintritt in das Feld
        if (Utilities.Collisions.checkCollision(hittedCharacter, this) && !isDestroyed)
            setTimeDistorion(hittedCharacter.gameObject, this.TimeDistortion);
    }

    #endregion


    #region Functions (private)
    private void removeTimeDistorion(GameObject hittedCharacter)
    {
        StandardSkill skill = hittedCharacter.GetComponent<StandardSkill>();

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
        StandardSkill skill = hittedCharacter.GetComponent<StandardSkill>();

        if (character != null)
        {
            Utilities.StatusEffectUtil.AddStatusEffect(this.timeEffect, character);
            this.affectedCharacters.Add(character);
        }

        if (skill != null)
        {
            skill.updateTimeDistortion(destortion);
            this.affectedSkills.Add(skill);
        }
    }
    #endregion
}
