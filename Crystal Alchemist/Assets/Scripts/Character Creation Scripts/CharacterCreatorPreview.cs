using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class CharacterCreatorPreview : MonoBehaviour
{
    [BoxGroup("Required")]
    [SerializeField]
    private CharacterPreset preset;

    [BoxGroup("Required")]
    [SerializeField]
    private List<GameObject> previews = new List<GameObject>();

    private void Start()
    {
        UpdatePreview();
    }

    private void UpdatePreview()
    {
        setImage();        
    }

    private void setImage()
    {
        for (int i = 0; i < this.previews.Count; i++)
        {
            foreach (Transform imageGO in previews[i].transform)
            {
                CharacterPreviewData data = preset.getPreviewData(imageGO.name);
                if (data != null)
                {
                    if(data.previewImages[i] != null) imageGO.GetComponent<Image>().sprite = data.previewImages[i];
                    imageGO.GetComponent<Image>().color = data.previewColor;                    
                }
                else imageGO.gameObject.SetActive(false);
            }
        }
    }
}
