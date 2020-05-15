using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class MiniGameMachine : Interactable
{
    [SerializeField]
    [Required]
    [BoxGroup("Mandatory")]
    private MiniGame miniGame;

    [SerializeField]
    [Required]
    [BoxGroup("Mandatory")]
    private MiniGameMatches matches;

    public override void Start()
    {
        base.Start();
    }

    public override void doSomethingOnSubmit()
    {
        MiniGame miniGame = Instantiate(this.miniGame);
        miniGame.setMiniGame(this.matches);

        MenuEvents.current.OpenMiniGame(miniGame);
    }
}
