using UnityEngine.SceneManagement;

public class Savepoint : Interactable
{
    public override void doSomethingOnSubmit()
    {
        Scene scene = SceneManager.GetActiveScene();

        SaveSystem.Save(this.player, scene.name, "Slot1");

        this.player.GetComponent<PlayerTeleport>().setLastTeleport(scene.name, this.player.transform.position);

        CustomUtilities.DialogBox.showDialog(this, this.player);
    }
}
