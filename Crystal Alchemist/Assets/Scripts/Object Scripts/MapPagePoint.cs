using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapPagePoint : MonoBehaviour
{
    [BoxGroup("Required")]
    [Required]
    public string areaID;

    [BoxGroup("Name")]
    public string areaName;
    [BoxGroup("Name")]
    public string areaNameEnglish;

    [BoxGroup("Description")]
    [TextArea]
    public string areaDescription;
    [BoxGroup("Description")]
    [TextArea]
    public string areaDescriptionEnglish;

    [BoxGroup("Icon")]
    public Sprite areaSprite;

    [BoxGroup("Icon")]
    [SerializeField]
    private Image image;

    [BoxGroup("Icon")]
    [SerializeField]
    private TextMeshProUGUI textField;

    private void OnEnable()
    {
        if(this.areaSprite != null) this.image.sprite = this.areaSprite;
        this.textField.text = CustomUtilities.Format.getLanguageDialogText(this.areaName, this.areaNameEnglish);
        this.textField.ForceMeshUpdate(true);
    }

    //TODO: switch to next map on click

}
