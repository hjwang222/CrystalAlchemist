using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class SaveAndLoadScript : MonoBehaviour
{
    [BoxGroup("Save Menu")]
    [Required]
    [SerializeField]
    private StringValue saveGameSlot;

    [BoxGroup("Save Menu")]
    [SerializeField]
    private GameObject success;

    [SerializeField]
    [BoxGroup("Save Menu")]
    [Required]
    private PlayerStats playerStats;

    [SerializeField]
    [BoxGroup("Load Menu")]
    [Required]
    private string firstScene = "Haus";

    [SerializeField]
    [BoxGroup("Load Menu")]
    private Vector2 playerPositionInNewScene = new Vector2(0,0);

    private Player player;

    private void OnEnable()
    {
        this.player = this.playerStats.player;
        if (this.success != null) this.success.SetActive(false);
    }

    public void saveGame(SaveSlot slot)
    {
        Scene scene = SceneManager.GetActiveScene();
        SaveSystem.Save(this.player, scene.name, slot.name);

        if (this.success != null) this.success.SetActive(true);
    }

    public void loadGame(SaveSlot slot)
    {
        if (slot != null && slot.data != null)
        {
            this.saveGameSlot.setValue(slot.gameObject.name);
            SceneManager.LoadSceneAsync(slot.data.scene);
            Cursor.visible = false;
        }
    }

    public void loadGame()
    {
        this.saveGameSlot.setValue("");
        SceneManager.LoadSceneAsync(this.firstScene);
        //this.player.spawnPosition = this.playerPositionInNewScene; TODO
        Cursor.visible = false;
    }
}
