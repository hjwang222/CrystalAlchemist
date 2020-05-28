using UnityEngine;
using Sirenix.OdinInspector;

public class ContextClue : MonoBehaviour
{
    [Required]
    [SerializeField]
    [FoldoutGroup("Signals", expanded: true)]
    private SimpleSignal showCurrencyUI;

    [Required]
    [SerializeField]
    [FoldoutGroup("Signals", expanded: true)]
    private FloatSignal hideCurrencyUI;

    [SerializeField]
    [FoldoutGroup("Signals", expanded: true)]
    private float hideDelay = 0f;

    private void OnEnable() => this.showCurrencyUI.Raise();
    
    private void OnDisable() => this.hideCurrencyUI.Raise(this.hideDelay);
   
    private void OnDestroy() => this.hideCurrencyUI.Raise(this.hideDelay);    
}
