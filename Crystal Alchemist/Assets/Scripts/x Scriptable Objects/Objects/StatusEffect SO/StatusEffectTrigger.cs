using Sirenix.OdinInspector;

public enum StatusEffectTriggerType
{
    init,
    intervall,
    hit,
    life,
    mana,
    destroyed
}

[System.Serializable]
public class StatusEffectTrigger
{
    public StatusEffectTriggerType triggerType;

    [ShowIf("triggerType", StatusEffectTriggerType.intervall)]
    public float intervall;

    [ShowIf("triggerType", StatusEffectTriggerType.life)]
    public float life;

    [ShowIf("triggerType", StatusEffectTriggerType.mana)]
    public float mana;

    [ShowIf("triggerType", StatusEffectTriggerType.hit)]
    public float hits;

    public bool requireAll = false;


}
