using System.Collections.Generic;
using UnityEngine;

public class UIButtonLayout : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> gamepadUI;

    [SerializeField]
    private List<GameObject> keyboardUI;

    private void setActive(bool value, List<GameObject> gameObjects)
    {
        foreach (GameObject obje in gameObjects) obje.SetActive(value);        
    }

    private void Start() => SettingsEvents.current.OnLayoutChanged += UpdateLayout;

    private void OnEnable() => UpdateLayout();

    private void OnDestroy() => SettingsEvents.current.OnLayoutChanged -= UpdateLayout;  

    private void UpdateLayout()
    {
        if(MasterManager.settings.layoutType == LayoutType.keyboard)
        {
            setActive(false, this.gamepadUI);
            setActive(true, this.keyboardUI);
        }
        else
        {
            setActive(true, this.gamepadUI);
            setActive(false, this.keyboardUI);
        }
    }
}
