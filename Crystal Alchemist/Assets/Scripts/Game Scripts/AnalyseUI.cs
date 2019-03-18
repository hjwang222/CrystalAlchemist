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

    private Character character;
    private Interactable interactable;


    private void Start()
    {
        init();
    }

    private void init()
    {
        if (this.target == null)
        {
            this.target = this.transform.parent.gameObject;
        }

        //set type of Analyse
        this.transform.position = new Vector2(this.target.transform.position.x + 1.5f, this.target.transform.position.y + 1.5f);

        if (this.target.GetComponent<Interactable>() != null)
        {
            //set UI to Treasure/Object Info
            this.interactable = this.target.GetComponent<Interactable>();
        }
        else if (this.target.GetComponent<Character>() != null)
        {
            //set UI to Character Info
            this.character = this.target.GetComponent<Character>();

            if (this.character.characterType != CharacterType.Object)
            {
                //show Basic Information for Enemies
                this.TMPcharacterName.enabled = true;
                this.TMPlifeAmount.enabled = true;
                this.heartImage.enabled = true;
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
            if (this.character.characterType != CharacterType.Object) showEnemyInfo();
            else showItem();
        }
        else if (this.interactable != null)
        {
            showItem();
        }

        updateStatusEffects();
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
        this.TMPcharacterName.text = this.character.characterName;
        this.TMPlifeAmount.text = "x" + (this.character.life * 4);

        if (this.character.items.Count > 0 && this.character.currentState != CharacterState.dead)
        {
            this.ImageitemPreview.enabled = true;
            this.ImageitemPreview.sprite = this.character.items[0].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        }
        else this.ImageitemPreview.enabled = false;

        //STatusEffect
    }

    private void showItem()
    {
        if (this.interactable != null)
        {
            //Show Object Information
            if (interactable.items.Count > 0 && this.interactable.currentState != objectState.opened)
            {
                this.ImageObjectitemIndicator.enabled = true;
                this.ImageObjectitemPreview.enabled = true;
                this.ImageObjectitemPreview.sprite = interactable.items[0].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
            }
            else
            {
                this.ImageObjectitemPreview.enabled = false;
                this.ImageObjectitemIndicator.enabled = false;
            }
        }
        else if (this.character != null)
        {
            //Show Object Information
            if (this.character.items.Count > 0 && this.character.currentState != CharacterState.dead)
            {
                this.ImageObjectitemIndicator.enabled = true;
                this.ImageObjectitemPreview.enabled = true;
                this.ImageObjectitemPreview.sprite = this.character.items[0].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
            }
            else
            {
                this.ImageObjectitemPreview.enabled = false;
                this.ImageObjectitemIndicator.enabled = false;
            }
        }
    }

    private void destroyIt()
    {
        //Wenn Truhe geöffnet wurde oder Gegner tot ist
        Destroy(this);
    }
}
