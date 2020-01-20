using UnityEngine;
using UnityEngine.SceneManagement;

public class Savepoint : Interactable
{
    [SerializeField]
    private SimpleSignal openSaveMenu;

    public override void doSomethingOnSubmit()
    {
        Scene scene = SceneManager.GetActiveScene();
        this.player.GetComponent<PlayerTeleport>().setLastTeleport(scene.name, this.player.transform.position);
        openSaveMenu.Raise();        
    }
}
