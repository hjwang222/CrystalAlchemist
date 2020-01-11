using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapPage : MonoBehaviour
{
    public string mapID;    

    [SerializeField]
    private string titleName;
    [SerializeField]
    private string titleNameEnglish;
    [SerializeField]
    private TextMeshProUGUI titleField;

    public GameObject points;

    public bool showMap;

    private void OnEnable()
    {
        this.titleField.text = CustomUtilities.Format.getLanguageDialogText(this.titleName, this.titleNameEnglish);
        if (this.showMap)
        {
            this.gameObject.SetActive(true);
        }
        else this.gameObject.SetActive(false);
    }

}
