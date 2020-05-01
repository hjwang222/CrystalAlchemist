using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class SaveAndLoadScript : MonoBehaviour
{
    [BoxGroup("Save Menu")]
    [SerializeField]
    private MenuDialogBoxLauncher launcher;

    [SerializeField]
    [BoxGroup("Load Menu")]
    [Required]
    private string firstScene = "Haus";

    [SerializeField]
    [BoxGroup("Load Menu")]
    private Vector2 playerPositionInNewScene = new Vector2(0,0);

    [BoxGroup("Required")]
    [Required]
    [SerializeField]
    private TeleportStats teleportStat;

    [BoxGroup("Required")]
    [Required]
    [SerializeField]
    private PlayerSaveGame saveGame;

    [BoxGroup("Required")]
    [Required]
    [SerializeField]
    private CharacterValues playerValue;

    [BoxGroup("Required")]
    [Required]
    [SerializeField]
    private PlayerInventory inventory;

    [BoxGroup("Required")]
    [Required]
    [SerializeField]
    private PlayerButtons buttons;

    [BoxGroup("Required")]
    [Required]
    [SerializeField]
    private CharacterPreset preset;



    public void SaveGame(SaveSlot slot)
    {
        Scene scene = SceneManager.GetActiveScene();

        // SaveSystem.Save(this.player, scene.name, slot.name);
        SaveSystem.Save(this.saveGame, this.playerValue, this.inventory, this.buttons, this.preset);

        if (this.launcher != null) this.launcher.raiseDialogBox();
    }

    public void LoadGame(SaveSlot slot)
    {
        if (slot != null && slot.data != null)
        {
           // this.saveGameSlot.setValue(slot.gameObject.name);
            SceneManager.LoadSceneAsync(slot.data.scene);
            Cursor.visible = false;
        }
    }

    public void LoadGame()
    {
       // this.saveGameSlot.setValue("");
        this.teleportStat.scene = this.firstScene;
        this.teleportStat.position = this.playerPositionInNewScene;

        SceneManager.LoadSceneAsync(this.firstScene);
        Cursor.visible = false;
    }
}
