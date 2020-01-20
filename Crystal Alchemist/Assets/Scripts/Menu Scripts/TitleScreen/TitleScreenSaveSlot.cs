using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class TitleScreenSaveSlot : MonoBehaviour
{
    [Required]
    [SerializeField]
    [BoxGroup("Main")]
    private string slotName;

    [Required]
    [SerializeField]
    [BoxGroup("Main")]
    private StringValue saveGameSlot;

    [Required]
    [SerializeField]
    [BoxGroup("Main")]
    private string firstScene = "Haus";

    [Required]
    [SerializeField]
    [BoxGroup("Main")]
    private GameObject newGame;

    [Required]
    [SerializeField]
    [BoxGroup("Main")]
    private GameObject loadGame;

    [Required]
    [BoxGroup("Easy Access")]
    [SerializeField]
    private TextMeshProUGUI characterName;

    [Required]
    [BoxGroup("Easy Access")]
    [SerializeField]
    private Image characterPreview;

    [Required]
    [BoxGroup("Easy Access")]
    [SerializeField]
    private TextMeshProUGUI characterLocation;

    [Required]
    [BoxGroup("Easy Access")]
    [SerializeField]
    private TextMeshProUGUI timePlayed;

    private PlayerData data;

    private void OnEnable()
    {
        getData();
    }

    private void getData()
    {
        this.newGame.SetActive(false);
        this.loadGame.SetActive(false);

        this.data = SaveSystem.loadPlayer(this.slotName);

        if (this.data != null)
        {
            string name = data.name;
            float timePlayed = data.timePlayed;
            string ort = data.scene;
            float life = data.health;
            float maxLife = data.maxHealth;
            float mana = data.mana;
            float maxMana = data.maxMana;

            this.loadGame.SetActive(true);
            //show save
        }
        else
        {
            this.newGame.SetActive(true);
            //show empty
        }
    }

    private void startGame()
    {
        if(this.data != null)
        {
            this.saveGameSlot.setValue(this.slotName);
            SceneManager.LoadSceneAsync(data.scene);            
        }
        else
        {
            this.saveGameSlot.setValue(null);
            SceneManager.LoadSceneAsync(this.firstScene);
        }

        Cursor.visible = false;
    }

    private void deleteGame()
    {
        string path = Application.persistentDataPath + "/"+this.slotName+"."+GlobalValues.saveGameFiletype;
        File.Delete(path);
        getData();
    }
}
