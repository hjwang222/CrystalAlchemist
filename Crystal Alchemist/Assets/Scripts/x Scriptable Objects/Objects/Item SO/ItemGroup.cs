using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Game/Items/Item Group")]
public class ItemGroup : ScriptableObject
{
    [SerializeField]
    private string itemGroup;

    [SerializeField]
    private string itemGroupEnglish;

    public string getItemGroup()
    {
        return CustomUtilities.Format.getLanguageDialogText(this.itemGroup, this.itemGroupEnglish);
    }
}
