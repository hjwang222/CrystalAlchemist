using UnityEngine;
using Sirenix.OdinInspector;

public class TitleScreenLoadSaveGames : TitleScreenSaveSlot
{
    [Required]
    [SerializeField]
    [BoxGroup("Main")]
    private TitleScreen titleScreen;

    public void startGame()
    {
        if (this.data != null)
        {
            this.titleScreen.startTheGame(data.scene, this.gameObject.name);
        }
    }

    private void deleteGame()
    {
        //string path = Application.persistentDataPath + "/" + this.gameObject.name + "." + GlobalValues.saveGameFiletype;
        //File.Delete(path);
        //getData();
    }
}
