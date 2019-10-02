using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Savepoint : Interactable
{
    public override void doSomethingOnSubmit()
    {
        Scene scene = SceneManager.GetActiveScene();

        SaveSystem.Save(this.player, scene.name);

        this.player.GetComponent<PlayerTeleport>().setLastTeleport(scene.name, this.player.transform.position);

        string text = Utilities.Format.getLanguageDialogText(this.dialogBoxText, this.dialogBoxTextEnglish);
        this.player.showDialogBox(text);
    }
}
