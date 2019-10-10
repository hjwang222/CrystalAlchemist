using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum SupportType
{
    none,
    heal,
    statuseffect,
    teleport
}

public class BuffSkill : StandardSkill
{
    [FoldoutGroup("Heal and Dispell", expanded: false)]
    public SupportType supportType = SupportType.none;

    [FoldoutGroup("Heal and Dispell", expanded: false)]
    [SerializeField]
    [Range(0,10)]
    [ShowIf("supportType", SupportType.statuseffect)]
    private float immortalTimer = 0;

    [FoldoutGroup("Heal and Dispell", expanded: false)]
    [SerializeField]
    [ShowIf("supportType", SupportType.statuseffect)]
    private StatusEffectType affectAllOfKind;

    [FoldoutGroup("Heal and Dispell", expanded: false)]
    [SerializeField]
    [ShowIf("supportType", SupportType.statuseffect)]
    private bool dispellIt = false;

    [FoldoutGroup("Heal and Dispell", expanded: false)]
    [SerializeField]
    [ShowIf("supportType", SupportType.statuseffect)]
    private bool allTheSame = false;

    [FoldoutGroup("Heal and Dispell", expanded: false)]
    [SerializeField]
    [Range(0, 100)]
    [ShowIf("supportType", SupportType.statuseffect)]
    private int extendTimePercentage = 0;

    [FoldoutGroup("Heal and Dispell", expanded: false)]
    [SerializeField]
    private bool useCollider = false;

    [FoldoutGroup("Heal and Dispell", expanded: false)]
    [SerializeField]
    private Color targetColor;

    [FoldoutGroup("Heal and Dispell", expanded: false)]
    [SerializeField]
    [ShowIf("supportType", SupportType.teleport)]
    private bool showAnimation = false;

    #region Overrides

    public override void init()
    {
        base.init();
        if (!this.useCollider 
            && this.GetComponent<SkillTargetModule>() != null
            && this.GetComponent<SkillTargetModule>().affectSelf) useSkill(this.sender); 
    }
    #endregion


    #region Functions (private)

    public override void DestroyIt()
    {
        this.sender.resetColor(this.targetColor);
        base.DestroyIt();
    }

    #endregion


    private void useSkill(Character character)
    {
        //TODO: Module

        if (this.immortalTimer > 0) character.setImmortal(this.immortalTimer);

        if (this.supportType == SupportType.heal)
        {
            character.gotHit(this);
        }
        else if (this.supportType == SupportType.statuseffect)
        {
            changeStatusEffects(character);
            character.gotHit(this);
        }
        else if (this.supportType == SupportType.teleport)
        {
            Teleport(character);
        }

        if (this.targetColor != null && character != null)
        {
            character.addColor(this.targetColor);
        }
    }


    public override void OnTriggerEnter2D(Collider2D hittedCharacter)
    {
        //With delay
        if (Utilities.Collisions.checkCollision(hittedCharacter, this)) useSkill(hittedCharacter.GetComponent<Character>());
    }

    public override void OnTriggerStay2D(Collider2D hittedCharacter)
    {

    }

    public override void OnTriggerExit2D(Collider2D hittedCharacter)
    {

    }


    private void changeStatusEffects(Character character)
    {
        List<StatusEffect> changeEffects = new List<StatusEffect>();
        
            //all effects of a kind
            if (this.affectAllOfKind == StatusEffectType.buff)
            {
                changeEffects.AddRange(character.buffs);
            }
            else
            {
                changeEffects.AddRange(character.debuffs);
            }

        if (this.allTheSame)
        {
            foreach (StatusEffect effect in changeEffects)
            {
                if (this.dispellIt) Utilities.StatusEffectUtil.RemoveStatusEffect(effect, false, character);
                if (this.extendTimePercentage > 0) effect.statusEffectTimeLeft += (effect.statusEffectTimeLeft * extendTimePercentage) / 100;
            }
        }
        else
        {
            if (this.dispellIt) Utilities.StatusEffectUtil.RemoveStatusEffect(changeEffects[0], false, character);
            if (this.extendTimePercentage > 0) changeEffects[0].statusEffectTimeLeft += (changeEffects[0].statusEffectTimeLeft * extendTimePercentage) / 100;
        }        
    }


    private void Teleport(Character character)
    {
        Player player = this.sender.GetComponent<Player>();

        string scene;
        Vector2 position;

        if (character != null
            && character.isPlayer
            && player != null 
            && player.GetComponent<PlayerTeleport>().getLastTeleport(out scene, out position))
        {
            character.GetComponent<Player>().GetComponent<PlayerTeleport>().teleportPlayer(scene, position, this.showAnimation);            
        }
    }
}

