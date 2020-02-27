using UnityEngine;
using Sirenix.OdinInspector;

public class SignalInteractable : Interactable
{
    [Required]
    [BoxGroup("Mandatory")]
    [SerializeField]
    private SimpleSignal openMenuSignal;

    public override void doSomethingOnSubmit()
    {       
        openMenuSignal.Raise();
    }
}
