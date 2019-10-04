using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AnalyseUI : MonoBehaviour
{
    [Header("Gegner-Info")]
    public GameObject target;

    [Header("Easy Access Objects")]
    public TextMeshProUGUI TMPcharacterName;
    public TextMeshProUGUI TMPlifeAmount;
    public Image heartImage;
    public Image ImageitemPreview;
    public Image ImageObjectitemIndicator;
    public Image ImageObjectitemPreview;
    public GameObject statusEffectHolder;

    [Header("Gruppen")]
    [SerializeField]
    private GameObject enemyInfo;
    [SerializeField]
    private GameObject objectInfo;
    [SerializeField]
    private AggroBar aggrobar;

    private Character character;
    private Interactable interactable;


    private void Start()
    {
        init();
    }

    private void init()
    {
        this.enemyInfo.SetActive(false);
        this.objectInfo.SetActive(false);
        this.aggrobar.gameObject.SetActive(false);

        //set type of Analyse
        this.transform.position = new Vector2(this.target.transform.position.x + 1.5f, this.target.transform.position.y + 1.5f);

        if (this.target.GetComponent<Interactable>() != null)
        {
            //set UI to Treasure/Object Info
            this.interactable = this.target.GetComponent<Interactable>();
            this.objectInfo.SetActive(true);
        }
        else if (this.target.GetComponent<Character>() != null)
        {
            //set UI to Character Info
            this.character = this.target.GetComponent<Character>();

            if (this.character.stats.characterType != CharacterType.Object)
            {
                //show Basic Information for Enemies
                this.enemyInfo.SetActive(true);
            }
            else this.objectInfo.SetActive(true);

            AI enemy = this.character.GetComponent<AI>();
            if (enemy != null)
            {
                this.aggrobar.gameObject.SetActive(true);
                this.aggrobar.setEnemy(enemy);
            }
        }
    }

    private void LateUpdate()
    {
        updateInfos();
    }

    private void updateInfos()
    {
        if (this.character != null)
        {
            if (this.character.stats.characterType != CharacterType.Object)
            {
                showEnemyInfo();
                updateStatusEffects();
            }
            else showItemInfo();
        }
        else if (this.interactable != null)
        {
            showItemInfo();
        }
    }

    private void updateStatusEffects()
    {
        if (this.character != null)
        {
            for (int i = 1; i < this.statusEffectHolder.transform.childCount; i++)
            {
                if (this.statusEffectHolder.transform.GetChild(i).gameObject.activeInHierarchy) Destroy(this.statusEffectHolder.transform.GetChild(i).gameObject);
            }

            //füge ggf. beide Listen hinzu oder selektiere nur eine
            List<StatusEffect> effectList = new List<StatusEffect>();

            effectList.AddRange(this.character.buffs);
            effectList.AddRange(this.character.debuffs);

            for (int i = 0; i < effectList.Count; i++)
            {
                //Make a copy
                GameObject statusEffectGUI = Instantiate(this.statusEffectHolder.transform.GetChild(0), this.statusEffectHolder.transform).gameObject;

                StatusEffect statusEffectFromList = effectList[i];
                statusEffectGUI.GetComponent<Image>().sprite = statusEffectFromList.iconSprite;

                string seconds = Mathf.RoundToInt(statusEffectFromList.statusEffectTimeLeft) + "s";
                if (statusEffectFromList.endType == StatusEffectEndType.mana || statusEffectFromList.statusEffectDuration == Utilities.maxFloatInfinite) seconds = "";

                statusEffectGUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = seconds;

                statusEffectGUI.SetActive(true);
                statusEffectGUI.hideFlags = HideFlags.HideInHierarchy;
            }
        }
    }

    private void showEnemyInfo()
    {
        this.ImageitemPreview.gameObject.SetActive(false);

        this.TMPcharacterName.text = Utilities.Format.getLanguageDialogText(this.character.stats.characterName, this.character.stats.englischCharacterName);
        this.TMPlifeAmount.text = "x" + (this.character.life * 4);

        if (this.character.inventory.Count > 0 && this.character.currentState != CharacterState.dead)
        {
            this.ImageitemPreview.gameObject.SetActive(true);
            this.ImageitemPreview.sprite = this.character.inventory[0].itemSprite;
        }
    }

    private void showItemInfo()
    {
        this.ImageObjectitemIndicator.gameObject.SetActive(false);

        if (this.interactable != null)
        {
            //Show Object Information
            if (interactable.inventory.Count > 0 && this.interactable.currentState != objectState.opened)
            {
                this.ImageObjectitemIndicator.gameObject.SetActive(true);
                this.ImageObjectitemPreview.sprite = interactable.inventory[0].itemSprite;
            }            
        }
        else if (this.character != null)
        {
            //Show Object Information
            if (this.character.inventory.Count > 0 && this.character.currentState != CharacterState.dead)
            {
                this.ImageObjectitemIndicator.gameObject.SetActive(true);
                this.ImageObjectitemPreview.sprite = this.character.inventory[0].itemSprite;
            }            
        }
    }

    private void destroyIt()
    {
        //Wenn Truhe geöffnet wurde oder Gegner tot ist
        Destroy(this);
    }
}
