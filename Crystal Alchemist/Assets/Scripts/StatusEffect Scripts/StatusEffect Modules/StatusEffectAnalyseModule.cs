using UnityEngine;

public class StatusEffectAnalyseModule : StatusEffectModule
{
    [SerializeField]
    private BoolValue isActive;

    public override void doAction()
    {
        this.isActive.setValue(true);
    }

    private void OnDestroy()
    {
        this.isActive.setValue(false);
    }
}
