using UnityEngine;
using Sirenix.OdinInspector;

public class MiniGameMachine : Interactable
{
    [SerializeField]
    [Required]
    [BoxGroup("Mandatory")]
    private MiniGameInfo miniGame;

    [SerializeField]
    [Required]
    [BoxGroup("Mandatory")]
    private MiniGameInfo UIInfo;

    public override void doSomethingOnSubmit()
    {
        this.UIInfo = this.miniGame;
        MenuEvents.current.OpenMiniGame();
    }
}
