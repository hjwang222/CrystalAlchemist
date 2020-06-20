using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

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

    public string ID;

    public LocalisationFileType type = LocalisationFileType.objects;

    public UnityEvent eventOnClose;
}


public class DialogSystem : MonoBehaviour
{
    [ButtonGroup("Add Text")]
    private void ScriptableObjects()
    {
        this.textValue = Resources.Load<StringValue>("Scriptable Objects/Menu/DialogText");
        this.eventValue = Resources.Load<EventValue>("Scriptable Objects/Menu/DialogEvent");
    }

    [ButtonGroup("Add Text")]
    private void AddShopText()
    {
        DialogText text = new DialogText();
        text.ID = "Bought";
        text.trigger = DialogTextTrigger.success;
        texts.Add(text);

        text = new DialogText();
        text.ID = "Cannot_Buy";
        text.trigger = DialogTextTrigger.failed;
        texts.Add(text);
    }

    [ButtonGroup("Add Text")]
    private void AddTreasureText()
    {
        DialogText text = new DialogText();
        text.ID = "Obtained";
        text.trigger = DialogTextTrigger.success;
        texts.Add(text);

        text = new DialogText();
        text.ID = "Cannot_Open";
        text.trigger = DialogTextTrigger.failed;
        texts.Add(text);

        text = new DialogText();
        text.ID = "Empty";
        text.trigger = DialogTextTrigger.empty;
        texts.Add(text);
    }

    [BoxGroup("Texts")]
    [SerializeField]
    private List<DialogText> texts = new List<DialogText>();

    [BoxGroup("Required")]
    [SerializeField]
    [Required]
    private StringValue textValue;

    [BoxGroup("Required")]
    [SerializeField]
    [Required]
    private EventValue eventValue;

    private DialogTextTrigger internalTrigger = DialogTextTrigger.none;

    public void SetDialogTrigger(DialogTextTrigger trigger) => this.internalTrigger = trigger;

    public void SetDialogSucces() => this.internalTrigger = DialogTextTrigger.success;

    public void SetDialogEmpty() => this.internalTrigger = DialogTextTrigger.empty;

    public void SetDialogNone() => this.internalTrigger = DialogTextTrigger.none;

    public void SetDialogFailed() => this.internalTrigger = DialogTextTrigger.failed;


    public void showDialog(Player player, Interactable interactable)
    {
        showDialog(player, interactable, null);
    }

    public void showDialog(Player player, Interactable interactable, DialogTextTrigger trigger)
    {
        SetDialogTrigger(trigger);
        showDialog(player, interactable, null);
    }

    public void showDialog(Player player, Interactable interactable, ItemStats loot)
    {
        show(player, interactable, loot);
    }

    public void showDialog(Player player, Interactable interactable, DialogTextTrigger trigger, ItemStats loot)
    {
        SetDialogTrigger(trigger);
        show(player, interactable, loot);
    }

    private void ShowDialogBox(Player player, string text, UnityEvent onClose)
    {
        if (player.values.currentState != CharacterState.inDialog)
        {
            this.textValue.SetValue(text);
            this.eventValue.SetValue(onClose);
            MenuEvents.current.OpenDialogBox();
        }
    }

    private void show(Player player, Interactable interactable, ItemStats loot)
    {
        foreach (DialogText text in this.texts)
        {
            if (text.trigger == this.internalTrigger)
            {
                ShowDialogBox(player, getText(text, interactable.costs.amount, interactable.costs.item, loot, player), text.eventOnClose);
                break;
            }
        }
    }

    private string getText(DialogText text, ItemStats loot, Player player)
    {
        string result = FormatUtil.GetLocalisedText(text.ID, text.type);

        result = result.Replace("<name>", player.name);
        result = result.Replace("<interactable>", getInteractableType());

        if (loot != null)
        {
            result = result.Replace("<loot name>", loot.getName());
            result = result.Replace("<loot amount>", loot.amount + "");
            result = result.Replace("<loot value>", loot.getTotalAmount() + "");
        }
        return result;
    }

    private string getText(DialogText text, float price, ItemGroup item, ItemStats loot, Player player)
    {
        string result = FormatUtil.GetLocalisedText(text.ID, text.type);

        result = result.Replace("<price>", price + "");
        result = result.Replace("<name>", player.name);
        result = result.Replace("<interactable>", getInteractableType());

        if (item != null)
        {
            result = result.Replace("<item name>", item.getName());
            result = result.Replace("<item amount>", item.GetAmount() + "");
        }

        if (loot != null)
        {
            result = result.Replace("<loot name>", loot.getName());
            result = result.Replace("<loot amount>", loot.amount + "");
            result = result.Replace("<loot value>", loot.getTotalAmount() + "");
        }

        return result;
    }

    private string getInteractableType()
    {
        if (this.GetComponent<Door>() != null) return FormatUtil.GetLocalisedText("Door", LocalisationFileType.objects);
        if (this.GetComponent<Treasure>() != null) return FormatUtil.GetLocalisedText("Treasure", LocalisationFileType.objects);
        return "";
    }

    private string GetItemName(float price)
    {
        string result = "";

        /*
        switch (this.resourceType)
        {
            case ResourceType.item:
                {
                    string typ = this.getItemGroup();
                    if (price == 1 && (typ != "Schlüssel" || GlobalGameObjects.settings.useAlternativeLanguage)) typ = typ.Substring(0, typ.Length - 1);

                    result = typ;
                }; break;
            case ResourceType.life: result = "Leben"; break;
            case ResourceType.mana: result = "Mana"; break;
        }*/

        return result;
    }
}
