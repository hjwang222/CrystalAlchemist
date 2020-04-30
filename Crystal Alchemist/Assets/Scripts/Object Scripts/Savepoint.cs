using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class Savepoint : Interactable
{
    [BoxGroup("SavePoint")]
    [SerializeField]
    private SimpleSignal openSaveMenu;

    public override void doSomethingOnSubmit()
    {
        Scene scene = SceneManager.GetActiveScene();
        this.player.GetComponent<PlayerTeleport>().AddTeleport(scene.name, this.player.transform.position);
        openSaveMenu.Raise();        
    }
}
