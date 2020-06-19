using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class AIInteraction : Interactable
{
    [BoxGroup("NPC")]
    [SerializeField]
    [Required]
    private AI npc;

    [BoxGroup("NPC")]
    [SerializeField]
    private UnityEvent onSubmit;

    public override void Start()
    {
        base.Start();
        this.context.transform.position = new Vector2(this.context.transform.position.x, this.context.transform.position.y + this.npc.GetHeight());
    }

    public override void DoOnSubmit()
    {
        this.onSubmit?.Invoke();
    }

    public void TurnHostile()
    {
        this.npc.values.characterType = CharacterType.Enemy;
    }

    public void TurnFriendly()
    {
        this.npc.values.characterType = CharacterType.Friend;
    }

    public void SetAggro(float value)
    {
        if (value > 0) GameEvents.current.DoAggroIncrease(this.npc, this.player, value);
        else GameEvents.current.DoAggroDecrease(this.npc, this.player, value);
    }

    public override bool PlayerIsLooking()
    {
        if (this.isPlayerInRange
            && this.npc.values.characterType == CharacterType.Friend
            && CollisionUtil.checkIfGameObjectIsViewed(this.player, this.npc.gameObject)) return true;
        return false;
    }
}
