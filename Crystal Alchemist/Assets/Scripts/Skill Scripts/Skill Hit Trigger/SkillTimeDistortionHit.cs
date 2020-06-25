using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SkillTimeDistortionHit : SkillHitTrigger
{
    #region Attributes
    
    private List<Character> affectedCharacters = new List<Character>();
    private List<Skill> affectedSkills = new List<Skill>();

    [Header("Zeitverzerrungs-Info")]
    [InfoBox("Kein Hit Modul verwenden!")]
    [Tooltip("Wert, für Zeitkrümmung. 0 = Stop, 1 = Normal, 2 = Hast")]
    [Range(-100, 100)]
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

        foreach (Character character in this.affectedCharacters)
        {
            StatusEffectUtil.RemoveStatusEffect(this.timeEffect, true, character);
            Invert(character.gameObject, false);
        }

        //Skills wieder normalisieren
        foreach (Skill skill in this.affectedSkills)
        {
            skill.updateTimeDistortion(0);
            Invert(skill.gameObject, false);
        }

        affectedCharacters.Clear();
        affectedSkills.Clear();
    }

    private void OnTriggerExit2D(Collider2D hittedCharacter)
    {
        //Normalisiere Zeit beim Austritt aus dem Feld
        if (CollisionUtil.checkCollision(hittedCharacter, this.skill))
            removeTimeDistorion(hittedCharacter.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        //Setze Zeit beim Eintritt in das Feld
        if (CollisionUtil.checkCollision(hittedCharacter, this.skill))
            setTimeDistorion(hittedCharacter.gameObject, this.TimeDistortion);
    }

    private void OnTriggerStay2D(Collider2D hittedCharacter)
    {
        //Setze Zeit beim Eintritt in das Feld
        if (CollisionUtil.checkCollision(hittedCharacter, this.skill))
            setTimeDistorion(hittedCharacter.gameObject, this.TimeDistortion);
    }

    #endregion


    #region Functions (private)
    private void removeTimeDistorion(GameObject hittedCharacter)
    {
        Invert(hittedCharacter, false);
        Skill skill = AbilityUtil.getSkillByCollision(hittedCharacter);        

        if (skill != null)
        {
            skill.updateTimeDistortion(0);
            this.affectedSkills.Remove(skill);
            return;
        }

        Character character = hittedCharacter.GetComponent<Character>();

        if (character != null)
        {
            StatusEffectUtil.RemoveStatusEffect(this.timeEffect, true, character);
            this.affectedCharacters.Remove(character);
        }
    }

    private void Invert(GameObject hittedCharacter, bool value)
    {
        //CharacterRenderingHandler handler = UnityUtil.GetComponentAll<CharacterRenderingHandler>(hittedCharacter);
        //CustomRenderer customRenderer = UnityUtil.GetComponentAll<CustomRenderer>(hittedCharacter);
        //if (handler != null) handler.Invert(true);
        //else if (customRenderer != null) customRenderer.InvertColors(true);
    }

    private void setTimeDistorion(GameObject hittedCharacter, float destortion)
    {
        Invert(hittedCharacter, true);
        Skill skill = AbilityUtil.getSkillByCollision(hittedCharacter);        

        if (skill != null)
        {
            skill.updateTimeDistortion(destortion);
            if(!this.affectedSkills.Contains(skill)) this.affectedSkills.Add(skill);
            return;
        }

        Character character = hittedCharacter.GetComponent<Character>();

        if (character != null)
        {
            StatusEffectUtil.AddStatusEffect(this.timeEffect, character);
            if (!this.affectedCharacters.Contains(character)) this.affectedCharacters.Add(character);
        }
    }
    #endregion
}
