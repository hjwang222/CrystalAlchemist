using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum SupportType
{
    none,
    heal,
    dispell,
    teleport
}

public class BuffSkill : StandardSkill
{
    [FoldoutGroup("Heal and Dispell", expanded: false)]
    [SerializeField]
    private SupportType supportType = SupportType.none;

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
        if (!this.useCollider && this.affectSelf) useSkill(this.sender); 
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
        if (this.supportType == SupportType.heal)
        {
            character.gotHit(this);
        }
        else if (this.supportType == SupportType.dispell)
        {
            if (character.debuffs.Count > 0) Utilities.StatusEffectUtil.RemoveStatusEffect(this.sender.debuffs[0], false, character);
        }
        else if (this.supportType == SupportType.teleport)
        {
            Teleport(character);
        }

        if (this.targetColor != null)
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


    private void Teleport(Character character)
    {
        Player player = this.sender.GetComponent<Player>();

        string scene;
        Vector2 position;

        if (character != null
            && character.isPlayer
            && player != null 
            && player.getLastTeleport(out scene, out position))
        {
            character.GetComponent<Player>().teleportPlayer(scene, position, this.showAnimation);            
        }
    }
}

