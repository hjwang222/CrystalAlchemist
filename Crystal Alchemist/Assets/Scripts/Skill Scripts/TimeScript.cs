using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScript : Script
{
    //SKILL SCRIPT "ZEIT STASE"

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

    public override void onDestroy()
    {
        //Zeit normalisieren wenn Skill zerstört/beendet wird
        isDestroyed = true; 

        foreach(Character character in this.affectedCharacters)
        {
            character.RemoveStatusEffect(this.timeEffect, true);
        }

        //Skills wieder normalisieren
        foreach (Skill skill in this.affectedSkills)
        {
            skill.updateTimeDistortion(0);
        }

        affectedCharacters.Clear();
        affectedSkills.Clear();
    }

    public override void onUpdate()
    {

    }

    public override void onInitialize()
    {
        //Setze Zeitvariable für Statuseffekt

        foreach (StatusEffect effect in this.skill.statusEffects)
        {
            if (effect.script != null)
            {
                effect.script.setValue(this.TimeDistortion);
            }
        }
    }

    public override void onStay(Collider2D hittedCharacter)
    {        
        //if (Utilities.checkCollision(hittedCharacter, this.skill) && !isDestroyed)
        //    setTimeDistorion(hittedCharacter.gameObject, this.TimeDistortion);
    }

    public override void onExit(Collider2D hittedCharacter)
    {
        //Normalisiere Zeit beim Austritt aus dem Feld
        if (Utilities.checkCollision(hittedCharacter, this.skill))
            removeTimeDistorion(hittedCharacter.gameObject);
    }

    public override void onEnter(Collider2D hittedCharacter)
    {
        //Setze Zeit beim Eintritt in das Feld
        if (Utilities.checkCollision(hittedCharacter, this.skill) && !isDestroyed)
            setTimeDistorion(hittedCharacter.gameObject, this.TimeDistortion);
    }

    private void removeTimeDistorion(GameObject hittedCharacter)
    {
        Skill skill = hittedCharacter.GetComponent<Skill>();

        if (skill != null)
        {
            skill.updateTimeDistortion(0);
            this.affectedSkills.Remove(skill);
        }

        Character character = hittedCharacter.GetComponent<Character>();

        if (character != null)
        {
            character.RemoveStatusEffect(this.timeEffect, true);
            this.affectedCharacters.Remove(character);
        }
    }

    private void setTimeDistorion(GameObject hittedCharacter, float destortion)
    {
        Character character = hittedCharacter.GetComponent<Character>();
        Skill skill = hittedCharacter.GetComponent<Skill>();

        if (character != null)
        {
            character.AddStatusEffect(this.timeEffect);
            this.affectedCharacters.Add(character);
        }

        if (skill != null)
        {
            skill.updateTimeDistortion(destortion);
            this.affectedSkills.Add(skill);
        }
    }
}
