using UnityEngine;
using Sirenix.OdinInspector;
using AssetIcons;

[CreateAssetMenu(menuName = "Game/Items/Item Info")]
public class ItemInfo : ScriptableObject
{
    [BoxGroup("Item Texts")]
    [SerializeField]
    private string itemName;

    [BoxGroup("Item Texts")]
    [SerializeField]
    private string itemNameEnglish;

    [Space(10)]
    [BoxGroup("Texts")]
    [TextArea]
    [SerializeField]
    private string description;

    [BoxGroup("Texts")]
    [TextArea]
    [SerializeField]
    private string descriptionEnglish;

    [BoxGroup("Icon")]
    [SerializeField]
    [AssetIcon]
    private Sprite icon;    


    public Sprite getSprite()
    {
        return this.icon;
    }

    public string getDescription()
    {
        return FormatUtil.getLanguageDialogText(this.description, this.descriptionEnglish);
    }

    public string getName()
    {
        return FormatUtil.getLanguageDialogText(this.itemName, this.itemNameEnglish);
    }
}
