using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public enum DialogTextTrigger
{
    none,
    success,
    failed, 
    empty
}

[System.Serializable]
public class DialogText
{
    public DialogTextTrigger trigger;

    [Tooltip("Anzeige-Text für die Dialog-Box")]
    [TextArea]
    public string dialogBoxText;

    [Tooltip("Englischer Anzeige-Text für die Dialog-Box")]
    [TextArea]
    public string dialogBoxTextEnglish;
}


public class DialogSystem : MonoBehaviour
{
    [ButtonGroup("Add Text")]
    private void AddShopText()
    {
        DialogText text = new DialogText();
        text.dialogBoxText = "Du hast <loot amount> <loot name> für <price> <item name> gekauft!";
        text.dialogBoxTextEnglish = "You bought <loot amount> <loot name> for <price> <item name>!";
        text.trigger = DialogTextTrigger.success;
        texts.Add(text);

        text = new DialogText();
        text.dialogBoxText = "Du kannst das nicht kaufen.\nDu benötigst <price> <item name>.";
        text.dialogBoxTextEnglish = "You cant buy that.\nYou need <price> <item name>.";
        text.trigger = DialogTextTrigger.failed;
        texts.Add(text);
    }

    [ButtonGroup("Add Text")]
    private void AddTreasureText()
    {
        DialogText text = new DialogText();
        text.dialogBoxText = "Du hast <loot amount> <loot name> erhalten!";
        text.dialogBoxTextEnglish = "You obtained <loot amount> <loot name>!";
        text.trigger = DialogTextTrigger.success;
        texts.Add(text);

        text = new DialogText();
        text.dialogBoxText = "Du kannst d<interactable> nicht öffnen. Du benötigst <price> <item name>.";
        text.dialogBoxTextEnglish = "You cant open the <interactable>. You need <price> <item name>.";
        text.trigger = DialogTextTrigger.failed;
        texts.Add(text);

        text = new DialogText();
        text.dialogBoxText = "D<interactable> ist leer... .";
        text.dialogBoxTextEnglish = "The <interactable> is empty... .";
        text.trigger = DialogTextTrigger.empty;
        texts.Add(text);
    }

    [SerializeField]
    private List<DialogText> texts = new List<DialogText>();

    public void show(Player player, Interactable interactable, Item loot)
    {
        if (this.texts.Count > 0)
        {
            player.showDialogBox(getText(this.texts[0], interactable.price, interactable.item, loot, player));
        }
    }

    public void show(Player player, DialogTextTrigger trigger, Interactable interactable, Item loot)
    {
        foreach(DialogText text in this.texts)
        {
            if(text.trigger == trigger)
            {
                player.showDialogBox(getText(text, interactable.price, interactable.item, loot, player));
                break;
            }
        }
    }

    private string getText(DialogText text, int price, Item item, Item loot, Player player)
    {
        string result = CustomUtilities.Format.getLanguageDialogText(text.dialogBoxText, text.dialogBoxTextEnglish);

        result = result.Replace("<price>", price + "");
        result = result.Replace("<name>", player.name);
        result = result.Replace("<interactable>", getInteractableType());

        if (item != null)
        {
            result = result.Replace("<item name>", getItemName(price, item));
            result = result.Replace("<item amount>", item.amount + "");
            result = result.Replace("<item value>", item.getTotalAmount() + "");
        }

        if (loot != null)
        {
            result = result.Replace("<loot name>", CustomUtilities.Format.getLanguageDialogText(loot.itemName, loot.itemNameEnglish));
            result = result.Replace("<loot amount>", loot.amount + "");
            result = result.Replace("<loot value>", loot.getTotalAmount() + "");
        }

        return result;
    }

    private string getInteractableType()
    {
        if (this.GetComponent<Door>() != null) return CustomUtilities.Format.getLanguageDialogText("ie Tür", "door");
        if (this.GetComponent<Treasure>() != null) return CustomUtilities.Format.getLanguageDialogText("ie Truhe", "chest");
        return "";
    }

    private string getItemName(int price, Item item)
    {
        string result = "";

        switch (item.resourceType)
        {
            case ResourceType.item:
                {
                    if (item.isKeyItem)
                    {
                        result = CustomUtilities.Format.getLanguageDialogText(item.itemName, item.itemNameEnglish);
                    }
                    else
                    {
                        string typ = CustomUtilities.Format.getLanguageDialogText(item.itemGroup, item.itemGroupEnglish);
                        if (price == 1 && (typ != "Schlüssel" || GlobalValues.useAlternativeLanguage)) typ = typ.Substring(0, typ.Length - 1);

                        result = typ;
                    }
                }; break;
            case ResourceType.life: result = "Leben"; break;
            case ResourceType.mana: result = "Mana"; break;
        }

        return result;
    }
}
