using UnityEngine;
using Sirenix.OdinInspector;

public class MiniGameMachine : Interactable
{
    [SerializeField]
    [Required]
    [BoxGroup("Mandatory")]
    private MiniGame miniGame;

    public override void doSomethingOnSubmit()
    {
        Instantiate(this.miniGame);        
    }
}
