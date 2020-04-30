using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectBar : MonoBehaviour
{
    private List<StatusEffectUI> activeStatusEffectUIs = new List<StatusEffectUI>();
    private List<StatusEffect> activeStatusEffects = new List<StatusEffect>();

    [SerializeField]
    private GameObject statusEffectHolder;
    [SerializeField]
    private StatusEffectUI statusEffectGameObject;
    [SerializeField]
    private PlayerStats playerStats;

    private Character character;

    private void Start()
    {
        this.statusEffectGameObject.gameObject.SetActive(false);
        if(this.playerStats != null) setCharacter(this.playerStats.player);
    }

    public void setCharacter(Character character)
    {
        this.character = character;
    }

    public void updateStatusEffect()
    {
        if (character != null)
        {
            //füge ggf. beide Listen hinzu oder selektiere nur eine
            List<StatusEffect> activeStatusEffects = new List<StatusEffect>();

            activeStatusEffects.AddRange(this.character.values.buffs);
            activeStatusEffects.AddRange(this.character.values.debuffs);

            foreach (StatusEffect statusEffect in activeStatusEffects)
            {
                if (!this.activeStatusEffects.Contains(statusEffect))
                {
                    StatusEffectUI statusEffectGUI = Instantiate(this.statusEffectGameObject, this.statusEffectHolder.transform);
                    statusEffectGUI.setUI(statusEffect);
                    statusEffectGUI.gameObject.SetActive(true);

                    this.activeStatusEffects.Add(statusEffect);
                    this.activeStatusEffectUIs.Add(statusEffectGUI);
                }
            }

            foreach(StatusEffectUI statusEffectUI in this.activeStatusEffectUIs)
            {
                statusEffectUI.updateUI();
            }
        }
    }
}
