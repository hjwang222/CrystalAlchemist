using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using System;

public class TitleScreenSaveSlot : MonoBehaviour
{
    [SerializeField]
    [Required]
    [BoxGroup("Main")]
    private BasicMenu mainGameObject;

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
    /*
    [Required]
    [BoxGroup("Easy Access")]
    [SerializeField]
    private Image characterPreview;*/

    [Required]
    [BoxGroup("Easy Access")]
    [SerializeField]
    private TextMeshProUGUI slotname;

    [Required]
    [BoxGroup("Easy Access")]
    [SerializeField]
    private TextMeshProUGUI characterLocation;

    [Required]
    [BoxGroup("Easy Access")]
    [SerializeField]
    private TextMeshProUGUI timePlayed;

    [Required]
    [BoxGroup("Easy Access")]
    [SerializeField]
    private GameObject health;

    [Required]
    [BoxGroup("Easy Access")]
    [SerializeField]
    private GameObject mana;

    public PlayerData data;

    private void OnEnable()
    {
        this.slotname.text = this.gameObject.name;
        getData();
    }

    public void SaveOrStartGame()
    {
        if(this.mainGameObject != null)
        {
            if (this.mainGameObject.GetComponent<SaveGameMenu>() != null) this.mainGameObject.GetComponent<SaveGameMenu>().saveGame(this.gameObject);
            else if (this.mainGameObject.GetComponent<TitleScreen>() != null && this.data != null) this.mainGameObject.GetComponent<TitleScreen>().startTheGame(data.scene, this.gameObject.name);
        }
    }

    public void getData()
    {
        this.newGame.SetActive(false);
        this.loadGame.SetActive(false);

        this.data = SaveSystem.loadPlayer(this.gameObject.name);

        if (this.data != null)
        {
            string name = data.name;
            float timePlayed = data.timePlayed;
            string ort = data.scene;
            float maxLife = data.maxHealth;
            float maxMana = data.maxMana;

            this.loadGame.SetActive(true);

            setLifeMana((int)maxLife, this.health);
            setLifeMana((int)maxMana, this.mana);
            this.characterName.text = name;
            this.characterLocation.text = ort;

            TimeSpan span = TimeSpan.FromMilliseconds(timePlayed);
            this.timePlayed.text = span.ToString(@"hh\:mm\:ss");
        }
        else
        {
            this.newGame.SetActive(true);
        }
    }

    private void setLifeMana(int value, GameObject temp)
    {
        for (int i = 0; i < temp.transform.childCount; i++)
        {
            temp.transform.GetChild(i).gameObject.SetActive(false);
            if (i < value) temp.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}
